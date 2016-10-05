using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Holmlia.TrainingWeb.Startup))]
namespace Holmlia.TrainingWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
