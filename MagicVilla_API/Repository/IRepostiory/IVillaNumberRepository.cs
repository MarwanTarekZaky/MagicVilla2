using MagicVilla_API.Models;
using System.Linq.Expressions;

namespace MagicVilla_API.Repository.IRepostiory
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
      
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
  
    }
}
