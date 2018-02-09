using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;

namespace Common.Interfaces
{
    public interface ITrades
    {
        void OnInitialize(List<Trade> trades);
        void OnTrades(Trade trade);
    }
}
