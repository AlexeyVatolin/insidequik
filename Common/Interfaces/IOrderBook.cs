using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface IOrderBook
    {
        void OnQuote(List<Models.OrderBook> orderBooks);
    }
}
