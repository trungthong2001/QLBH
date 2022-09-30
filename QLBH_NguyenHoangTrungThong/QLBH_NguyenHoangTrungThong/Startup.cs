using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLBH_NguyenHoangTrungThong.Startup))]
namespace QLBH_NguyenHoangTrungThong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
