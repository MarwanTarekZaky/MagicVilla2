using MagicVilla2_API.Models;
using System.Linq.Expressions;

namespace MagicVilla2_API.Repository.IRepostiory
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
      
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
  
    }
}
