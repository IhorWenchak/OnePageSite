using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnePageSite.Startup))]
namespace OnePageSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
