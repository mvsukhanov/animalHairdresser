using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace animalHairdresser
{
    public class OrderBaseService : IOrderBaseService
    {
        public List<TimeOnly> AllMabyTimes = new List<TimeOnly>
        {
            new TimeOnly(8, 00),
            new TimeOnly(8, 30),
            new TimeOnly(9, 00),
            new TimeOnly(9, 30),
            new TimeOnly(10, 00),
            new TimeOnly(10, 30),
            new TimeOnly(11, 00),
            new TimeOnly(11, 30),
            new TimeOnly(13, 00),
            new TimeOnly(13, 30),
            new TimeOnly(14, 00),
            new TimeOnly(14, 30),
            new TimeOnly(15, 00),
            new TimeOnly(15, 30),
            new TimeOnly(16, 00),
            new TimeOnly(16, 30)
        };

        public async Task<bool> AddOrderBaseAsync(string connString, Order order)
        {
            await using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sqlCommand = string.Format("INSERT INTO order_base (order_date, name, phone, kind_of_animal, animal_name, breed, price) " +
                    "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}', {6})",
                    order.Date.ToString(), 
                    order.Name, 
                    order.Phone, 
                    order.KindOfAnimal, 
                    order.AnimalName, 
                    order.Breed,
                    order.Price);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

        public async Task<List<TimeOnly>> SelectFreeTimeFromDateTimeAsync(DateOnly date, HttpContext context)
        {
            List<TimeOnly> mabyTimes = AllMabyTimes;

            await using (var conn = new NpgsqlConnection(context.User.FindFirst("connString").Value))
            {
                conn.Open();
                string sqlCommand = string.Format(
                    @"select order_date from order_base where order_date > '{0}' and order_date < '{0}'::Date + 1", date);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TimeOnly time = TimeOnly.FromDateTime(reader.GetDateTime(0));
                        mabyTimes.Remove(time);
                    }
                }
            }
            return mabyTimes;
        }

        public async Task<List<Order>> GetOrderListAsync(HttpContext context)
        {
            List<Order> orders = new List<Order>();

            await using (var conn = new NpgsqlConnection(context.User.FindFirst("connString").Value))
            {

                conn.Open();
                string sqlCommand = string.Format(@"Select * from order_base where name = '{0}' and order_date >= current_date",
                    context.User.FindFirst(ClaimTypes.Name).Value);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Order order = new Order(reader.GetDateTime(1), reader.GetString(2), reader.GetString(3),
                            reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7));
                        orders.Add(order);
                    }
                }
            }
            return orders;
        }

        public async Task DeleteOrderAsync(DateTime dateTime, HttpContext context)
        {
            await using (var conn = new NpgsqlConnection(context.User.FindFirst("connString").Value))
            {
                conn.Open();
                string sqlCommand = string.Format(@"Delete from order_base where name = '{0}' and order_date = '{1}'",
                    context.User.FindFirst(ClaimTypes.Name).Value, dateTime);

                using (var command = new NpgsqlCommand(sqlCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
