using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bgrimmett_portfolio2.Startup))]
namespace bgrimmett_portfolio2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
