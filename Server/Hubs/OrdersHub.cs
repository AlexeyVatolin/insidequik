using Common.Interfaces;
using Inside.Core.Base.DatabaseContext.Entities;
using Microsoft.AspNet.SignalR;
using QuikSharp.DataStructures.Transaction;
using Server.Quik;
using ServerCore.Base.DatabaseContext;
using ServerCore.Hubs;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class OrdersHub: BaseHub<ClientBase, IHubClientCallbacks>
    {
        public void SendOrder(Common.Models.ClientOrder order)
        {
            string userID = CurrentUser.Id;

            long transId = Quik.QuikData.NewOrder(order, userID);
        }
        public void SendStopOrder(Common.Models.ClientStopOrder stopOrder)
        {
            string userID = CurrentUser.Id;

            long transId = Quik.QuikData.NewStopOrder(stopOrder, userID);
        }
        public void SubscribeToOrdersRefresh()
        {

            string userID = CurrentUser.Id;
            try
            {
                QuikData.SubscribeToOrdersRefresh(NewOrder);
            }
            catch { }
        }
        public void CancelOrder(Order order) //TODO:Создать свою модель
        {
           QuikConnector.Quik.Orders.KillOrder(order);
        }
        public List<Order> InitializeOrders()
        {
            string userID = CurrentUser.Id;
            List<Order> userOrders = QuikConnector.Quik.Orders.GetOrders().Result.Where(x => x.UserId == userID).ToList();
            return userOrders;
        }
        private void NewOrder(Order order)
        {//TODO: распределение заявок по id 
            //id будет записан в TransId
            using (Context db = new Context())
            {
                OrderHistory orderHistory = new OrderHistory
                {
                    OrderId = order.OrderNum,
                    UserId = order.UserId,
                    User = db.Users.Find(order.UserId)
                };
                db.OrderHistories.Add(orderHistory);
            }
            
        }
    }
}
