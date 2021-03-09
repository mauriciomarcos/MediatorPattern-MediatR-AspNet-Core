using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediatRSample.Models.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task Add(T e);

        Task Edit(T e);

        Task Delete(int id);
    }
}