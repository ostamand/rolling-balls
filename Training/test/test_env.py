import unittest
import numpy as np
from rolling_ball_env import RollingBallEnv

class TestEnv(unittest.TestCase):

    def test_can_create_env(self):
        env = RollingBallEnv()
        self.assertEqual(env.action_size, 2)
        self.assertEqual(env.state_size, 8)
        env.close()

    def test_can_step_env(self):
        env = RollingBallEnv()
        env.reset()
        actions = (np.random.rand(2) * 2)-1 # actions are btwn -1..1 
        next_state, reward, done = env.step(actions)
        self.assertNotEqual(reward, 0)
        self.assertEqual(next_state.shape[0], 8)
        self.assertTrue(type(done), bool)
        env.close()

    def test_num_agents(self):
        env =RollingBallEnv(filename='data/RollingBall_v2/RollingBall_v2.exe')
        self.assertEqual(env.num_agents, 4)
        env.close()
