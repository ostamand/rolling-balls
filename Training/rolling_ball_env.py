from mlagents.envs.environment import UnityEnvironment
import numpy as np

class RollingBallEnv:
    def __init__(self, filename='data/RollingBall/RollingBall.exe', no_graphics=True):
        self.env = UnityEnvironment(filename, no_graphics=no_graphics)
        self.brain_name = self.env.brain_names[0]
        brain = self.env.brains[self.brain_name]
        self.action_size = brain.vector_action_space_size[0]
        env_info = self.env.reset(train_mode=True)[self.brain_name] 
        self.state_size = env_info.vector_observations[0].shape[0]
        self.num_agents = len(env_info.agents)

    def reset(self, train=True):
        # reset environment
        env_info = self.env.reset(train_mode=train)[self.brain_name]
        return env_info.vector_observations

    def step(self, action):
        env_info = self.env.step(action)[self.brain_name]
        next_state = env_info.vector_observations
        reward = env_info.rewards
        done = env_info.local_done
        return (next_state, np.array(reward), np.array(done))

    def sample(self):
        return (np.random.rand(2) * 2)-1

    def close(self):
        self.env.close()