using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(University.Web.Startup))]
namespace University.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
