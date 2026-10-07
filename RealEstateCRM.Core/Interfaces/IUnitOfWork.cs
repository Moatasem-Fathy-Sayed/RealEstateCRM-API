using RealEstateCRM.Core.Entities;

namespace RealEstateCRM.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> CompleteAsync();
}