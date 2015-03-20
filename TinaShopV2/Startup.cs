using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TinaShopV2.Startup))]
namespace TinaShopV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
