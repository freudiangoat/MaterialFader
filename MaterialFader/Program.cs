using MaterialFader.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaterialFader
{
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            var svcProvider = RegisterServices();
            var stateManager = svcProvider.GetService<IStateManager>();
            var mainHandler = svcProvider.GetService<IMainMessageHandler>();
            var socketManager = svcProvider.GetService<ISessionHandler>();

            socketManager.RegisterStateManager(stateManager);

            await stateManager.ChangeState("disconnected");

            var host = WebSocketHostBuilder.Create()
                .UseWebSocketMessageHandler(mainHandler.HandleIncoming)
                .UseSessionHandler(socketManager.OnConnected, socketManager.OnDisconnected)
                .ConfigureAppConfiguration((_, conf) =>
                {
                    conf.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["serverOptions:name"] = "MaterialFader",
                        ["serverOptions:listeners:0:ip"] = "Any",
                        ["serverOptions:listeners:0:port"] = "2021"
                    });
                })
                .Build();

            host.Run();
        }

        private static IServiceProvider RegisterServices()
        {
            var builder = new ServiceCollection();
            builder.AddSingleton<FaderPort>()
                .AddSingleton<IStateManager, StateManager>()
                .AddSingleton<IMainMessageHandler, MessageHandler>()
                .AddSingleton<WebSocketManager>()
                .AddSingleton<IMessageFactory, MessageFactory>()
                .AddAllImplementations<IMessageParser>(typeof(Program).Assembly)
                .AddSingleton<ISessionHandler>(sp => sp.GetRequiredService<WebSocketManager>())
                .AddSingleton<IWebSocketSessionManager>(sp => sp.GetRequiredService<WebSocketManager>())
                .AddAllImplementations<IStateMessageHandler>(typeof(Program).Assembly);

            return builder.BuildServiceProvider();
        }
    }
}
