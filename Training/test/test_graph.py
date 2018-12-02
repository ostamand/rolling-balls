import unittest
from model.ppo import Policy
import tensorflow as tf
from rolling_ball_env import RollingBallEnv
from agent_ppo import Agent

class TestGraph(unittest.TestCase):

    def setUp(self):
        self.env = RollingBallEnv()
        self.state_size = self.env.state_size
        self.action_size = self.env.action_size

    def tearDown(self):
        self.env.close()

    def test_can_actor_graph(self):
        p = Policy(self.state_size, self.action_size)
        # get action and log_prob from actor graph
        state = p.train_graph.get_tensor_by_name('state:0')
        action = p.train_graph.get_tensor_by_name('action:0')

        # sample actions
        with tf.Session(graph=p.train_graph) as sess:
            sess.run(tf.global_variables_initializer())
            state = self.env.reset()
            # get action from initial state of env. 
            actions = sess.run(action, feed_dict={p.batch_size: 1, p.state: state.reshape(-1,self.state_size)})
            self.assertEqual(actions.shape, (1, self.action_size))

    def test_can_step_agent(self):
        # create policy with environment
        policy = Policy(self.state_size, self.action_size)

        # create agent with policy and environment 
        agent = Agent(self.env, policy, nsteps=200, nbatchs=10)

        # step the agent
        agent.step()

