
using Api.Agent;
using Game.Core;
using Grpc.Core;
using Google.Protobuf.Collections;
using Google.Protobuf;
using System;

namespace Game.API;


public class AgentServicer : Api.Agent.AgentService.AgentServiceBase
{
    private readonly GameManager gameManeger;

    public AgentServicer(GameManager gameManeger)
    {
        this.gameManeger = gameManeger;
    }


    public async override Task<StepResponse> Step(StepRequest request, ServerCallContext context)
    {

        var result = gameManeger.Action(request.Action);
        gameManeger.PrintMaze();
        var state = gameManeger.GetState();
        if (result)
        {
            if (gameManeger.IsGoal())
            {
                Console.WriteLine("-----GOAL-----");
                return await Task.FromResult(new StepResponse { State = { state }, Reward = 1.0f, Terminated = true });
            }
            else
            {
                return await Task.FromResult(new StepResponse { State = { state }, Reward = -0.1f, Terminated = false });
            }
        }
        else
        {
            return await Task.FromResult(new StepResponse { State = { state }, Reward = -1.0f, Terminated = false });
        }
    }

    public async override Task<Api.Agent.ResetResponse> Reset(Api.Agent.ResetRequest request, ServerCallContext context)
    {
        gameManeger.Reset();
        var state = gameManeger.GetState();
        return await Task.FromResult(new ResetResponse { State = { state } });
    }
}
