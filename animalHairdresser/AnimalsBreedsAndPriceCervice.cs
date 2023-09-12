using Npgsql;

namespace animalHairdresser
{
    public class AnimalsBreedsAndPriceCervice : IAnimalsBreedsAndPriceService
    {
        public async Task<int> GetPriceAsync(string kindOfAnimal, string breed, string connString)
        {
            int cooficient = 0;
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format(
                    "SELECT * FROM animals_breed_and_price WHERE breed = '{0}' and kind_of_animal = '{1}'", breed, kindOfAnimal);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    cooficient = reader.GetInt32(3);
                }
            }
            int answer = 0;
            if (kindOfAnimal == "пес")
                answer = 150 + 70 * cooficient;
            if (kindOfAnimal == "кот")
                answer = 100 + 50 * cooficient;
            return answer;
        }

        public async Task<List<string>> KindOfAnimalsListAsync(string connString)
        {
            List<string> kindOfAnimals = new List<string>();

            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = "select distinct kind_of_animal from animals_breed_and_price";

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        kindOfAnimals.Add(reader.GetString(0));
                    }
                }
            }
            return kindOfAnimals;
        }

        public async Task<List<string>> BreedFromKindOfAnimalsAsync(string kindOfAnimal, string connString)
        {
            List<string> kindOfAnimals = new List<string>();

            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"select distinct breed from animals_breed_and_price where kind_of_animal = '{0}'", kindOfAnimal);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        kindOfAnimals.Add(reader.GetString(0));
                    }
                }
            }
            return kindOfAnimals;
        }
    }
}
