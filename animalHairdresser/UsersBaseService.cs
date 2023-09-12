using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace animalHairdresser
{
    public class UsersBaseService : IUserBaseService
    {
        public async Task UserExistsOrNotAsync(string connString)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
            }
        }

        public async Task CreateUserAsync(string name, string password)
        {
            await using (var conn = new NpgsqlConnection(Program.connString))
            {
                conn.Open();
                string sqlCommand = "CREATE ROLE \"" + name + "\" WITH LOGIN IN ROLE \"users\" PASSWORD \'" + password + "\'" +
                    ";  alter role \"" + name + "\" set search_path to administratorschema;";

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
