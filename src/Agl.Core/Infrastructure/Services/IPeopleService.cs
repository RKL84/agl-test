using Agl.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agl.Core.Infrastructure.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<Person>> FetchAllAsync();
    }
}
