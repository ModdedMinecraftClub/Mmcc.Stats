using System.Threading.Tasks;
using Mmcc.Stats.Core.Models;

namespace Mmcc.Stats.Core.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GetToken(string value);
    }
}