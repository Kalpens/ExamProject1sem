using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IServiceGateway<T>
    {
        Task<T> Get(int id);
        Task<IEnumerable<Customer>> Get(string name);
        Task<IEnumerable<Customer>> Get();
        Task<bool> Create(T newObject);
        Task<bool> Update(T updateObject);
        Task<bool> Delete(int id);
    }
}
