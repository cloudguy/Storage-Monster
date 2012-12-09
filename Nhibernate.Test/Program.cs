using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StorageMonster.Database.Nhibernate.Mappings;
using StorageMonster.Domain;

namespace Nhibernate.Test
{
    public class custom_provider : NHibernate.Connection.DriverConnectionProvider
    {
        public override IDbConnection GetConnection()
        {
            var conn = base.GetConnection();
            using (IDbCommand command = conn.CreateCommand())
            {
                command.CommandText = "SET time_zone='+00:00'; SET names 'utf8';";
                command.ExecuteNonQuery();
            }

            return conn;
        }
    }

    class Program
    {
        static void Act(string s)
        {
            Console.WriteLine(s);
        }
        static void Main(string[] args)
        {
            NHibernate.Cfg.Configuration _config;
            ISessionFactory factory = CreateSessionFactory(out _config);
            using (ISession session = factory.OpenSession())
            {

                new SchemaExport(_config).Create(Act,false);//.Execute(true, true, false, session.Connection, Console.Out);

               // session.Connection..CreateSQLQuery("'SET time_zone='+00:00'; SET names 'utf8';'").ExecuteUpdate();
                //session.Save(p);
                //session.Flush();
            //     using (IDbCommand command = session.Connection.CreateCommand())
            //{
            //    command.CommandText = "SET time_zone='+00:00'; SET names 'utf8';";
            //    command.ExecuteNonQuery();
            //}
                using (var tran = session.BeginTransaction())
                {
                    ICriteria sc = session.CreateCriteria<User>();
                    var products = sc.List<User>();
                    Console.WriteLine(products.Count);
                    foreach (var product in products)
                    {
                        Console.WriteLine(product.Id + "---" + product.Name);
                    }

                    var pq = sc.List<User>();
                    foreach (var user in pq)
                    {
                        user.Name = user.Name+"1";
                        session.SaveOrUpdate(user);
                    }

                    ICriteria sc1 = session.CreateCriteria<StoragePlugin>();

                    //StoragePlugin s = new StoragePlugin();
                    //s.ClassPath = "clspth";
                    //s.Status = StoragePluginStatus.Loaded;
                    //session.SaveOrUpdate(s);
                    

                    var spl = sc1.List<StoragePlugin>();
                   //User u =new User();
                   // u.Email = "uuu";
                   // u.Name = "nm";
                   // u.Password = "pswd";
                   // u.TimeZone = 0;
                   // u.UserRole = UserRole.User;
                   // u.Locale = "en";
                   // session.SaveOrUpdate(u);

                    tran.Commit();
                }





                session.Close();
            }
            factory.Close();
        }

        private static ISessionFactory CreateSessionFactory(out NHibernate.Cfg.Configuration cfg)
        {
            cfg = new NHibernate.Cfg.Configuration();
           // cfg.Configure("hibernate.cfg.xml");
            return Fluently.Configure(cfg.Configure())
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();
        }
    }
}
