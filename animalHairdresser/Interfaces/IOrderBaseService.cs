using Npgsql;
using System.Security.Claims;

namespace animalHairdresser
{
    public interface IOrderBaseService
    {
        public Task<bool> AddOrderBaseAsync(string connString, Order order);
        public Task<List<TimeOnly>> SelectFreeTimeFromDateTimeAsync(DateOnly date, HttpContext context);
        public Task<List<Order>> GetOrderListAsync(HttpContext context);
        public Task DeleteOrderAsync(DateTime dateTime, HttpContext context);
    }
}
