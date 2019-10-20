using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TaPegandoFogoBicho.Telegram
{
    class Program
    {
        static ITelegramBotClient botClient;

        static void Main(string[] args)
        {
            var telegramBotTokenApiToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_API_TOKEN_TA_PEGANDO_FOGO");
            botClient = new TelegramBotClient(telegramBotTokenApiToken);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {

            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:   $"Por favor, nos mande sua localização"
                );
            } else if (e.Message.Location != null) {
                Console.WriteLine($"Received a location message in chat {e.Message.Chat.Id}. Lat: {e.Message.Location.Latitude} Long: {e.Message.Location.Longitude}");

                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:   $"Obrigado vamos entrar em contato com as autoridades reponsavéis!"
                );
            }
        }
    }
}
