using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animalHairdresser
{
    public class Order
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string KindOfAnimal { get; set; }
        public string AnimalName { get; set; }
        public string Breed { get; set; }
        public int Price { get; set; }

        public Order(DateTime dateTime, string name, string phone, string kindOfAnimal, string animalName, string breed, int price)
        {
            Date = dateTime;
            Name = name;
            Phone = phone;
            KindOfAnimal = kindOfAnimal;
            AnimalName = animalName;
            Breed = breed;
            Price = price;
        }
    }
}
