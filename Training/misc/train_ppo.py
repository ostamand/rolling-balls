import pickle
import numpy as np
import tensorflow as tf 

def train(agent,
          iterations=1000,
          log_each=10,
          solved=90,
          out_file=None):

    # writer session graph of agent
    
    with agent.sess as sess:
        writer = tf.summary.FileWriter('logs', sess.graph)

        # add score logging
        score_ph = tf.placeholder(tf.float32, shape=(), name='score')
        score_summ = tf.summary.scalar('score', score_ph)

        rewards = []
        last_saved = 0
        for it in range(iterations):
            frac = 1.0 - it / (iterations-1)
            agent.step()

            if len(agent.episodes_reward) >= 100:
                r = agent.episodes_reward[:-101:-1]
                rewards.append((agent.steps, min(r), max(r), np.mean(r), np.std(r)))

            if (it+1) % log_each == 0:
                summary = ''
                #pylint: disable=line-too-long
                if rewards:
                    mean = rewards[-1][3]
                    minimum = rewards[-1][1]
                    maximum = rewards[-1][2]
                    summary = f', Rewards: {mean:.2f}/{rewards[-1][4]:.2f}/{minimum:.2f}/{maximum:.2f} mean/std/min/max'

                    # logging score to tensorflow 
                    summ = sess.run([score_summ], feed_dict={score_ph: mean})
                    writer.add_summary(summ[-1], it)

                    if out_file and mean >= solved and mean > last_saved:
                        last_saved = mean
                        #agent.save(out_file)
                        summary += " (saved)"

                print(f"Iteration: {it+1:d}, Episodes: {len(agent.episodes_reward)}, Steps: {agent.steps:d}{summary}")

        pickle.dump(rewards, open('rewards.p', 'wb'))                                   
