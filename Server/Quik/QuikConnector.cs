using System;
using System.Threading;
using System.Threading.Tasks;
using QuikSharp;

namespace Server
{
    static class QuikConnector
    {
        public static QuikSharp.Quik Quik { get; private set; }
        public static bool isConnected { get; private set; }

        public static async Task<bool> Connect()
        {

            Quik = new QuikSharp.Quik(QuikSharp.Quik.DefaultPort, new InMemoryStorage());
            var connectionCheckTask = Quik.Service.IsConnected();
            int timeout = 2000; //2 раз проверяем подключение и ждем ответа 2 с. Если не подключилось, от возвращаем false

            Thread.Sleep(100); //Ожидаем полной инициализации Quik.
            for (int i = 0; i < 2; i++)
            {
                if (await Task.WhenAny(connectionCheckTask, Task.Delay(timeout)) == connectionCheckTask)
                {
                    isConnected = true;
                    /*userAccount = new UserAccount();
                    userAccount.currentBalance = BalanceWorker.GetCurrentBalance();*/
                    return true;
                }
                try
                {
                    Quik.StopService();
                }
                catch (Exception)
                {
                    // ignored
                }
                Quik = null;
                Quik = new QuikSharp.Quik(QuikSharp.Quik.DefaultPort, new InMemoryStorage());
            }

            return false;
        }

        public static void Disconnect()
        {
            Quik.StopService();
        }
    }
}
