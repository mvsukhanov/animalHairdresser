using Npgsql;
using System.Security.Claims;

namespace animalHairdresser
{
    public interface IOrderBaseService
    {
        public Task<bool> AddOrderBaseAsync(Order order);
        public Task<List<TimeOnly>> SelectFreeTimeFromDateTimeAsync(DateOnly date);
        public Task<List<Order>> GetOrderListAsync(string name);
        public Task DeleteOrderAsync(DateTime dateTime, string name);
    }
}
