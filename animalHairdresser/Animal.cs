using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animalHairdresser
{
    public class Animal
    {
        [PgName("kind_of_animal")]
        public string KindOfAnimal { get; set; }

        [PgName("breed")]
        public string Breed { get; set; }

        [PgName("animal_name")]
        public string AnimalName { get; set; }

        public Animal() { }
        public Animal(string kindOfAnimal, string breed, string animalName)
        {
            KindOfAnimal = kindOfAnimal;
            Breed = breed;
            AnimalName = animalName;
        }
    }
}
