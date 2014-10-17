using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GrowthWare.WebApplication.Startup))]
namespace GrowthWare.WebApplication
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            //ConfigureAuth(app);
        }
    }
}
