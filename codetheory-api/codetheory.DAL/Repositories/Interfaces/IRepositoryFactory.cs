namespace codetheory.DAL.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        T GetRepository<T>() where T : class;
    }
}
