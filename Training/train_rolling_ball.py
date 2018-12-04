from misc.train_ppo import train
from rolling_ball_env import RollingBallEnv
from agent_ppo import Agent
from model.ppo import Policy
import tensorflow as tf

# TODO check if std is trained properly 
# TODO check trained results 
# TODO lrate scheduler 

if __name__ == '__main__':
    #pylint: disable=invalid-name

    iterations = 10000
    gamma = 0.99
    nsteps = 2048
    ratio_clip = 0.2
    nbatchs = 128
    epochs = 10
    gradient_clip = 0.5
    lrate = 1e-4
    log_each = 1
    beta = 0.0
    gae_tau = 0.95
    solved = -5.0
    out_file = 'saved_models/v1/agent.ckpt'

    # environment 
    env = RollingBallEnv(filename='data/RollingBall_v2/RollingBall_v2.exe', no_graphics=False)

    # policy
    policy = Policy(env.state_size, env.action_size,
        lrate=lrate,
        ratio_clip=ratio_clip,
        beta=beta,
        g_clip = gradient_clip
    )

    # agent
    a = Agent(
        env,
        policy,
        nsteps=nsteps,
        epochs=epochs,
        nbatchs=nbatchs,
        gae_tau=gae_tau,
        gamma = gamma
    )

    train(a, iterations=iterations, log_each=log_each,
          solved=solved, out_file=out_file)
