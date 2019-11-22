using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(rhHouseholdBudgeter.Startup))]
namespace rhHouseholdBudgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
