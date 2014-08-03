using NHibernate;

namespace CloudBin.Data.NHibernate
{
	public abstract class Repository<T, TId> : IRepository<T, TId> where T : class
	{
		protected ISession CurrentSession
		{
			get { return ((SessionManager) DatabaseSessionManager.Current).CurrentSession; }
		}

		void IRepository<T, TId>.Create(T entity)
		{
			CurrentSession.Save(entity);
		}

		T IRepository<T, TId>.Read(TId id)
		{
			return CurrentSession.Load<T>(id);
		}

		void IRepository<T, TId>.Update(T entity)
		{
			CurrentSession.Update(entity);
		}

		void IRepository<T, TId>.Delete(T entity)
		{
			CurrentSession.Delete(entity);
		}
	}
}
