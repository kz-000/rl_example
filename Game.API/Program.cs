using Api.Agent;
using Game.API;
using Grpc.Core;

const int Port = 50051;

Server server = new Server
{
    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
};
server.Services.Add(AgentService.BindService(new AgentServicer(new())));
server.Start();
Console.WriteLine("start");
Console.ReadKey();
await server.ShutdownAsync();
