using Agl.Client.Properties;
using Agl.Core.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Agl.Client
{
    public class PeopleInfoConsoleWriter
    {
        private readonly IPeopleService _peopleService;
        private readonly ILogger<PeopleInfoConsoleWriter> _logger;

        public PeopleInfoConsoleWriter(IPeopleService peopleService,
            ILogger<PeopleInfoConsoleWriter> logger)
        {
            _peopleService = peopleService;
            _logger = logger;
        }

        public async Task Run()
        {
            var result = await _peopleService.FetchAllAsync();
            if (result == null || result.Count() == 0)
            {
                Console.WriteLine(Resource.NoResultFound);
                return;
            }

            var genderGroup = result.GroupBy(g => g.Gender).ToList();
            foreach (var item in genderGroup)
            {
                var gender = item.Key;
                var consoleTableHelper = new ConsoleTableHelper();
                consoleTableHelper.AddColumn(gender);
                var petCatCollection = item.Where(p => p.PetCollection != null)
                .SelectMany(p => p.PetCollection)
                .Where(p => p.Type.Equals("Cat", StringComparison.OrdinalIgnoreCase));

                foreach (var cat in petCatCollection.OrderBy(a => a.Name))
                    consoleTableHelper.AddRow(cat.Name);

                consoleTableHelper.Write();
            }
        }
    }
}
