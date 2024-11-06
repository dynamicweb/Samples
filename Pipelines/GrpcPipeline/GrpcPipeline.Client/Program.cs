using Grpc.Net.Client;
using GrpcPipeline;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:6001");
var client = new Greeter.GreeterClient(channel);
var request = new HelloRequest { Name = "gRPC client" };

var reply = await client.SayHelloAsync(request);

Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();