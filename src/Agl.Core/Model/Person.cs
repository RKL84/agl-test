using Newtonsoft.Json;
using System.Collections.Generic;

namespace Agl.Core.Model
{
    public class Person
    {
        public Person()
        {
            PetCollection = new List<Pet>();
        }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        [JsonProperty("age")]
        public int Age { get; set; }

        /// <summary>
        /// Pet Collection
        /// </summary>
        [JsonProperty("pets")]
        public List<Pet> PetCollection { get; set; }
    }
}
