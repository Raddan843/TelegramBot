

using Telegram.Bot.Types;
using Telegram.Bot;




var bot = new TelegramBotClient("8636785011:AAGU7kyn1OmkDfk8_wwXXMRrREEwGdWC420");


bot.StartReceiving(
    updateHandler: async (bot, update, cancellationToken) =>
    {
        if (update.Message is not Message message)

            return;

        Console.WriteLine(message.Text);

        if(message.Text == "/start") {

            await bot.SendMessage(message.Chat.Id, "Привет");
}           
        else if(message.Text == "/price")
        {
            await bot.SendMessage(message.Chat.Id, "не знаю такой цены");
        }


    },

    errorHandler: async (bot, Exception, CancellationToken) =>
    {
        Console.WriteLine(Exception.Message);
    
    
    }

    






    );
Console.WriteLine("Бот запущен");
Console.ReadLine();