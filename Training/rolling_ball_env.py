from mlagents.envs.environment import UnityEnvironment
import numpy as np

class RollingBallEnv:
    def __init__(self, filename='data/RollingBall/RollingBall.exe', no_graphics=True):
        self.env = UnityEnvironment(filename, no_graphics=no_graphics)
        self.brain_name = self.env.brain_names[0]
        brain = self.env.brains[self.brain_name]
        self.action_size = brain.vector_action_space_size[0]
        state = self.reset()
        self.state_size = state.shape[0]

    def reset(self, train=True):
        # reset environment
        env_info = self.env.reset(train_mode=train)[self.brain_name]
        return env_info.vector_observations[0]

    def step(self, action):
        env_info = self.env.step(action)[self.brain_name]
        next_state = env_info.vector_observations[0]
        reward = env_info.rewards[0]
        done = env_info.local_done[0]
        return (next_state, reward, done)

    def sample(self):
        return (np.random.rand(2) * 2)-1

    def close(self):
        self.env.close()