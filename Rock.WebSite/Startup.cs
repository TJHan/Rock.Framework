using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rock.WebSite.Startup))]
namespace Rock.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
