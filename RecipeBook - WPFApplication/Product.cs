using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RecipeBook
{
    public class Product : IRecipesDatabase
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Product()
        {

        }

        public Product(string name)
        {
            this.Name = name;
        }

        public Product(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        // Create list of available products from database table
        public List<T> ReadTableFromDatabase<T>()
        {
            List<Product> productList = new List<Product>();
            string readProductsQuery = "SELECT * FROM [RecipeBook].[dbo].Products";

            try
            {
                using (SqlConnection connector = new SqlConnection(App.recipeConnector))
                {
                    connector.Open();
                    SqlCommand command = new SqlCommand(readProductsQuery, connector);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product thisProduct = new Product
                        {
                            ID = Convert.ToInt32(reader["ProdID"]),
                            Name = (string)reader["ProdName"]
                        };
                      
                        productList.Add(thisProduct);
                    }

                    connector.Close();
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Create Product List" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Create Product List" + error.ToString());
            }

            return productList as List<T>;
        }

        // Retrieve details of selected product from database 
        public T ReadItemFromDatabase<T>(int product)
        {
            List<Product> allProducts = ReadTableFromDatabase<Product>();

            Product thisProduct = (from p in allProducts
                                   where p.ID == product
                                   select new Product { ID = p.ID, Name = p.Name }).FirstOrDefault();

            return (T)(thisProduct as object);
        }

        public Boolean UpdateItemInDatabase<T>(T item)
        {
            throw new NotImplementedException();
        }

        // Add new product to database
        public Boolean AddNewItemToDatabase<T>(T item)
        {
            bool isSuccess = false;

            Product newProduct = (Product)(item as object);             
            SqlConnection connector = new SqlConnection(App.recipeConnector);
            SqlDataAdapter adapter = new SqlDataAdapter();

            string newProductQuery = "INSERT INTO [RecipeBook].[dbo].Products (ProdName) " +
                                     "VALUES ('" + newProduct.Name + "')";

            try
            {
                connector.Open();
                adapter.InsertCommand = new SqlCommand(newProductQuery, connector);
                adapter.InsertCommand.ExecuteNonQuery();
                isSuccess = true;
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Add New Product\n" + error.ToString());
                isSuccess = false;
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Add New Product\n" + error.ToString());
                isSuccess = false;
            }
            finally
            {
                connector.Close();
            }

            return isSuccess;
        }

        public Boolean DeleteItemFromDatabase<T>(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
