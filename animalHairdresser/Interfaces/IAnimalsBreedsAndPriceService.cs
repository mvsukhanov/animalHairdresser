using Npgsql;

namespace animalHairdresser
{
    public interface IAnimalsBreedsAndPriceService
    {
        public Task<int> GetPriceAsync(string kindOfAnimal, string breed, string connString);
        public Task<List<string>> KindOfAnimalsListAsync(string connString);
        public Task<List<string>> BreedFromKindOfAnimalsAsync(string kindOfAnimal, string connString);
    }
}
