using QuikSharp.DataStructures;

namespace MarketServerTest
{
    class DepoLimit
    {
        public string balancePrice { get; set; }//Балансовая цена
        public string currentBalance { get; set; }//Позиция (количество лотов)
        public string limitKind { get; set; }//тип лимита T0, T1, T2
        public string secCode { get; set; }
        public string ClassCode { get; set; }
        public string Price { get; set; }//Цена
        public string bestPrice { get; set; } //Ликв. цена
        public string Cost { get; set; }//Стоимость
        public string bestCost { get; set; }//Ликв. стоимость
        public string nPL { get; set; }//Нереал. PL

        public string lockedBuy { get; set; }
        public string lockedSell { get; set; }
        public string lockedBuyValue { get; set; }
        public string lockedSellValue { get; set; }


        public string stopOrdersQty { get; set; }

        //
        public long currBalance;//Позиция
        public decimal price;//Цена
        public decimal bePrice;//Ликв. цена
        public double balPrice;//Балансовая цена
        //
        public DepoLimit(DepoLimitEx depoLimitEx)
        {
            secCode = depoLimitEx.SecCode;
            ClassCode = QuikConnector.GetSecurityClass(secCode);

            currBalance = depoLimitEx.CurrentBalance;
            price = QuikConnector.LastPrice(secCode);
            bePrice = QuikConnector.BestPrice(secCode);
            balPrice = depoLimitEx.AweragePositionPrice;

            balancePrice = depoLimitEx.AweragePositionPrice.ToString();
            currentBalance = currBalance.ToString();
            limitKind = depoLimitEx.LimitKind.ToString();
            lockedBuy = depoLimitEx.LockedBuy.ToString();
            lockedSell = depoLimitEx.LockedSell.ToString();
            lockedBuyValue = depoLimitEx.LockedBuyValue.ToString();
            lockedSellValue = depoLimitEx.LockedSellValue.ToString();
 
            Price = price.ToString();
            //SecurityInfo item = QuikConnector.GetSecurityInfo(secCode); //информация о бумаге
            Cost = (currBalance * price).ToString();
            bestPrice = bePrice.ToString();
            bestCost = (currBalance * bePrice).ToString();

            nPL = ((double)(currBalance * bePrice) - balPrice * currBalance).ToString();//nPL=Стоимость-Позиция*бал.цена

            stopOrdersQty = QuikConnector.GetStopOrdersQty(secCode).ToString();
        }
    }
}
