using Npgsql;
using System.Security.Claims;

namespace animalHairdresser
{
    public interface IClientBaseService
    {
        public Task AddClientListAsync(string name, string phone);
        public Task<bool> ContainsClientAsync(string name);
        public int returnIdClient(string name);
        public Task<bool> ClientContainsAnimalsAsync(string name, Animal animal);
        public Task<Animal[]> SelectAnimalsFromClientAsync(string name);
        public Task DeleteAnimalAsync(string name, Animal animal);
        public Task AddAnimalToClientAsync(string name, Animal animal);
        public Task<bool> ChangePhoneAsync(string name, string phone);
        public Task CreateUserAsync(string name, string password);
        public Task UserExistsOrNotAsync(string name, string password);
    }
}
