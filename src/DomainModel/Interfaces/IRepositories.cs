using DomainModel.Specifications.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> FindAsync(IQuerySpecification<T> spec);
        Task<int> CountAsync(IQuerySpecification<T> spec);

        Task SaveChangeAsync();
    }
}
