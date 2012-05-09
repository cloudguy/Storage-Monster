using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageMonster.DB.Domain;
using StorageMonster.DB.Repositories;

namespace StorageMonster.DB.MySQL.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected IConnectionProvider Connectionprovider;

        protected const string TableName = "users";
        protected const string SelectFieldList = "u.id AS Id, u.name AS Name, u.email AS Email, u.password AS Password, u.locale AS Locale, u.timezone AS TimeZone, u.stamp AS Stamp";
        protected const string UpdateFieldList = "name=@Name, email=@Email, password=@Password, locale=@Locale, timezone=@TimeZone";
        protected const string InsertFieldList = "(name, email, password, locale, timezone) VALUES (@Name, @Email, @Password, @Locale, @TimeZone)";

        public UserRepository(IConnectionProvider connectionprovider)
        {
            Connectionprovider = connectionprovider;
        }


        public User GetUserBySessionToken(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            return GetUserBySessionToken(session.SessionToken);
        }

        public User GetUserBySessionToken(string token)
        {
            return SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u " +
                "INNER JOIN sessions s ON u.id=s.user_id "
                + "WHERE s.session_token=@SessionToken LIMIT 1", SelectFieldList, TableName);
                return Connectionprovider.CurrentConnection.Query<User>(query, new { SessionToken = token }).FirstOrDefault();
            });
        }

        public User GetByEmail(string email)
        {
            return SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u WHERE u.email=@Email LIMIT 1", SelectFieldList, TableName);
                    return Connectionprovider.CurrentConnection.Query<User>(query, new { Email = email }).FirstOrDefault();
                });
        }

        public IEnumerable<User> List()
        {
            return SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u ORDER BY Id",
                                                 SelectFieldList, TableName);
                    return Connectionprovider.CurrentConnection.Query<User>(query, null);
                });
        }

        public void DeleteAll()
        {
            SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0}", TableName);
                    Connectionprovider.CurrentConnection.Execute(query, null);
                });
        }

        public User Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");


            return SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "UPDATE {1} SET {0} WHERE Id=@Id", UpdateFieldList, TableName);
                    Connectionprovider.CurrentConnection.Execute(query, new { user.Email, user.Name, user.Password, user.Locale, user.TimeZone, user.Id });

                    String idAndStampQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id AS Id, stamp AS Stamp FROM {0} WHERE id=@Id;", TableName);
                    IdAndStamp idAndStamp = Connectionprovider.CurrentConnection.Query<IdAndStamp>(idAndStampQuery, new { user.Id }).FirstOrDefault();

                    if (idAndStamp == null)
                        throw new StorageMonsterDbException("User update failed");

                    user.Id = idAndStamp.Id;
                    user.Stamp = idAndStamp.Stamp;
                    return user;
                });
        }

        public User Insert(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
                    int insertedId = (int)Connectionprovider.CurrentConnection.Query<long>(query, new { user.Name, user.Email, user.Password, user.Locale, user.TimeZone }).FirstOrDefault();
                    if (insertedId <= 0)
                        throw new StorageMonsterDbException("User insertion failed");

                    String idAndStampQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id AS Id, stamp AS Stamp FROM {0} WHERE id=@Id;", TableName);
                    IdAndStamp idAndStamp = Connectionprovider.CurrentConnection.Query<IdAndStamp>(idAndStampQuery, new { Id = insertedId }).FirstOrDefault();

                    if (idAndStamp == null)
                        throw new StorageMonsterDbException("User insertion failed");

                    user.Id = idAndStamp.Id;
                    user.Stamp = idAndStamp.Stamp;
                    return user;
                });
        }

        public User Select(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Select(user.Id);
        }

        public User Select(int id)
        {
            return SqlQueryExecutor.Exceute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u WHERE u.Id=@Id ORDER BY Id", SelectFieldList, TableName);
                    return Connectionprovider.CurrentConnection.Query<User>(query, new { Id = id }).FirstOrDefault();
                });
        }

    }
}
