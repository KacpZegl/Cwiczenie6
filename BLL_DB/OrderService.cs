using BLL_DB.DTOModels;
using BLL_DB.ServiceInterfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace BLL_DB
{
    public class OrderService : IOrderService
    {
        public void GenerateOrderFromBasket(int userId)
        {
            string sql = "exec dbo.GenerateOrderFromBasket @UserId";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                ExecuteSql(command);
            }
        }

        public IEnumerable<OrderPositionResponseDTO> GetOrderPositions(int orderId)
        {
            List<OrderPositionResponseDTO> orderPositions = new List<OrderPositionResponseDTO>();
            string sql = "exec dbo.GetOrderPositions @OrderId";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=lab6;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    command.Connection = connection;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderPositionResponseDTO orderPosition = new OrderPositionResponseDTO
                            {
                                ProductName = reader["ProductName"].ToString(),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity")), // Zmienione na decimal
                                TotalValue = reader.GetDecimal(reader.GetOrdinal("TotalValue"))
                            };
                            orderPositions.Add(orderPosition);
                        }
                    }
                }
            }
            return orderPositions;
        }

        public IEnumerable<OrderResponseDTO> GetOrders(string sortBy, int? orderIdFilter, bool? paidStatusFilter)
        {
            List<OrderResponseDTO> orders = new List<OrderResponseDTO>();
            string sql = "exec dbo.GetOrders @SortBy, @OrderIdFilter, @PaidStatusFilter";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@SortBy", sortBy ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@OrderIdFilter", orderIdFilter ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PaidStatusFilter", paidStatusFilter ?? (object)DBNull.Value);
                using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=lab6;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    command.Connection = connection;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderResponseDTO order = new OrderResponseDTO
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Paid = reader.GetBoolean(reader.GetOrdinal("Paid")),
                                Total = reader.GetDecimal(reader.GetOrdinal("Total"))
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public void PayOrder(int orderId, decimal amountPaid)
        {
            string sql = "exec dbo.PayOrder @OrderId, @AmountPaid";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@AmountPaid", amountPaid);
                ExecuteSql(command);
            }
        }

        private void ExecuteSql(SqlCommand command)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=lab6;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
