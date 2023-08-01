import agent_pb2 as model
import agent_pb2_grpc as service
import grpc


class AgentClient:
    def __init__(self):
        channel = grpc.insecure_channel("localhost:50051")
        self.client = service.AgentServiceStub(channel)

    def step(self, action):
        response = self.client.Step(model.StepRequest(Action=action))
        return (response.State, response.Reward, response.Terminated)

    def reset(self):
        response = self.client.Reset(model.ResetRequest())
        return response.State
