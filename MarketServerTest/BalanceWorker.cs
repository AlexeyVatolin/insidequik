using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest
{

    public static class BalanceWorker
    {
        private static List<DepoLimitEx> depoLimit = new List<DepoLimitEx>();
        private static List<MoneyLimitEx> moneyLimit = new List<MoneyLimitEx>();

        public static List<DepoLimitEx> GetDepoLimit()
        {
            depoLimit = QuikConnector.GetDepoLimits();
            return depoLimit;
        }

        public static List<MoneyLimitEx> GetMoneyLimit()
        {
            moneyLimit = QuikConnector.GetMoneyLimit();
            return moneyLimit;
        }
        /// <summary>
        /// получить текущий баланс в рублях (графа позиция)
        /// </summary>
        /// <returns></returns>
        public static long GetBalance()
        {
            moneyLimit = GetMoneyLimit();
            long balance = 0;
            foreach (var item in moneyLimit)
            {
                if (item != null && item.LimitKind == 2)
                {
                    if (item.CurrCode == "SUR")
                    {
                        balance += (long)item.CurrentBal;
                        QuikConnector.userAccount.currentBalance = balance;
                        break;
                    }
                }
            }
            return balance;

        }
        /// <summary>
        /// получить текущий баланс с учетом ликвидной стоимости активов
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentBalance()
        {
            long currentBalance = 0;
            depoLimit = GetDepoLimit();
            moneyLimit = GetMoneyLimit();

            foreach (var item in moneyLimit)
            {
                if (item != null && item.LimitKind == 2)
                {

                    if (item.CurrCode == "SUR")
                    {
                        currentBalance += (long)item.CurrentBal;
                        //QuikConnector.userAccount.currentBalance = balance;
                    }
                }
            }

            foreach (var item in depoLimit)
            {
                if (item != null && item.LimitKindInt == 2) //Заполняем только по T2
                {
                    DepoLimit depo = new DepoLimit(item);
                    currentBalance += (long)(depo.bePrice * depo.currBalance);
                }
            }
            return currentBalance;
        }
        /// <summary>
        /// получить текущую прибыль дня
        /// </summary>
        /// <returns></returns>
        public static long GetDayProfit()
        {
            long currentBalance = GetCurrentBalance();
            return (currentBalance - QuikConnector.userAccount.dayBalance);
        }


    }
}
