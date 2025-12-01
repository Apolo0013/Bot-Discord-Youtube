using Bot.Run;
using Bot.Resquest;

class Program
{
    public static async Task Main(string[] args)
    {
        BotRun bot = new BotRun();
        await bot.Run();
    }
}