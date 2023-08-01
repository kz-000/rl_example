from src.mage_env import TicTacEnv, onnx_export
from stable_baselines3 import PPO


env = TicTacEnv()

model = PPO("MlpPolicy", env, verbose=1, tensorboard_log="./logs/")
model.learn(1000000)
onnx_export(model, "test.onnx")
