using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animalHairdresser
{
    public class Animals
    {
        [PgName("kindofanimal")]
        public string KindOfAnimal { get; set; }

        [PgName("breed")]
        public string Breed { get; set; }

        [PgName("animalname")]
        public string AnimalName { get; set; }

        public Animals() { }
        public Animals(string kindOfAnimal, string breed, string animalName)
        {
            KindOfAnimal = kindOfAnimal;
            Breed = breed;
            AnimalName = animalName;
        }
    }
}
