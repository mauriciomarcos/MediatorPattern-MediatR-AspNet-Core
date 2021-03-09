using MediatRSample.Models;
using MediatRSample.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatRSample.Repositories
{
    public class PessoaRepository : IRepository<Pessoa>
    {
        private static Dictionary<int, Pessoa> pessoas = new Dictionary<int, Pessoa>(); 

        public async Task Add(Pessoa e)
        {
            await Task.Run(() => 
            {
                pessoas.Add(e.Id, e);
            });
        }

        public async Task Delete(int id)
        {
            await Task.Run(() =>
            {
                pessoas.Remove(id);
            });
        }

        public async Task Edit(Pessoa e)
        {
            await Task.Run(() =>
            {
                pessoas.Remove(e.Id);
                pessoas.Add(e.Id, e);
            });
        }

        public async Task<Pessoa> Get(int id)
        {
            return await Task.Run(() => pessoas.GetValueOrDefault(id));
        }

        public async Task<IEnumerable<Pessoa>> GetAll()
        {
            return await Task.Run(() => pessoas.Values.ToList());
        }
    }
}