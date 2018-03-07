using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication0523mvc.Startup))]
namespace WebApplication0523mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
