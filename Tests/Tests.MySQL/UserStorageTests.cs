using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StorageMonster.DB.Domain;
using StorageMonster.DB.MySQL;
using StorageMonster.DB.MySQL.Configuration;
using StorageMonster.DB.MySQL.Repositories;
using StorageMonster.DB.Repositories;

namespace Tests.MySQL
{
    [TestFixture]
    public class UserStorageTests
    {
        private ConnectionProvider _connProvider;
        private IUserRepository _userRepo;

        [SetUp]
        protected void SetUp()
        {
            DbConfiguration config = new DbConfiguration();
            _connProvider = new ConnectionProvider(config);
            _userRepo = new UserRepository(_connProvider);
        }

        [TearDown]
        protected void TearDown()
        {
            _connProvider.CloseCurentConnection();
        }

        [Test]
        public void TestDeleteAndListAndInsert()
        {
            _userRepo.DeleteAll();

            var items = _userRepo.List();
            Assert.True(items.Count() == 0);

            List<User> userCollection = new List<User>();

            int userCounter = 20;

            for (int i = 0; i < userCounter; i++)
            {
                User user = new User
                    {
                        Locale = "en",
                        Name = i.ToString(),
                        Password = "pass",
                        TimeZone = 1000
                    };

                _userRepo.Insert(user);
                Assert.GreaterOrEqual(user.Id, 1);
                Assert.GreaterOrEqual(user.Stamp, DateTime.UtcNow.AddMinutes(-1));
                userCollection.Add(user);
            }

            _userRepo.List();

        }
    }
}