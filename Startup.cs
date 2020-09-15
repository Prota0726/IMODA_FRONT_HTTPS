using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IMODA_FRONT_HTTPS.Startup))]
namespace IMODA_FRONT_HTTPS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
