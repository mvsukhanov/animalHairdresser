using Npgsql;
using Npgsql.NameTranslation;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace animalHairdresser
{
    public class ClientBaseService : IClientBaseService
    {
        
        public async Task AddClientListAsync(string name, string phone)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format(
                    "INSERT INTO client_base (phone) VALUES ('{1}') WHERE name = '{0}'", name, phone);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> ContainsClientAsync(string name)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format("SELECT * FROM client_base WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    if (reader.HasRows) return true;
                    return false;
                }
            }
        }

        public int returnIdClient(string name)
        {
            using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format("SELECT * FROM client_base WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }

        public async Task<bool> ClientContainsAnimalsAsync(string name, Animal animal)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT ARRAY[CAST(('{0}', '{1}', '{2}') as animal)] <@ 
                    (SELECT animals FROM client_base WHERE name = '{3}')",
                    animal.KindOfAnimal, animal.Breed, animal.AnimalName, name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.IsDBNull(0)) return false;
                    if (reader.GetBoolean(0) == true) return true;
                    return false;
                }
            }
        }

        public async Task<Animal[]> SelectAnimalsFromClientAsync(string name)
        {
            Animal[] animals = {};
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT animals FROM client_base WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0)) continue;
                        animals = reader.GetFieldValue<Animal[]>(0);
                    }
                }
            }
            return animals;
        }

        public async Task DeleteAnimalAsync(string name, Animal animal)
        {
            Animal[] animals = { };
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT animals FROM client_base WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0)) continue;
                        animals = reader.GetFieldValue<Animal[]>(0);
                    }
                }
            }

            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                string sqlCommand = string.Format(@"UPDATE client_base SET animals = null WHERE name = '{0}'", name);

                conn.Open();
                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }

            List<Animal> animalsList = animals.ToList();

            for (int i = animalsList.Count - 1; i >= 0; i--)
            {
                if (animalsList[i].KindOfAnimal == animal.KindOfAnimal && animalsList[i].Breed == animal.Breed
                    && animalsList[i].AnimalName == animal.AnimalName)
                    animalsList.Remove(animalsList[i]);
            }

            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                foreach (var item in animalsList)
                {
                    string sqlCommand = string.Format(@"UPDATE client_base SET animals = 
                    (SELECT animals FROM client_base WHERE name = '{0}') || 
                    CAST(('{1}','{2}','{3}') AS animal) WHERE name = '{0}'",
                    name, item.KindOfAnimal, item.Breed, item.AnimalName);

                    using (var command = new NpgsqlCommand(sqlCommand, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            //Если удаляем животное, то и удаляем все заказы в будующем с эти животным
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                string sqlCommand = string.Format(
                    @"delete from order_base where current_date < order_date and 
                    kind_of_animal = '{0}' and breed = '{1}' and animal_name='{2}'",
               animal.KindOfAnimal, animal.Breed, animal.AnimalName);

                conn.Open();
                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task AddAnimalToClientAsync(string name, Animal animal)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"UPDATE client_base SET animals = 
                    (SELECT animals FROM client_base WHERE name = '{0}') || 
                    CAST(('{1}','{2}','{3}') AS animal) WHERE name = '{0}'",
                    name, animal.KindOfAnimal, animal.Breed, animal.AnimalName);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> ChangePhoneAsync(string name, string phone)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {

                conn.Open();
                string sqlCommand = string.Format(@"update client_base set phone = '{0}' where name = '{1}'",
                    phone, name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
        public async Task CreateUserAsync(string name, string password)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format("INSERT INTO client_base (name, password) VALUES ('{0}', '{1}')", name, password);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public async Task UserExistsOrNotAsync(string name, string password)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT (password) FROM client_base WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!(reader.GetString(0) == password) || reader.GetString(0) is null) 
                            throw new Exception("Такого пользователя не существует");
                    }
                }
            }
        }
    }
}
