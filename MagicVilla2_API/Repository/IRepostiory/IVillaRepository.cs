using MagicVilla2_API.Models;
using System.Linq.Expressions;

namespace MagicVilla2_API.Repository.IRepostiory
{
    public interface IVillaRepository : IRepository<Villa>
    {
      
        Task<Villa> UpdateAsync(Villa entity);
  
    }
}
