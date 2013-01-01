﻿using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class StoragePluginDescriptorsRepository : IStoragePluginDescriptorsRepository
    {
        public void SetStoragesStatuses(StoragePluginStatus status)
        {
            SessionManager.CurrentSession.CreateQuery("update StoragePluginDescriptor set Status = :status")
                .SetParameter("status", status)
                .ExecuteUpdate();
        }

        public StoragePluginDescriptor GetPluginByClassPath(string classPath)
        {
            return SessionManager.CurrentSession.QueryOver<StoragePluginDescriptor>()
                          .Where(s => s.ClassPath == classPath).SingleOrDefault();
        }

        public StoragePluginDescriptor Insert(StoragePluginDescriptor pluginDescriptor)
        {
            SessionManager.CurrentSession.Save(pluginDescriptor);
            SessionManager.CurrentSession.Flush();
            return pluginDescriptor;
        }
        public void Update(StoragePluginDescriptor pluginDescriptor)
        {
            SessionManager.CurrentSession.Update(pluginDescriptor);
            SessionManager.CurrentSession.Flush();
        }
        public void SaveOrUpdate(StoragePluginDescriptor pluginDescriptor)
        {
            SessionManager.CurrentSession.SaveOrUpdate(pluginDescriptor);
            SessionManager.CurrentSession.Flush();
        }
    }
}
