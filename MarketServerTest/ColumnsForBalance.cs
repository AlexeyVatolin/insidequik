using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    class ColumnsForBalance
    {
        //балансовая цена
        public string balancePrice { get; set; }
        //Позиция (количество лотов)
        public string currentBalance { get; set; }
        //тип лимита T0, T1, T2
        public string limitKind { get; set; }
        public string secCode { get; set; }
        public string ClassCode { get; set; }
        public string Price { get; set; }//Цена
        public string bestPrice { get; set; } //Ликв. цена
        public string Cost { get; set; }//Стоимость
        public string bestCost { get; set; }//Ликв. стоимость
        public string nPL { get; set; }//Нереал. PL

        public string lockedBuy { get; set; }//количество
        public string lockedSell { get; set; }
        public string lockedBuyValue { get; set; }
        public string lockedSellValue { get; set; }


        public string stopOrdersQty { get; set; }





        public ColumnsForBalance(DepoLimitEx depoLimitEx)
        {
            
            balancePrice = depoLimitEx.AweragePositionPrice.ToString();
            currentBalance = depoLimitEx.CurrentBalance.ToString();
            limitKind = depoLimitEx.LimitKind.ToString();
            lockedBuy = depoLimitEx.LockedBuy.ToString();
            lockedSell = depoLimitEx.LockedSell.ToString();
            lockedBuyValue = depoLimitEx.LockedBuyValue.ToString();
            lockedSellValue = depoLimitEx.LockedSellValue.ToString();
            secCode = depoLimitEx.SecCode;

            ClassCode = QuikConnector.GetSecurityClass(secCode);
            Price = QuikConnector.LastPrice(secCode).ToString();
            //SecurityInfo item = QuikConnector.GetSecurityInfo(secCode);
            Cost = (depoLimitEx.CurrentBalance * QuikConnector.LastPrice(secCode)).ToString();
            bestPrice= QuikConnector.BestPrice(secCode).ToString();
            bestCost= (depoLimitEx.CurrentBalance * QuikConnector.BestPrice(secCode)).ToString();

            nPL = ((double)(depoLimitEx.CurrentBalance * QuikConnector.BestPrice(secCode))-
                depoLimitEx.AweragePositionPrice * depoLimitEx.CurrentBalance ).ToString();//nPL=Стоимость-Позиция*бал.цена

            stopOrdersQty = QuikConnector.GetStopOrdersQty(secCode).ToString();
        }

        public ColumnsForBalance(MoneyLimitEx moneyLimitEx)
        {
            
            secCode = moneyLimitEx.CurrCode.ToString();
            currentBalance = moneyLimitEx.CurrentBal.ToString();
            limitKind = moneyLimitEx.LimitKind.ToString();
            lockedBuy = moneyLimitEx.Locked.ToString();
            Price = QuikConnector.GetCrossRate(secCode).ToString();
            Cost = (moneyLimitEx.CurrentBal * (double)QuikConnector.GetCrossRate(secCode)).ToString(); //касты?
            bestCost = Cost;
            //SecurityInfo item = QuikConnector.GetSecurityInfo(secCode);
        }
    }
}
