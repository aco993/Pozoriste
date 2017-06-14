using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pozoriste.Startup))]
namespace Pozoriste
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
