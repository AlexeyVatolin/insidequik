using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IOrders
    {
        void OnOrders(Order order);
        void OnInitialize(List<Order> orders);
    }
}
