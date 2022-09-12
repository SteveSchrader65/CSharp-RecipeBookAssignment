using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RecipeBook
{
    public class Category : IRecipesDatabase
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Category()
        {

        }

        public Category(string name)
        {
            this.Name = name;
        }

        public Category(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        // Create list of all categories in database
        public List<T> ReadTableFromDatabase<T>()
        {
            List<Category> categoryList = new List<Category>();
            string readCategoriesQuery = "SELECT * FROM [RecipeBook].[dbo].Categories";

            try
            {
                using (SqlConnection connector = new SqlConnection(App.recipeConnector))
                {
                    connector.Open();
                    SqlCommand command = new SqlCommand(readCategoriesQuery, connector);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Category thisCategory = new Category
                        {
                            ID = Convert.ToInt32(reader["CatID"]),
                            Name = (string)reader["CatName"]
                        };
                        categoryList.Add(thisCategory);
                    }

                    connector.Close();
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Read Category List\n" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Read Category List\n" + error.ToString());
            }
            return categoryList as List<T>;
        }

        // Read name of selected category from database
        public T ReadItemFromDatabase<T>(int categoryNum)
        {
            List<Category> allCategories = ReadTableFromDatabase<Category>();

            Category thisCategory = (from c in allCategories
                                     where c.ID == categoryNum
                                     select new Category { ID = c.ID, Name = c.Name }).FirstOrDefault();

            return (T)(thisCategory as object);
        }

        public Boolean UpdateItemInDatabase<T>(T item)
        {
            throw new NotImplementedException();
        }

        public Boolean AddNewItemToDatabase<T>(T item)
        {
            throw new NotImplementedException();
        }

        public Boolean DeleteItemFromDatabase<T>(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
