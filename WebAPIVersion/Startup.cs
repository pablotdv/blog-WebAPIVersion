using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebAPIVersion.Startup))]

namespace WebAPIVersion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureSwagger(app);
        }
    }
}
