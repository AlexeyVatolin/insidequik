using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Base.Interfaces
{
    public interface IDispose
    {
        bool IsDisposed { get; set; }
    }
}
