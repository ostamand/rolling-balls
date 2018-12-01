from rolling_ball_env import RollingBallEnv

def watch():
    env = RollingBallEnv(no_graphics=False)
    env.reset(train=False)
    score = 0
    for step_i in range(200):
        actions = env.sample() # sample random actions
        _, reward, _ = env.step(actions)
        score += reward
    env.close()
    print(f'Score: {score:.2f}')

if __name__ == '__main__':
    watch()