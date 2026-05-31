using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Generic;

Dictionary<long, bool> waitingGif = new Dictionary<long, bool>();

var bot = new TelegramBotClient("8636785011:AAGU7kyn1OmkDfk8_wwXXMRrREEwGdWC420");

bot.StartReceiving(
    updateHandler: async (bot, update, cancellationToken) =>
    {
        if (update.Message is not Message message)
            return;

        Console.WriteLine(message.Text);

        if (waitingGif.ContainsKey(message.Chat.Id))
        {
            waitingGif.Remove(message.Chat.Id);

            if (message.Text?.ToLower() == "хомяк")
            {
                await bot.SendAnimation(
                    message.Chat.Id,
                    InputFile.FromStream(
                        File.OpenRead(@"F:\Gifs\hamster.mp4")));

                return;
            }

            await bot.SendMessage(message.Chat.Id, "Такой гифки нет");
            return;
        }

        if (message.Text?.ToLower() == "скинь гифку")
        {
            waitingGif[message.Chat.Id] = true;

            await bot.SendMessage(message.Chat.Id, "Какую гифку?");

            return;
        }


        switch (message.Text)
        {
            case "/start":
                await bot.SendMessage(message.Chat.Id, "Привет");
                break;

            case "/price":
                await bot.SendMessage(message.Chat.Id, "Не знаю сколько стоит");
                break;


        }
    },

    errorHandler: async (bot, exception, cancellationToken) =>
    {
        Console.WriteLine(exception.Message);
    }
);

Console.WriteLine("Бот запущен");
Console.ReadLine();