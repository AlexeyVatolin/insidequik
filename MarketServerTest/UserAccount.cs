using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest
{
    class UserAccount
    {
        public long dayBalance { get; set; }//баланс на день (начальный баланс)
        public long currentBalance { get; set; } //текущий баланс
        public long userLimit { get; set; } //Лимит денег, которые можно потерять

        public bool limitLock { get; set; } //true, если достигнут предел убытков 

        public UserAccount()
        {
            this.dayBalance = 200000;
            this.userLimit = -1000;
        }
        public UserAccount(long dayBalance)
        {
            this.dayBalance = dayBalance;
        }

        

    }
}
