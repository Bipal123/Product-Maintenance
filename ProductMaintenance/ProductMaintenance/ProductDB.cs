using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProductMaintenance
{
    public class ProductDB
    {
        public static Product GetProduct(string productCode)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string selectStatement = "SELECT ProductCode, Description, UnitPrice, OnHandQuantity " +
                                      "FROM Products " + "WHERE ProductCode = @ProductCode;";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@ProductCode", productCode);
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    Product product = new Product
                    {
                        Code = reader["ProductCode"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDecimal(reader["UnitPrice"])
                    };
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }
        public static bool UpdateProduct (Product originalProduct, Product updatedProduct)
        {
            SqlConnection connection= MMABooksDB.GetConnection();
            string updateStatement = "UPDATE Products SET " +
                                        "Description = @newDescription, " +
                                        "UnitPrice = @newPrice " +
                                        "WHERE ProductCode = @oldCode " +
                                        "AND Description = @oldDescription " +
                                        "AND UnitPrice = @oldPrice;";
            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
            updateCommand.Parameters.AddWithValue("@newDescription", updatedProduct.Description);
            updateCommand.Parameters.AddWithValue("@newPrice", updatedProduct.Price);
            updateCommand.Parameters.AddWithValue("@oldCode", originalProduct.Code);
            updateCommand.Parameters.AddWithValue("@oldDescription", originalProduct.Description);
            updateCommand.Parameters.AddWithValue("@oldPrice", originalProduct.Price);
            try
            {
                connection.Open();
                if (updateCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public static bool AddProduct(Product product)
        {
            List<Product> allProducts = GetAllProducts();
            bool ifProductExists = false;
            foreach (Product p in allProducts)
            {
                if (p.Code == product.Code)
                {
                    ifProductExists = true;
                    break;
                }
            }
            if (!ifProductExists)
            {
                SqlConnection connection = MMABooksDB.GetConnection();
                string insertStatement = "INSERT Products (ProductCode, Description, UnitPrice) " +
                                            "VALUES (@ProductCode, @Description, @UnitPrice);";
                SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
                insertCommand.Parameters.AddWithValue("@ProductCode", product.Code);
                insertCommand.Parameters.AddWithValue("@Description", product.Description);
                insertCommand.Parameters.AddWithValue("@UnitPrice", product.Price);
                try
                {
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                return false;
            }
        }
        public static bool DeleteProduct(Product product)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string deleteStatement = "DELETE FROM Products " +
                                        "WHERE ProductCode = @Code " +
                                        "AND Description = @Description " +
                                        "AND UnitPrice = @Price;";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("@Code", product.Code);
            deleteCommand.Parameters.AddWithValue("@Description", product.Description);
            deleteCommand.Parameters.AddWithValue("@Price", product.Price);
            try
            {
                connection.Open();
                if (deleteCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        private static List<Product> GetAllProducts()
        {
            List<Product> allProducts = new List<Product>();
            SqlConnection connection = MMABooksDB.GetConnection();
            string selectStatement = "SELECT ProductCode, Description, UnitPrice, OnHandQuantity " +
                                      "FROM Products; ";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Code = reader["ProductCode"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDecimal(reader["UnitPrice"])
                    };
                    allProducts.Add(product);
                }
                return allProducts;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
