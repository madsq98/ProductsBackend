using System.Collections.Generic;
using ProductsBackend.Core.Models;

namespace ProductsBackend.Domain.IRepositories
{
    public interface IRepository<T>
    {
        public List<T> GetAll();

        public T Add(T obj);
        public T GetOne(int id);
        public T Update(int id, T obj);
    }
}