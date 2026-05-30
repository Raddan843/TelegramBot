

using Telegram.Bot.Types;
using Telegram.Bot;




var bot = new TelegramBotClient("8636785011:AAGU7kyn1OmkDfk8_wwXXMRrREEwGdWC420");


bot.StartReceiving(
    updateHandler: async (bot, update, cancellationToken) =>
    {
        if (update.Message is not Message message)

            return;

        Console.WriteLine(message.Text);

        await bot.SendMessage(
            chatId: message.Chat.Id,
            text:"Привет"
            
            );

    },

    errorHandler: async (bot, Exception, CancellationToken) =>
    {
        Console.WriteLine(Exception.Message);
    }




    );
Console.WriteLine("Бот запущен");
Console.ReadLine();