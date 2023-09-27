using Npgsql;

namespace animalHairdresser
{
    public interface IAnimalsBreedsAndPriceService
    {
        public Task<int> GetPriceAsync(string kindOfAnimal, string breed);
        public Task<List<string>> KindOfAnimalsListAsync();
        public Task<List<string>> BreedFromKindOfAnimalsAsync(string kindOfAnimal);
    }
}
