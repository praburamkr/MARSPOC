using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        DbSet<T> ModelSet { get; set; }

        Task<MarsResponse> CreateAsync(T item);

        Task<MarsResponse> GetAsync(int id);

        Task<MarsResponse> SearchAsync(T item);

        Task<MarsResponse> SearchAllAsync(IEnumerable<int> idList);

        Task<MarsResponse> DeleteAsync(int id);

        Task<MarsResponse> UpdateAsync(int id, T item);
    }
}
