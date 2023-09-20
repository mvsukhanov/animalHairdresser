using Npgsql;
using System.Security.Claims;

namespace animalHairdresser
{
    public interface IClientBaseService
    {
        public Task AddClientListAsync(string name, string phone, string connString);
        public Task<bool> ContainsClientAsync(string name, string connString);
        public int returnIdClient(string name, string connString);
        public Task<bool> ClientContainsAnimalsAsync(string connString, string name, Animal animal);
        public Task<Animal[]> SelectAnimalsFromClientAsync(string connString, string name);
        public Task DeleteAnimalAsync(string connString, string name, Animal animal);
        public Task AddAnimalToClientAsync(string connString, string name, Animal animal);
        public Task<bool> ChangePhoneAsync(string connString, string name, string phone);
    }
}
