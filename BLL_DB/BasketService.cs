using BLL_DB.ServiceInterfaces;
using Microsoft.Data.SqlClient;
using System;

namespace BLL_DB
{
    public class BasketService : IBasketService
    {
        public void AddProductToBasket(int productId, int userId)
        {
            string sql = "exec dbo.AddProductToBasket @ProductId, @UserId";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@UserId", userId);
                ExecuteSql(command);
            }
        }

        public void ChangeBasketPositionQuantity(int productId, int userId, int newQuantity)
        {
            string sql = "exec dbo.ChangeBasketPositionQuantity @ProductId, @UserId, @NewQuantity";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@NewQuantity", newQuantity);
                ExecuteSql(command);
            }
        }

        public void RemoveProductFromBasket(int productId, int userId)
        {
            string sql = "exec dbo.RemoveProductFromBasket @UserId, @ProductId";
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ProductId", productId);
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
