using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RecipeBook
{
    public class Recipe : IRecipesDatabase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
        public int Time { get; set; }
        public int Temp { get; set; }
        public int Category { get; set; }
        public int Serve { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Recipe()
        {

        }

        public Recipe(int id, string name, int cat)
        {
            this.ID = id;
            this.Name = name;
            this.Category = cat;
        }

        public Recipe(string name, string method, int cook, int temp, int cat, List<Ingredient> ingredients)
        {
            this.Name = name;
            this.Method = method;
            this.Time = cook;
            this.Temp = temp;
            this.Category = cat;
            this.Ingredients = ingredients;
        }

        public Recipe(int id, string name, string method, int cook, int temp, int cat, List<Ingredient> ingredients)
        {
            this.ID = id;
            this.Name = name;
            this.Method = method;
            this.Time = cook;
            this.Temp = temp;
            this.Category = cat;
            this.Ingredients = ingredients;
        }

        public List<Ingredient> GetRecipeIngredients(int recipeNum)
        {
            List<Ingredient> ingredientList = new List<Ingredient>();
            string readIngredientsQuery = "SELECT * FROM [RecipeBook].[dbo].Ingredients " +
                                          "WHERE RecipeID = " + recipeNum.ToString();

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
                Console.WriteLine("SQL error - Create Recipe Ingredients List\n" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Create Recipe Ingredients List\n" + error.ToString());
            }

            return ingredientList;
        }

        // Create list of recipes from database table
        public List<T> ReadTableFromDatabase<T>()
        {
            List<Recipe> recipeList = new List<Recipe>();
            string readRecipesQuery = "SELECT * FROM [RecipeBook].[dbo].Recipes";

            try
            {
                using (SqlConnection connector = new SqlConnection(App.recipeConnector))
                {                   
                    connector.Open();
                    SqlCommand command = new SqlCommand(readRecipesQuery, connector);
                    SqlDataReader reader = command.ExecuteReader();
    
                    while (reader.Read())
                    {
                        Recipe thisRecipe = new Recipe
                        {
                            ID = Convert.ToInt32(reader["RecipeID"]),
                            Name = (string)reader["Name"],
                            Method = (string)reader["Method"],
                            Time = Convert.ToInt32(reader["CookTime"]),
                            Temp = Convert.ToInt32(reader["CookTemp"]),
                            Category = Convert.ToInt32(reader["Category"])
                        };
                        recipeList.Add(thisRecipe);
                    }

                    connector.Close();
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("An error occurred - Create Recipe List\n" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Create Recipe List\n" + error.ToString());
            }

            // Sort list alphabetically on recipe name
            recipeList = (List<Recipe>)recipeList.OrderBy(r => r.Name).ToList();

            return recipeList as List<T>;
        }

        // Retrieve selected recipe from database 
        public T ReadItemFromDatabase<T>(int recipeNum)
        {
            Recipe thisRecipe;
            Ingredient ingrInstance = new Ingredient();

            // Read ingredients for this recipe from database
            List<Ingredient> recipeIngredients = GetRecipeIngredients(recipeNum);

            // Read this recipe
            List<Recipe> allRecipes = ReadTableFromDatabase<Recipe>();
            thisRecipe = (from r in allRecipes
                          where r.ID == recipeNum
                          select new Recipe { ID = r.ID, Name = r.Name, Method = r.Method, Time = r.Time, Temp = r.Temp, Category = r.Category, Ingredients = recipeIngredients }).FirstOrDefault();

            return (T)(thisRecipe as object);
        }

        // Modify details of selected recipe in database
        public Boolean UpdateItemInDatabase<T>(T item)
        {
            bool isSuccess = false;
            Recipe editRecipe = (Recipe)(item as object);

            // Get ingredient list for this recipe
            List<Ingredient> ingredientList = editRecipe.Ingredients;

            SqlConnection connector = new SqlConnection(App.recipeConnector);
            SqlTransaction editTransaction;

            string editRecipeQuery = "UPDATE [RecipeBook].[dbo].Recipes " +
                                     "SET Name = '" + editRecipe.Name + "', " +
                                     "Method = '" + editRecipe.Method + "', " +
                                     "CookTime = " + editRecipe.Time.ToString() + ", " +
                                     "CookTemp = " + editRecipe.Temp.ToString() + ", " +
                                     "Category = " + editRecipe.Category.ToString() + " " +
                                     "WHERE RecipeID = " + editRecipe.ID.ToString();

            connector.Open();
            editTransaction = connector.BeginTransaction();

            try
            {
                // Update recipe in database
                new SqlCommand(editRecipeQuery, connector, editTransaction).ExecuteNonQuery();

                // Update ingredient list of this recipe (delete existing ingredients first)
                Ingredient ingrInstance = new Ingredient();
                ingrInstance.DeleteItemFromDatabase<Ingredient>(editRecipe.ID);

                foreach (Ingredient ingredient in ingredientList)
                {
                    ingredient.RecipeID = editRecipe.ID;
                    ingrInstance.AddNewItemToDatabase<Ingredient>(ingredient);
                }

                // Commit the transaction
                editTransaction.Commit();
                isSuccess = true;
            }
            // If an error occurs, then rollback the transaction
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Edit Recipe\nTransaction has been rolled-back\n" + error.ToString());
                isSuccess = false;
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Edit Recipe\nTransaction has been rolled-back\n" + error.ToString());
                isSuccess = false;
            }
            finally
            {
                connector.Close();
            }

            return isSuccess;
        }

        // Add new recipe to database
        public Boolean AddNewItemToDatabase<T>(T item)
        {
            bool isSuccess = false;
            Recipe newRecipe = (Recipe)(item as object);

            // Get ingredient list for this recipe
            List<Ingredient> ingredientList = newRecipe.Ingredients;
            
            SqlConnection connector = new SqlConnection(App.recipeConnector);            
            SqlTransaction addTransaction;

            string newRecipeQuery = "INSERT INTO [RecipeBook].[dbo].Recipes " +
                                    "(RecipeID, Name, Method, CookTime, CookTemp, Category) " +
                                    "VALUES (" + newRecipe.ID + ", " +
                                    "'" + newRecipe.Name + "', " +
                                    "'" + newRecipe.Method + "', " +
                                    newRecipe.Time + ", " +
                                    newRecipe.Temp + ", " +
                                    newRecipe.Category + ");";

            connector.Open();
            addTransaction = connector.BeginTransaction();

            try
            {
                // Add recipe to database
                new SqlCommand(newRecipeQuery, connector, addTransaction).ExecuteNonQuery();

                // Add ingredient list for this recipe
                Ingredient ingrInstance = new Ingredient();
                foreach (Ingredient ingredient in ingredientList)
                {
                    ingrInstance.AddNewItemToDatabase<Ingredient>(ingredient);
                }

                // Commit the transaction
                addTransaction.Commit();
                isSuccess = true;
            }
            // If an error occurs, then rollback the transaction
            catch (SqlException error)
            {
                addTransaction.Rollback();
                Console.WriteLine("SQL error - Add New Recipe\nTransaction has been rolled-back.\n" + error.ToString());
                isSuccess = false;
            }
            catch (Exception error)
            {
                addTransaction.Rollback();
                Console.WriteLine("An error occurred - Add New Recipe\nTransaction has been rolled-back.\n" + error.ToString());
                isSuccess = false;
            }
            finally
            {
                connector.Close();
            }

            return isSuccess;
        }

        //  Delete selected recipe and associated ingredient list from database
        public Boolean DeleteItemFromDatabase<T>(int recipeNum)
        {
            bool isSuccess = false;
            SqlConnection connector = new SqlConnection(App.recipeConnector);
            SqlTransaction deleteTransaction;
            
            string deleteRecipeQuery = "DELETE FROM [RecipeBook].[dbo].Recipes " +
                                       "WHERE RecipeID = " + recipeNum.ToString();

            connector.Open();
            deleteTransaction = connector.BeginTransaction();
            
            try
            {
                // Delete Recipe
                new SqlCommand(deleteRecipeQuery, connector, deleteTransaction).ExecuteNonQuery();

                // Delete Ingredients of recipe
                Ingredient ingrInstance = new Ingredient();
                ingrInstance.DeleteItemFromDatabase<Ingredient>(recipeNum);

                // Remove recipe from Favourites file
                Favourite faveInstance = new Favourite();
                faveInstance.DeleteFavourite(recipeNum);

                // Commit the transaction
                deleteTransaction.Commit();
                isSuccess = true;
            }
            // If an error occurs, then rollback the transaction
            catch (SqlException error)
            {
                deleteTransaction.Rollback();
                Console.WriteLine("SQL error - Delete Recipe\nTransaction has been rolled-back.\n" + error.ToString());
                isSuccess = false;
            }
            catch (Exception error)
            {
                deleteTransaction.Rollback();
                Console.WriteLine("An error occurred - Delete Recipe\nTransaction has been rolled-back.\n" + error.ToString());
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
