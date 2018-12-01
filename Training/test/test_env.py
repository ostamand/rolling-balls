import unittest
import numpy as np
from rolling_ball_env import RollingBallEnv

class TestEnv(unittest.TestCase):

    def setUp(self):
        self.env = RollingBallEnv()

    def tearDown(self):
        self.env.close()

    def test_can_create_env(self):
        self.assertEqual(self.env.action_size, 2)
        self.assertEqual(self.env.state_size, 8)

    def test_can_step_env(self):
        self.env.reset()
        actions = (np.random.rand(2) * 2)-1 # actions are btwn -1..1 
        next_state, reward, done = self.env.step(actions)
        self.assertNotEqual(reward, 0)
        self.assertEqual(next_state.shape[0], 8)
        self.assertTrue(type(done), bool)
