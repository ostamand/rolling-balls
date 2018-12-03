import os
import pdb
import numpy as np
import tensorflow as tf

class Agent():
    """PPO Agent

    Args
        env:                Environment wrapper
        policy:             Network to optimise
        nsteps:             Number of steps per iteration is nsteps * num_agents
        epochs:             Number of training epoch per iteration
        nbatchs:            Number of batch per training epoch
        ratio_clip:         Probability ratio clipping
        lrate:              Learning rate 
        beta:               Policy entropy coefficient
        gae_tau:            GAE tau. Advantage estimation discounting factor
        gamma:              Discount rate 
        gradient_clip       Gradient norm clipping
        restore
    """
    def __init__(self, env, policy,
                 nsteps=200, epochs=10, nbatchs=32, gae_tau=0.95, gamma=0.99):
        self.env = env
        self.p = policy
        # create session from policy graph
        self.sess = tf.Session(graph=self.p.train_graph)
        self.sess.run(self.p.init)

        self.nsteps = nsteps
        self.gamma = gamma
        self.epochs = epochs
        self.nbatchs = nbatchs
        self.gae_tau = gae_tau

        self.state = self.env.reset()

        self.rewards = np.zeros(self.num_agents)
        self.episodes_reward = []
        self.steps = 0

        assert((self.nsteps * self.num_agents) % self.nbatchs == 0)

    @property
    def num_agents(self):
        return self.env.num_agents

    @property
    def action_size(self):
        return self.env.action_size

    @property
    def state_size(self):
        return self.env.state_size

    def get_batch(self, states, actions, old_log_probs, returns, advs):
        length = states.shape[0] # nsteps * num_agents
        batch_size = int(length / self.nbatchs)
        idx = np.random.permutation(length)
        for i in range(self.nbatchs):
            rge = idx[i*batch_size:(i+1)*batch_size]
            yield (
                states[rge], actions[rge], old_log_probs[rge], returns[rge], advs[rge]
                )

    def step(self):
        trajectory_raw = []
        for _ in range(self.nsteps):
            inference_dict = {self.p.state: self.state.reshape(-1, self.state_size)}
            action, log_p, value = self.sess.run([self.p.action, self.p.log_prob, self.p.vf], feed_dict=inference_dict)
            next_state, reward, done = self.env.step(action)
            self.rewards += reward

            # check if some episodes are done
            for i, d in enumerate(done):
                if d:
                    self.episodes_reward.append(self.rewards[i])
                    self.rewards[i] = 0

            trajectory_raw.append((self.state, action, reward, log_p, value, 1-done))
            self.state = next_state

        next_value = self.sess.run([self.p.vf], feed_dict={self.p.state: self.state.reshape(-1, self.state_size) })[-1]
        trajectory_raw.append((self.state, None, None, None, next_value, None))
        trajectory = [None] * (len(trajectory_raw)-1)
        # process raw trajectories
        # calculate advantages and returns
        advs = np.zeros((self.num_agents, 1))
        R = next_value

        for i in reversed(range(len(trajectory_raw)-1)):

            states, actions, rewards, log_probs, values, dones = trajectory_raw[i]
            next_values = trajectory_raw[i+1][-2]

            R = rewards[:,None] + self.gamma * R * dones[:,None]
            # without gae, advantage is calculated as:
            td_errors = rewards[:,None] + self.gamma * dones[:,None] * next_values - values
            advs = advs * self.gae_tau * self.gamma * dones[:,None] + td_errors
            # with gae
            trajectory[i] = (states, actions, log_probs, R.squeeze(1), advs.squeeze(1))

        states, actions, old_log_probs, returns, advs = map(lambda x: np.concatenate(x, axis=0), zip(*trajectory))

        # normalize advantages
        advs = (advs - advs.mean())  / (advs.std() + 1.0e-10)

        # train policy with random batchs of accumulated trajectories
        for _ in range(self.epochs):

            for states_b, actions_b, old_log_probs_b, returns_b, advs_b in \
                self.get_batch(states, actions, old_log_probs, returns, advs):

                f_dict = {self.p.state: states_b, 
                          self.p.action_ph: actions_b, 
                          self.p.old_log_prob: old_log_probs_b, 
                          self.p.adv: advs_b[:,None], 
                          self.p.rtrn: returns_b[:,None]
                         }
                a_loss, c_loss, _ = self.sess.run([self.p.actor_loss, self.p.critic_loss, self.p.train_op], feed_dict=f_dict)
                #self.sess.run([self.p.clip], feed_dict=f_dict)

        # steps of the environement processed by the agent 
        self.steps += self.nsteps * self.num_agents
