namespace CloudBin.Data
{
    public interface IRepository<T, TId> where T : class
    {
        void Create(T entity);
        T Read(TId id);
        void Update(T entity);
        void Delete(T entity);
    }
}
