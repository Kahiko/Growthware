using Microsoft.Owin;
using Owin;
using GrowthWare.WebApplication.App_Start;

[assembly: OwinStartupAttribute(typeof(GrowthWare.WebApplication.App_Start.Startup))]
namespace GrowthWare.WebApplication.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
        }
    }
}