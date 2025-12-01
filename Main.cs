using Discord;
using Discord.WebSocket;
//Utils
using BotDiscord.Messagem; // arquivo separado exclusivamente para receber as messagem
//types
using Bot.Type;
namespace Bot.Run
{
    class Global
    {
        public static Dictionary<ulong, UserInstrucaoAbaixa> UsersIntrucao = new Dictionary<ulong, UserInstrucaoAbaixa>();
        //Constante
        public const string VIDEO = "VIDEO";
        public const string AUDIO = "AUDIO";
    }

    public class BotRun
    {
        private DiscordSocketClient _client;

        public async Task Run()
        {
            //config
            var config = new DiscordSocketConfig
            {
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(config);

            _client.Log += msg =>
            {
                Console.WriteLine(msg);
                return Task.CompletedTask;
            };

            _client.MessageReceived += OnMessage;
            //Token
            string Token = "";

            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnMessage(SocketMessage msg)
        {
            Bot_GetMessagem Messagem = new Bot_GetMessagem(msg);
            await Messagem.Read();
        }
    }

}