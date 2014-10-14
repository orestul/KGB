using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Photofolio.Startup))]
namespace Photofolio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
