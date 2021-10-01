using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Praktyki2021.Startup))]
namespace Praktyki2021
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
