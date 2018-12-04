#pylint: disable=E1129

import tensorflow as tf

class Policy:
    def __init__(self, state_size, action_size, **kwargs):
        self.state_size = state_size
        self.action_size = action_size

        self.lrate = kwargs['lrate'] if 'lrate' in kwargs else 1e-4
        self.ratio_clip = kwargs['ratio_clip'] if 'ratio_clip' in kwargs else 0.2
        self.beta = kwargs['beta'] if 'beta' in kwargs else 0.01
        self.g_clip = kwargs['g_clip'] if 'g_clip' in kwargs else 0.5

        self.create_graph(state_size, action_size, **kwargs)

    def create_graph(self, state_size, action_size, **kwargs):
        self.train_graph = tf.Graph()
        with self.train_graph.as_default():

            # placeholders
            self.state = tf.placeholder(tf.float32, shape=(None, state_size), name='state')
            self.batch_size = tf.placeholder(tf.int32, shape=(), name='batch_size')
            self.action_ph = tf.placeholder(tf.float32, shape=(None,action_size))

            # common
            fc1 = tf.nn.relu(tf.layers.dense(self.state, 100))
            fc2 = tf.nn.relu(tf.layers.dense(fc1, 100))

            # actor 
            with tf.variable_scope('actor'):
                mean = tf.layers.dense(fc2, action_size, name='mean')
                std = tf.get_variable('std', shape=(1, action_size), initializer=tf.zeros_initializer())
                dist = tf.distributions.Normal(mean, tf.nn.softplus(std))
                self.action = dist.sample()
                self.log_prob = dist.log_prob(self.action)
                self.log_prob_a = dist.log_prob(self.action_ph)
                self.entropy = dist.entropy()

            tf.identity(self.log_prob, name='log_prob')
            tf.identity(self.action, name='action')

            # critic
            with tf.variable_scope('critic'):
                self.vf = tf.layers.dense(fc2, 1, name='vf') # state value, from critic

            # for training
            with tf.variable_scope('train'):
                # placeholders for training 
                self.old_log_prob = tf.placeholder(tf.float32, shape=(None, self.action_size), name='old_log_prob')
                self.adv = tf.placeholder(tf.float32, shape=(None), name='advantage')
                self.rtrn = tf.placeholder(tf.float32, shape=(None), name='return')

                self.ratio = tf.exp(self.log_prob_a - self.old_log_prob) # log prob of selected actions
                self.clip = tf.clip_by_value(self.ratio, 1-self.ratio_clip, 1+self.ratio_clip)
                self.clipped_surrogate = tf.minimum(self.ratio*self.adv, self.clip*self.adv)

                self.actor_loss = -tf.reduce_mean(self.clipped_surrogate) - self.beta * tf.reduce_mean(self.entropy)
                self.critic_loss = tf.losses.huber_loss(self.vf, self.rtrn)
                tf.summary.scalar('actor_loss', self.actor_loss)
                tf.summary.scalar('critic_loss', self.critic_loss)

                self.loss = self.actor_loss + self.critic_loss

                optimizer = tf.train.AdamOptimizer(learning_rate = self.lrate)

                # calculate gradients and clip by norm
                gradients = optimizer.compute_gradients(self.loss)
                capped_gradients = [(tf.clip_by_norm(grad, self.g_clip), var) for grad, var in gradients]
                self.train_op = optimizer.apply_gradients(capped_gradients)

            self.init = tf.global_variables_initializer() # init op
            self.saver = tf.train.Saver() # save op
            self.summaries = tf.summary.merge_all()    

            

