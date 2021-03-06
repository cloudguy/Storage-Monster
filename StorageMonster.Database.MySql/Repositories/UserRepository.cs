﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.MySql.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        private const string TableName = "users";
        private const string SelectFieldList = "u.id AS Id, u.name AS Name, u.email AS Email, u.password AS Password, u.locale AS Locale, u.timezone AS TimeZone, u.stamp AS Stamp";
        private const string UpdateFieldList = "name=@Name, email=@Email, password=@Password, locale=@Locale, timezone=@TimeZone";
        private const string InsertFieldList = "(name, email, password, locale, timezone) VALUES (@Name, @Email, @Password, @Locale, @TimeZone)";

        public UserRepository(IConnectionProvider connectionprovider)
        {
            _connectionProvider = connectionprovider;
        }


        public User GetUserBySessionToken(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            return GetUserBySessionToken(session.Token);
        }

		public User Get(int id)
		{
            return SqlQueryExecutor.Execute(() =>
			{
				String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u " +
				"WHERE u.Id = @Id LIMIT 1", SelectFieldList, TableName);
				return _connectionProvider.CurrentConnection.Query<User>(query, new { Id = id }).FirstOrDefault();
			});
		}

        public User GetUserBySessionToken(string token)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u " +
                "INNER JOIN sessions s ON u.id=s.user_id "
                + "WHERE s.session_token=@SessionToken LIMIT 1", SelectFieldList, TableName);
                return _connectionProvider.CurrentConnection.Query<User>(query, new { SessionToken = token }).FirstOrDefault();
            });
        }

        public User GetUserByEmail(string email)
        {
            return SqlQueryExecutor.Execute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u WHERE u.email=@Email LIMIT 1", SelectFieldList, TableName);
                    return _connectionProvider.CurrentConnection.Query<User>(query, new { Email = email }).FirstOrDefault();
                });
        }

        public IEnumerable<User> List()
        {
            return SqlQueryExecutor.Execute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u ORDER BY Id",
                                                 SelectFieldList, TableName);
                    return _connectionProvider.CurrentConnection.Query<User>(query, null);
                });
        }

        public void DeleteAll()
        {
            SqlQueryExecutor.Execute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0}", TableName);
                    _connectionProvider.CurrentConnection.Execute(query, null);
                });
        }

        public UpdateResult Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");


            return SqlQueryExecutor.Execute(() =>
                {
                    return _connectionProvider.DoInTransaction(() =>
                    {
                        String checkStampQuery = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u " +
                        "WHERE u.Id = @Id AND u.stamp = @Stamp LIMIT 1", SelectFieldList, TableName);
                        User userCheck = _connectionProvider.CurrentConnection.Query<User>(checkStampQuery, new { user.Id, user.Stamp }).FirstOrDefault();

                        if (userCheck == null)
                        {
                            String checkUserQuery = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u " +
                            "WHERE u.Id = @Id LIMIT 1", SelectFieldList, TableName);
                            userCheck = _connectionProvider.CurrentConnection.Query<User>(checkUserQuery, new { user.Id }).FirstOrDefault();
                            return userCheck == null ? UpdateResult.ItemNotExists : UpdateResult.Stalled;
                        }

                        String query = string.Format(CultureInfo.InvariantCulture, "UPDATE {1} SET {0} WHERE Id=@Id", UpdateFieldList, TableName);
                        _connectionProvider.CurrentConnection.Execute(query, new { user.Email, user.Name, user.Password, user.Locale, user.TimeZone, user.Id });

                        String idAndStampQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id AS Id, stamp AS Stamp FROM {0} WHERE id=@Id;", TableName);
                        IdAndStamp idAndStamp = _connectionProvider.CurrentConnection.Query<IdAndStamp>(idAndStampQuery, new { user.Id }).FirstOrDefault();

                        if (idAndStamp == null)
                            throw new MonsterDbException("User update failed");

                        user.Id = idAndStamp.Id;
                        user.Stamp = idAndStamp.Stamp;                        
                        return UpdateResult.Success;
                    });
                });
        }

        public User Insert(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return SqlQueryExecutor.Execute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
                    int insertedId = (int)_connectionProvider.CurrentConnection.Query<long>(query, new { user.Name, user.Email, user.Password, user.Locale, user.TimeZone }).FirstOrDefault();
                    if (insertedId <= 0)
                        throw new MonsterDbException("User insertion failed");

                    String idAndStampQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id AS Id, stamp AS Stamp FROM {0} WHERE id=@Id;", TableName);
                    IdAndStamp idAndStamp = _connectionProvider.CurrentConnection.Query<IdAndStamp>(idAndStampQuery, new { Id = insertedId }).FirstOrDefault();

                    if (idAndStamp == null)
                        throw new MonsterDbException("User insertion failed");

                    user.Id = idAndStamp.Id;
                    user.Stamp = idAndStamp.Stamp;
                    return user;
                });
        }

        public User Load(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Load(user.Id);
        }

        public User Load(int id)
        {
            return SqlQueryExecutor.Execute(() =>
                {
                    String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} u WHERE u.Id=@Id ORDER BY Id", SelectFieldList, TableName);
                    return _connectionProvider.CurrentConnection.Query<User>(query, new { Id = id }).FirstOrDefault();
                });
        }
    }
}
