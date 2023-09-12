using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace animalHairdresser
{
    public interface IUserBaseService
    {
        public Task UserExistsOrNotAsync(string connString);
        public Task CreateUserAsync(string name, string password);
    }
}
