using BLL_DB.DTOModels;
using BLL_DB.ServiceInterfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_DB
{
    public class ProductService : IProductService
    {
        public void ActivateProduct(int productId)
        {
            executeSql(String.Format("exec dbo.ActivateProduct {0}", productId));
        }

        public void AddProduct(ProductRequestDTO productDTO)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=lab6;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                string sql = "exec dbo.AddProduct @Name, @Price, @Image, @GroupId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", productDTO.Name);
                    command.Parameters.AddWithValue("@Price", productDTO.Price);
                    command.Parameters.AddWithValue("@Image", productDTO.Image);
                    command.Parameters.AddWithValue("@GroupId", productDTO.GroupId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeactivateProduct(int productId)
        {
            executeSql(String.Format("exec dbo.DeactivateProduct {0}", productId));
        }

        public void DeleteProduct(int productId)
        {
            executeSql(String.Format("delete from dbo.Products where Id = {0}", productId));
        }

        public IEnumerable<ProductResponseDTO> GetProducts(string sortBy, string filterByName, string filterByGroupName, int? filterByGroupId, bool includeInactive)
        {
            List<ProductResponseDTO> products = new List<ProductResponseDTO>();
            string sql = "exec dbo.GetProducts @IncludeInactive";

            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=lab6;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IncludeInactive", includeInactive);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductResponseDTO
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = (double)reader.GetDecimal(2), // Ensure correct conversion from decimal to double
                                GroupId = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            // Filter results
            if (!string.IsNullOrEmpty(filterByName))
            {
                products = products.Where(p => p.Name.Contains(filterByName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (filterByGroupId.HasValue)
            {
                products = products.Where(p => p.GroupId == filterByGroupId.Value).ToList();
            }

            // Sort results
            switch (sortBy)
            {
                case "Name":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "Price":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "GroupName": // Remove if GroupName is not required
                    // products = products.OrderBy(p => p.GroupName).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Id).ToList();
                    break;
            }

            return products;
        }

        private string executeSql(string sql)
        {
            string output = "";
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=lab6;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            output += reader[i] + ";";
                        }
                        output += '\n';
                    }
                }
            }
            return output;
        }
    }
}
