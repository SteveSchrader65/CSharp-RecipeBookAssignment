using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeBook
{
    // This class is used for displaying the details of a recipe in the View/EditRecipe and AddRecipe modals
    public class IngredientDisplay
    {
        public string Ingredient { get; set; }
        public float Quantity { get; set; }
        public string Measure { get; set; }
        
        public IngredientDisplay()
        {

        }

        public IngredientDisplay(string product, int quantity, string measure)
        {
            Ingredient = product;
            Quantity = quantity;
            Measure = measure;
        }
    }

    public class Ingredient : IRecipesDatabase
    {
        public int RecipeID { get; set; }
        public int ProdID { get; set; }
        public int Measure { get; set; }
        public decimal Quantity { get; set; }

        public Ingredient()
        {

        }

        public Ingredient(int recipeNum, int prodNum, int measure, decimal quantity)
        {
            this.RecipeID = recipeNum;
            this.ProdID = prodNum;
            this.Measure = measure;
            this.Quantity = quantity;
        }

        // Create list of ingredients required for all recipes from database table
        public List<T> ReadTableFromDatabase<T>()
        {
            List<Ingredient> ingredientList = new List<Ingredient>();
            string readIngredientsQuery = "SELECT * FROM [RecipeBook].[dbo].Ingredients";

            try
            {
                using (SqlConnection connector = new SqlConnection(App.recipeConnector))
                {
                    connector.Open();
                    SqlCommand command = new SqlCommand(readIngredientsQuery, connector);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Ingredient thisIngredient = new Ingredient
                        {
                            RecipeID = Convert.ToInt32(reader["RecipeID"]),
                            ProdID = Convert.ToInt32(reader["ProdID"]),
                            Measure = Convert.ToInt32(reader["Measure"]),
                            Quantity = Convert.ToDecimal(reader["Quantity"])
                        };
                        ingredientList.Add(thisIngredient);
                    }

                    connector.Close();
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Create Ingredient List\n" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Create Ingredient List\n" + error.ToString());
            }

            return ingredientList as List<T>;
        }

        public T ReadItemFromDatabase<T>(int recipeNum)
        {
            throw new NotImplementedException();
        }

        // Modify ingredient list for selected recipe
        public Boolean UpdateItemInDatabase<T>(T item)
        {
            throw new NotImplementedException();
        }

        // Add list of ingredients for this recipe to database
        public Boolean AddNewItemToDatabase<T>(T item)
        {
            bool isSuccess = false;

            Ingredient newIngredient = (Ingredient)(item as object);
            SqlConnection connector = new SqlConnection(App.recipeConnector);
            SqlDataAdapter adapter = new SqlDataAdapter();

            string newIngredientQuery = "INSERT INTO [RecipeBook].[dbo].Ingredients " +
                                        "(RecipeID, ProdID, Measure, Quantity) " +
                                        "VALUES (" + newIngredient.RecipeID + ", " +
                                        newIngredient.ProdID + ", " +
                                        newIngredient.Measure + ", " +
                                        newIngredient.Quantity + ");";

            try
            {
                connector.Open();
                adapter.InsertCommand = new SqlCommand(newIngredientQuery, connector);
                adapter.InsertCommand.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Add New Ingredient\n" + error.ToString());
                isSuccess = false;
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Add New Ingredient\n" + error.ToString());
                isSuccess = false;
            }
            finally
            {
                connector.Close();
            }

            return isSuccess;
        }

        // Delete ingredients of selected recipe from database
        public Boolean DeleteItemFromDatabase<T>(int recipeNum)
        {
            bool isSuccess = false;

            SqlConnection connector = new SqlConnection(App.recipeConnector);

            string deleteIngredients = "DELETE FROM [RecipeBook].[dbo].[Ingredients] " +
                                       "WHERE RecipeID = " + recipeNum.ToString();

            connector.Open();

            try
            {
                new SqlCommand(deleteIngredients, connector).ExecuteNonQuery();
                isSuccess = true;
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Delete Recipe Ingredients\n" + error.ToString());
                isSuccess = false;
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Delete Recipe Ingredients\n" + error.ToString());
                isSuccess = false;
            }
            finally
            {
                connector.Close();
            }

            return isSuccess;
        }
    }
}
