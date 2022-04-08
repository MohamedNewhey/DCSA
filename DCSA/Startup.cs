using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DCSA.Startup))]
namespace DCSA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
