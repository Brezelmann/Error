using System;
using System.Linq;
using System.Threading.Tasks;
using ErrorTest;
using ErrorTest.Domain.Entities;
using ErrorTest.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Test
{
    [TestFixture]
    public class UnitTest1
    {
        private DbContextOptions<ApplicationDbContext> _options;

        private Guid _firstGuid;

        [SetUp]
        public void Init()
        {
            _firstGuid = Guid.NewGuid();
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(_firstGuid.ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using (var context = new ApplicationDbContext(_options))
            {
                context.Users.Add(new User()
                {
                    Id = _firstGuid,
                    Name = (FullName) "Max_Mustermann"
                });

                for (int i = 0; i < 5; i++)
                {
                    context.Claims.Add(new UserClaims()
                    {
                        Id = Guid.NewGuid(),
                        Type = "Type " + i,
                        Value = "Value " + i,
                        UserId = _firstGuid
                    });
                }

                context.SaveChanges();
            }
        }

        [Test]
        public async Task TestEagerLoadingFails()
        {
            using (var con = new ApplicationDbContext(_options))
            {
                var users = await con.Claims.Where(x => x.Type.Equals("Type 0") && x.Value.Equals("Value 0"))
                    .Include(y => y.User).Select(z => z.User).ToListAsync();

                Assert.That(users.ElementAtOrDefault(0).Name, Is.Not.Null);
            }
        }

        [Test]
        public async Task TestEagerLoadingWorks()
        {
            using (var con = new ApplicationDbContext(_options))
            {
                var users = await con.Claims.Where(x => x.Type.Equals("Type 0") && x.Value.Equals("Value 0"))
                    .Include(y => y.User).Select(z => z.User).Include(a => a.Name).ToListAsync();

                Assert.That(users.ElementAtOrDefault(0).Name, Is.Not.Null);
            }
        }
    }
}
