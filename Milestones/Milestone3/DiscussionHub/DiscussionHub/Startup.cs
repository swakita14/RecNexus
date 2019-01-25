using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiscussionHub.Startup))]
namespace DiscussionHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
