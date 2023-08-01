from agent_client import AgentClient
import torch as th
import numpy as np
import gymnasium as gym
from gymnasium import spaces


class MageEnv(gym.Env):
    def __init__(self, actions=4, render_mode="console"):
        super(MageEnv, self).__init__()

        self.agent = AgentClient()

        self.render_mode = render_mode
        self.action_space = spaces.Discrete(actions)
        self.observation_space = spaces.Box(
            low=-1.0, high=2.0, shape=(5, 5), dtype=np.float32
        )

    def reset(self, seed=None, options=None):
        super().reset(seed=seed, options=options)
        state = self.agent.reset()
        state = np.array(state).reshape(5, 5)
        return state, {}

    def step(self, action):
        state, reward, terminated = self.agent.step(action)
        truncated = False
        state = np.array(state).reshape(5, 5)
        return (state, reward, terminated, truncated, {})

    def render(self):
        if self.render_mode == "console":
            print("end", end="")

    def close(self):
        pass


class OnnxablePolicy(th.nn.Module):
    def __init__(self, extractor, action_net, value_net):
        super().__init__()
        self.extractor = extractor
        self.action_net = action_net
        self.value_net = value_net

    def forward(self, observation):
        action_hidden, value_hidden = self.extractor(observation)
        return self.action_net(action_hidden), self.value_net(value_hidden)


def onnx_export(model, path):
    onnxable_model = OnnxablePolicy(
        model.policy.mlp_extractor, model.policy.action_net, model.policy.value_net
    )

    observation_size = model.observation_space.shape
    dummy_input = th.randn(1, *observation_size)
    th.onnx.export(
        onnxable_model,
        dummy_input,
        path,
        opset_version=9,
        input_names=["input"],
    )
