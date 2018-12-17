# Rolling Balls of Christmas

![logo](/doc/rolling_balls_logo.png)

## Introduction

Rolling Balls of Christmas is a fast-paced game where you get to play against an AI opponent trained through a reinforcement learning algorithm.

The rules are simple. Both the player (in green) and the opponent (in red) control a ball sitting on a square platform while a gift (the target) is also located on it. Each time either the player or the opponent reaches the gift it gets one point. Inversely, each time a ball falls off the platform the respective opponent gets one point. The first one to get to 20 points wins the game. Each level the opponents gets better.

![logo](/doc/gameplay.png)

- To play the game navigate to the [Download](#download) section. Available for Windows and Android.
- To watch me play a game follow this [youtube link](https://youtu.be/OQNvxL2XfUI).
- To learn more on how the opponent was trained using reinforcement learning follow this [link](https://ostamand.github.io/rl/2018/12/08/unity-ai.html) to my blog. 
- To train your own agent using the provided training environement with 12 parallel agents use these [steps](#installation)


## Download

The game is available for Windows & Android. 

- [Link for Windows](https://drive.google.com/open?id=1AP2-EKezVETNA5LhOVAkjuF8rWsINX_r). Extract the zipped folder & double click on the .exe to play.
- [Link for Android](https://drive.google.com/open?id=14C-qxHd1at3j9scWi_Z3vZ913qOUQUcd). Download to your device and install to easily add to your apps.

To move the ball on Windows use either the arrows or the W (up), A (left), S (down) and D (down) keys. On a mobile device use the provided control at the bottom right of the screen.

## Installation

To train your own agent follow the steps below.

Create a Python virtual environment.

```
conda create --name rolling-balls python=3.6 
activate rolling-balls
```

Install ml-agents.

```
git clone https://github.com/Unity-Technologies/ml-agents.git
cd ml-agents
git checkout 0.5.0a
cd ml-agents
pip install .
```

Clone this repository.

```
git clone https://github.com/ostamand/rolling-balls.git
```

[Download](https://drive.google.com/open?id=1tGsYAxEHaB2hq1vUV3hin-xioQv5zyDD) the training environment and copy/paste the content of the extracted folder to `Training/data`

Train the agent using the included PPO algorithm (or use your own).

```
python train_rolling_ball.py
```