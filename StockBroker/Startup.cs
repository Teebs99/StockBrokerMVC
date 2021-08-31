using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StockBroker.Startup))]
namespace StockBroker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
