using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Telegram.Bot;

namespace finam.ru_economic_calendar.Class
{
    public class MyTelegram
    {
        BackgroundWorker bw;
        TelegramBotClient Bot;
        public string helpStr = "/statisticGAZP - последняя статистика\n" +
                               "/statisticGAZP_average - cредняя цена за день";
        public string path = "TestTelegram.txt";
        public List<Item> lists = new List<Item>();

        public int apiId = 146653;
        public string apiHash = "5dd9a03936e1d3769c9d9c32733ec4d7";
        public string userID = string.Empty;

        public MyTelegram()
        {
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync("466671186:AAG8wa5HtjaTaHw9A4HknhHvYxPNsPRMjYo");
        }
        public void SendMessage(string message)
        {
            if (message != null || message != "")
            {
                if (userID != null)
                {
                    Bot.SendTextMessageAsync(userID, message);
                }
            }
        }
        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as string; // получаем ключ из аргументов
            try
            {
                Bot = new TelegramBotClient(key); // инициализируем API
                await Bot.SetWebhookAsync("");
                //Bot.SetWebhook(""); // Обязательно! убираем старую привязку к вебхуку для бота
                int offset = 0; // отступ по сообщениям
                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset); // получаем массив обновлений

                    foreach (var update in updates) // Перебираем все обновления
                    {
                        var message = update.Message;
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        {
                            if (message.Text == "/help")
                            {
                                // в ответ на команду /saysomething выводим сообщение
                                await Bot.SendTextMessageAsync(message.Chat, helpStr,
                                       replyToMessageId: message.MessageId);
                            }
                            if (message.Text == "/statisticGAZP")
                            {
                                await Bot.SendTextMessageAsync(message.Chat, statisticGAZP(),
                                       replyToMessageId: message.MessageId);
                            }
                            if (message.Text == "/statisticGAZP_average")
                            {
                                await Bot.SendTextMessageAsync(message.Chat, statisticGAZP_average(),
                                       replyToMessageId: message.MessageId);
                            }
                        }
                        offset = update.Id + 1;
                    }

                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                MessageBox.Show(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }

        }
        public string statisticGAZP()
        {
            Item last = lists[lists.Count - 1];
            return string.Format("LastOperation: {0}\nPrice: {1}\nVol: {2}\nOperation: {3}", last.Time, last.Last, last.Vol, last.Oper);
        }
        public string statisticGAZP_average()
        {
            double sum = 0;
            for (int i = 0; i < lists.Count; i++)
            {
                sum += Convert.ToDouble(lists[i].Last.Replace(".", ","));
            }
            return string.Format("Средняя цена за день: {0}", sum / lists.Count);
        }
    }
}
