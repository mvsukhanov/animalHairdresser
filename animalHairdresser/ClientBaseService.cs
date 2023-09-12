using Npgsql;
using Npgsql.NameTranslation;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace animalHairdresser
{
    public class ClientBaseService : IClientBaseService
    {
        
        public async Task AddClientListAsync(string name, string phone, string connString)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format("INSERT INTO clientbase (name, phone) VALUES ('{0}', '{1}')", name, phone);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> ContainsClientAsync(string name, string connString)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format("SELECT * FROM clientbase WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    if (reader.HasRows) return true;
                    return false;
                }
            }
        }

        public int returnIdClient(string name, string connString)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format("SELECT * FROM clientbase WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }

        public async Task<bool> ClientContainsAnimalsAsync(string connString, string name, Animals animal)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT ARRAY[CAST(('{0}', '{1}', '{2}') as animals)] <@ 
                    (SELECT animals FROM clientbase WHERE name = '{3}')",
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

        public async Task<Animals[]> SelectAnimalsFromClientAsync(string connString, string name)
        {
            Animals[] animals = {};
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT animals FROM clientbase WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0)) continue;
                        animals = reader.GetFieldValue<Animals[]>(0);
                    }
                }
            }
            return animals;
        }

        public async Task DeleteAnimalAsync(string connString, string name, Animals animal)
        {
            Animals[] animals = { };
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"SELECT animals FROM clientbase WHERE name = '{0}'", name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0)) continue;
                        animals = reader.GetFieldValue<Animals[]>(0);
                    }
                }
            }

            await using (var conn = new NpgsqlConnection(connString))
            {
                string sqlCommand = string.Format(@"UPDATE clientbase SET animals = null WHERE name = '{0}'", name);

                conn.Open();
                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }

            List<Animals> animalsList = animals.ToList();

            for (int i = animalsList.Count - 1; i >= 0; i--)
            {
                if (animalsList[i].KindOfAnimal == animal.KindOfAnimal && animalsList[i].Breed == animal.Breed
                    && animalsList[i].AnimalName == animal.AnimalName)
                    animalsList.Remove(animalsList[i]);
            }

            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                foreach (var item in animalsList)
                {
                    string sqlCommand = string.Format(@"UPDATE clientbase SET animals = 
                    (SELECT animals FROM clientbase WHERE name = '{0}') || 
                    CAST(('{1}','{2}','{3}') AS animals) WHERE name = '{0}'",
                    name, item.KindOfAnimal, item.Breed, item.AnimalName);

                    using (var command = new NpgsqlCommand(sqlCommand, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            //Если удаляем животное, то и удаляем все заказы в будующем с эти животным
            await using (var conn = new NpgsqlConnection(connString))
            {
                string sqlCommand = string.Format(
                    @"delete from orderlist where current_date < orderdate and 
                    kindofanimal = '{0}' and breed = '{1}' and animalname='{2}'",
               animal.KindOfAnimal, animal.Breed, animal.AnimalName);

                conn.Open();
                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task AddAnimalToClientAsync(string connString, string name, Animals animal)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format(@"UPDATE clientbase SET animals = 
                    (SELECT animals FROM clientbase WHERE name = '{0}') || 
                    CAST(('{1}','{2}','{3}') AS animals) WHERE name = '{0}'",
                    name, animal.KindOfAnimal, animal.Breed, animal.AnimalName);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> ChangePhoneAsync(string connString, string name, string phone)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();
                string sqlCommand = string.Format(@"update clientbase set phone = '{0}' where name = '{1}'",
                    phone, name);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
