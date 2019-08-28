using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PostgreSQLTest.Startup))]
namespace PostgreSQLTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
