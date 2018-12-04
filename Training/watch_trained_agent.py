import tensorflow as tf
from rolling_ball_env import RollingBallEnv
import numpy as np

def watch_trained(filename, episodes):
    # create environment 
    env = RollingBallEnv('data/RollingBall_v2/RollingBall_v2.exe', no_graphics=False)    

    # load graph
    loaded_graph = tf.Graph()
    with loaded_graph.as_default():
        loader = tf.train.import_meta_graph(filename + '.meta')
        state_a = loaded_graph.get_tensor_by_name('state:0')
        action_a = loaded_graph.get_tensor_by_name('action:0')

    with tf.Session(graph=loaded_graph) as sess:
        loader.restore(sess, filename)
        running_score = np.zeros((env.num_agents))
        scores = []
        state = env.reset(train=False)
        while len(scores) <= episodes:
            action = sess.run(action_a, feed_dict={state_a: state})
            state, reward, done = env.step(action)
            running_score += reward
            for i in range(env.num_agents):
                if done[i]:
                    scores.append(running_score[i])
                    running_score[i] = 0

    env.close()
    print(f"Average score: {np.mean(scores):.2f}")

if __name__ == '__main__':
    watch_trained('saved_models/test.ckpt', 1000)
    
    