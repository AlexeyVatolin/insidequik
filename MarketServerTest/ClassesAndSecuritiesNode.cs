using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    public class ClassesAndSecuritiesNode
    {
        public ClassInfo ClassInfo { get; set; }
        public ObservableCollection<SecurityInfo> SecurityInfos { get; set; }
    }
}
