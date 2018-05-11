using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IServiceGateway<T>
    {
        T Get(int id);
        List<T> Get();
        bool Create(T newObject);
        T Update(T updateObject);
        bool Delete(int id);
    }
}
