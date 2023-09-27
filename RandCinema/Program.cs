using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;

namespace RandCinema
{
    class Program
    {
        /// <summary>
        /// Клиент бота.
        /// </summary>
        static public ITelegramBotClient botClient;

        public static void Main(string[] args)
        {
            botClient = new TelegramBotClient("6673192230:AAHYVRmnYBWd32jiXo7jtJ2pB5OZEtVAcEg");
            var me =botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
            botClient.StartReceiving(Update, Error);

            Console.ReadLine();
        }

        /// <summary>
        /// Обновление сообщений.
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="update">Входящее обновление</param>
        /// <param name="token"></param>
        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                switch (message.Text)
                {
                    case string command when command == "/hello":
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Hello!");
                        break;
                    case string command when command == "/rand":
                        await SendRandomCinemaFromFile(update);
                        break;
                }
            }
        }

        async static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
/// <summary>
/// Отправить случайное название фильма.
/// </summary>
/// <param name="update"></param>
        async static Task SendRandomCinemaFromFile(Update update)
        {
            var messageUpd = update.Message;
            
            string fileName = System.IO.File.ReadAllText("C:/Users/User/Desktop/RandomCinemaBot/CinemaNames.json");
            List<CinemaClass> cinemaNamesList = JsonConvert.DeserializeObject<List<CinemaClass>>(fileName);
            
            Random rnd = new Random();
            var CinemaNumber = rnd.Next(cinemaNamesList.Count-1);
            string message = cinemaNamesList[CinemaNumber].Name;
            Console.WriteLine(message);
            await botClient.SendTextMessageAsync(chatId: messageUpd.Chat.Id, text: message);
        }

/// <summary>
/// Класс фильмов.
/// </summary>
        public class CinemaClass
        {
            public string Name { get; set; }
        }
    }
}