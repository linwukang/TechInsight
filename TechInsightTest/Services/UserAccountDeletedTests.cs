using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TechInsight;
using TechInsight.Services;
using TechInsight.Services.Implementation;
using TechInsightDb.Data;

namespace TechInsightTest.Services
{
    [TestClass]
    public class UserAccountDeletedTests
    {
        private IRegisterAccountService _registerAccountService;
        private ILoginAccountService _loginAccountService;
        private ApplicationDbContext _context;

        [TestInitialize]
        public void Init()
        {
            var services = new ServiceCollection();
            DI.Configuration(services);
            var serviceProvider = services.BuildServiceProvider();

            _loginAccountService = serviceProvider.GetService<ILoginAccountService>();
            _registerAccountService = serviceProvider.GetService<IRegisterAccountService>();
            _context = serviceProvider.GetService<ApplicationDbContext>();
        }

        /// <summary>
        /// 测试注册账号
        /// </summary>
        public void TestRegisterAccount()
        {

            var zhangsan = _registerAccountService.RegisterAccount("张三", "12332143", "1233@qq.com");
            var lisi = _registerAccountService.RegisterAccount("李四", "545555", "dsad@qq.com");
            var wangwu = _registerAccountService.RegisterAccount("王五", "2423536", "123dsad3@qq.com");
            var zhaoliu = _registerAccountService.RegisterAccount("赵六", "2343242", "dsa@qq.com");

            Assert.IsNotNull(zhangsan);
            Assert.IsNotNull(lisi);
            Assert.IsNotNull(wangwu);
            Assert.IsNotNull(zhaoliu);
        }

        /// <summary>
        /// 测试删除账号
        /// </summary>
        public void TestDeleteAccount()
        {
            var zhangsan = _context
                .UserAccounts
                .Where(ua => ua.IsDeleted == null)
                .First(ua => "张三".Equals(ua.UserName));
            var lisi = _context
                .UserAccounts
                .Where(ua => ua.IsDeleted == null)
                .First(ua => "李四".Equals(ua.UserName));
            var wangwu = _context
                .UserAccounts
                .Where(ua => ua.IsDeleted == null)
                .First(ua => "王五".Equals(ua.UserName));
            var zhaoliu = _context
                .UserAccounts
                .Where(ua => ua.IsDeleted == null)
                .First(ua => "赵六".Equals(ua.UserName));

            _registerAccountService.DeleteAccount(zhangsan.Id, "zd", zhangsan.Id);
            _registerAccountService.DeleteAccount(lisi.Id, "ls", lisi.Id);
            _registerAccountService.DeleteAccount(wangwu.Id, "ww", wangwu.Id);
            _registerAccountService.DeleteAccount(zhaoliu.Id, "zl", zhaoliu.Id);
        }

        public void TestRealDeleteAccount()
        {
            var zhangsan = _context.UserAccounts.IgnoreQueryFilters().First(ua => "张三".Equals(ua.UserName));
            var lisi = _context.UserAccounts.IgnoreQueryFilters().First(ua => "李四".Equals(ua.UserName));
            var wangwu = _context.UserAccounts.IgnoreQueryFilters().First(ua => "王五".Equals(ua.UserName));
            var zhaoliu = _context.UserAccounts.IgnoreQueryFilters().First(ua => "赵六".Equals(ua.UserName));

            Assert.IsTrue(_registerAccountService.RealDeleteAccount(zhangsan.Id));
            Assert.IsTrue(_registerAccountService.RealDeleteAccount(lisi.Id));
            Assert.IsTrue(_registerAccountService.RealDeleteAccount(wangwu.Id));
            Assert.IsTrue(_registerAccountService.RealDeleteAccount(zhaoliu.Id));
        }

        [TestMethod]
        public void TestRegisterAndDeleteAccount()
        {
            Assert.IsFalse(_registerAccountService.ExistsUserName("张三"));
            Assert.IsFalse(_registerAccountService.ExistsUserName("李四"));
            Assert.IsFalse(_registerAccountService.ExistsUserName("王五"));
            Assert.IsFalse(_registerAccountService.ExistsUserName("赵六"));

            TestRegisterAccount();

            Assert.IsTrue(_registerAccountService.ExistsUserName("张三"));
            Assert.IsTrue(_registerAccountService.ExistsUserName("李四"));
            Assert.IsTrue(_registerAccountService.ExistsUserName("王五"));
            Assert.IsTrue(_registerAccountService.ExistsUserName("赵六"));

            TestDeleteAccount();

            Assert.IsFalse(_registerAccountService.ExistsUserName("张三"));
            Assert.IsFalse(_registerAccountService.ExistsUserName("李四"));
            Assert.IsFalse(_registerAccountService.ExistsUserName("王五"));
            Assert.IsFalse(_registerAccountService.ExistsUserName("赵六"));

            TestRealDeleteAccount();
        }

    }
}