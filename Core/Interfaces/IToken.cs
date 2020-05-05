using Core.Entities.Identity;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IToken
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}