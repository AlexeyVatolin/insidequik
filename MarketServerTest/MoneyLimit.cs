using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest
{
    class MoneyLimit
    {
 
        //Позиция (количество лотов)
        public string currentBalance { get; set; }
        public string limitKind { get; set; }//тип лимита T0, T1, T2
        public string secCode { get; set; }
        public string Price { get; set; }//Цена
        public string Cost { get; set; }//Стоимость
        public string bestCost { get; set; }//Ликв. стоимость
        public string lockedBuy { get; set; }

        public MoneyLimit(MoneyLimitEx moneyLimitEx)
        {

            secCode = moneyLimitEx.CurrCode.ToString();
            var currBalance = moneyLimitEx.CurrentBal;
            currentBalance = moneyLimitEx.CurrentBal.ToString();
            limitKind = moneyLimitEx.LimitKind.ToString();
            lockedBuy = moneyLimitEx.Locked.ToString();
            Price = QuikConnector.GetCrossRate(secCode).ToString();
            Cost = (currBalance * (double)QuikConnector.GetCrossRate(secCode)).ToString(); //касты?
            bestCost = Cost;
            //SecurityInfo item = QuikConnector.GetSecurityInfo(secCode);
        }
    }
}
