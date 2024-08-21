namespace Domain.Interfaces.Repositories
{
    public interface IBaseSqlRepository<T> where T : class
    {
        Task<int> Save(T modelObject);
        Task<int> SaveMany(IEnumerable<T> modelList);
        Task<int> Update(T modelObject);
        Task<int> UpdateMany(IEnumerable<T> modelList);
        Task<IEnumerable<T>?> GetAll();
        Task<T?> GetById(int id);
        Task Delete(T modelObject);
        Task StartTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
