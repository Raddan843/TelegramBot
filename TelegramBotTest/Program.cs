using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections.Generic;
using Npgsql;



string connectionString = "Host=localhost;Port=4321;Username=postgres;Password=Karabas2002;Database=testdb";

Dictionary<long, bool> waitingGif = new Dictionary<long, bool>();


await using var connection = new NpgsqlConnection(connectionString);
await connection.OpenAsync();

Console.WriteLine("База запущена");



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

            var cmd = new NpgsqlCommand("SELECT path FROM gifs WHERE LOWER(name) = LOWER(@name)", connection);

            cmd.Parameters.AddWithValue("name", message.Text  ?? "");

            Console.WriteLine($"Ищу гифку: {message.Text}");


            var result = await cmd.ExecuteScalarAsync();

            Console.WriteLine(result);

            if(result != null)
            {

                var path = result.ToString();


                if (!File.Exists(path))
                {
                    await bot.SendMessage(message.Chat.Id, "Файл гифки не найден");
                    return;
                }

                await bot.SendAnimation(
                    message.Chat.Id,
                    InputFile.FromStream(File.OpenRead(path)));

                var updateCmd = new NpgsqlCommand(
                    "UPDATE gifs SET uses = uses + 1 WHERE LOWER(name) = LOWER(@name)",
                    connection);

                updateCmd.Parameters.AddWithValue("name", message.Text ?? "");

                await updateCmd.ExecuteNonQueryAsync();

                return;

            }

            await bot.SendMessage(message.Chat.Id, "Гифки с таким названием нет");

            return;

        }




        if (message.Text?.ToLower().Contains("гифк") == true)
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


            case "/gifs":
                {
                    await bot.SendMessage(message.Chat.Id, "Пока не завезли");
                    break;
                    
                }

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