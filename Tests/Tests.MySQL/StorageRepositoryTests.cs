using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StorageMonster.Common;
using StorageMonster.DB.MySQL;
using StorageMonster.DB.MySQL.Configuration;
using StorageMonster.DB.MySQL.Repositories;
using StorageMonster.DB.Repositories;

namespace Tests.MySQL
{
	[TestFixture]
	public class StorageRepositoryTests
	{
		private ConnectionProvider _connProvider;
		private IStorageRepository _storageRepo;

		[SetUp]
		protected void SetUp()
		{
		    //Configuration.Init();
            DbConfiguration config = new DbConfiguration();
            _connProvider = new ConnectionProvider(config);
			_storageRepo = new StorageRepository(_connProvider);
		}

        [TearDown]
        protected void TearDown()
        {
            _connProvider.CloseCurentConnection();
        }

		[Test]
		public void BagMultiply()
		{
			//var items = _storageRepo.List();
            //items = _storageRepo.List();
		}
	}
}
