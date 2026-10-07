using System.Linq.Expressions;
using RealEstateCRM.Core.Entities;

namespace RealEstateCRM.Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> AsQueryable();
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}