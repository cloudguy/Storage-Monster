﻿using System.Collections.Generic;
using StorageMonster.Domain;
using System;

namespace StorageMonster.Services
{
	public interface IUserService
	{
		User GetUserBySessionToken(Session session);
        User Load(int id);
		IEnumerable<UserRole> GetRolesForUser(string email);
		IEnumerable<UserRole> GetRolesForUser(int userId);
		IEnumerable<UserRole> GetRolesForUser(User user);
		bool IsUserInRole(string userName, string roleName);
		User GetUserByEmail(string email);
	    User Insert(User user);
	    void CreateRoleForUser(User user, string role);
        void UpdateUser(User user);
        ResetPasswordRequest GetActivePasswordResetRequestByToken(string token);
        ResetPasswordRequest CreatePasswordResetRequestForUser(User user, DateTime expiration);
        void DeleteResetPasswordRequest(int requestId);
	}
}
