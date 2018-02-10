using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ServerCore.Base.DatabaseContext;
using ServerCore.Base.DatabaseContext.Entities;

namespace ServerCore.Base.IdentityConfig
{
    public class InsideUserStore : UserStore<User>
    {
        public InsideUserStore(IOwinContext context) : base(context.Get<Context>())
        { }

        public new Context Context => (Context)base.Context;
    }
}
