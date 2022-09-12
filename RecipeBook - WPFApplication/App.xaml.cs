using System;
using System.Windows;
using System.Data.SqlClient;

namespace RecipeBook
{
    public partial class App : Application
    {
        public static readonly String recipeConnector = "Data Source = localhost\\MSSQLSERVER02; Initial Catalog = RecipeBook; Integrated Security = true";

        public App()
        {
            // Register Syncfusion licence - free Community Licence courtesy of Batrice Ramsay (Global Product Consultant, SyncFusion USA)
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDQ5NDEwQDMxMzkyZTMxMmUzMFZqUVVOVlVNQ1p4ZFZYOG9jenpMUDNoRFNvOC9tWDBkREVpQ2s4SnRJd009");
            CheckIfDatabaseExists();
            CheckForExistingTables();
        }

        // Check if database exists and build if there is none
        private void CheckIfDatabaseExists()
        {
            // Not implemented, as this would require specifics about path of your database, password etc etc
            // See commented-out section at end of code
        }

        // Check if database has existing tables and create them if they do not exist
        private void CheckForExistingTables()
        {
            string checkRecipesExistQuery = "IF OBJECT_ID ('[RecipeBook].[dbo].Recipes', 'U') IS NULL " +
                                            "CREATE TABLE[RecipeBook].[dbo].Recipes " +
                                            "(RecipeID int NOT NULL, " +
                                            "Name varchar(30), " +
                                            "Method text, " +
                                            "CookTime int, " +
                                            "CookTemp int, " +
                                            "Category int);";

            string checkProductsExistQuery = "IF OBJECT_ID('[RecipeBook].[dbo].Products', 'U') IS NULL " +
                                             "BEGIN " +
                                             "CREATE TABLE[RecipeBook].[dbo].Products " +
                                             "(ProdID int NOT NULL, " +
                                             "ProdName varchar(30), " +
                                             "PRIMARY KEY(ProdID)) " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES ('Salt') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Black Pepper') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Rosemary') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Sage') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Thyme') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Oregano') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Bay Leaves') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Nutmeg') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Brown Sugar') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('White Sugar') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Butter') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Milk') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Plain Flour') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Self-Raising Flour') " +
                                             "INSERT [RecipeBook].[dbo].[Products]([ProdName]) VALUES('Eggs') " +
                                             "END";

            string checkIngredientsExistQuery = "IF OBJECT_ID('[RecipeBook].[dbo].Ingredients', 'U') IS NULL " +
                                                "BEGIN " +
                                                "CREATE TABLE[RecipeBook].[dbo].Ingredients " +
                                                "(RecipeID int, " +
                                                "ProdID int, " +
                                                "Measure int, " +
                                                "Quantity float) " +
                                                "END";

            string checkCategoriesExistQuery = "IF OBJECT_ID('[RecipeBook].[dbo].Categories', 'U') IS NULL " +
                                               "BEGIN " +
                                               "CREATE TABLE[RecipeBook].[dbo].Categories " +
                                               "(CatID int NOT NULL, " +
                                               "CatName varchar(20), " +
                                               "PRIMARY KEY (CatID)) " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES ('Meat') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Vegetarian') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Poultry') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Cakes') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Dessert') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Seafood') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Pasta') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Salad') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Biscuits') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Eggs') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Drinks') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Pies') " +
                                               "INSERT [RecipeBook].[dbo].[Categories]([CatName]) VALUES('Savouries') " +
                                               "END";

            string checkMeasuresExistQuery = "IF OBJECT_ID('[RecipeBook].[dbo].Measures', 'U') IS NULL " +
                                             "BEGIN " +
                                             "CREATE TABLE[RecipeBook].[dbo].Measures " +
                                             "(MeasureID int NOT NULL, " +
                                             "Unit varchar(20), " +
                                             "PRIMARY KEY (MeasureID)) " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('tsp') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('tbsp') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('cup') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('oz') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('lb') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('kg') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('pc') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('pinch') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('gm') " +
                                             "INSERT [RecipeBook].[dbo].[Measures]([Unit]) VALUES('sprinkle') " +
                                             "END";
            try
            {
                // Check if Recipes table exists and create if none
                using (SqlConnection databaseConnector = new SqlConnection(recipeConnector))
                {
                    databaseConnector.Open();
                    SqlCommand recipesCheckCommand = new SqlCommand(checkRecipesExistQuery, databaseConnector);
                    SqlDataReader recipesCheck = recipesCheckCommand.ExecuteReader();
                    databaseConnector.Close();
                }

                // Check if Products table exists and seed if none
                using (SqlConnection databaseConnector = new SqlConnection(recipeConnector))
                {
                    databaseConnector.Open();
                    SqlCommand productsCheckCommand = new SqlCommand(checkProductsExistQuery, databaseConnector);
                    SqlDataReader productsCheck = productsCheckCommand.ExecuteReader();
                    databaseConnector.Close();
                }

                // Check if Ingredients table exists and create if none
                using (SqlConnection databaseConnector = new SqlConnection(recipeConnector))
                {
                    databaseConnector.Open();
                    SqlCommand ingredientsCheckCommand = new SqlCommand(checkIngredientsExistQuery, databaseConnector);
                    SqlDataReader ingredientsCheck = ingredientsCheckCommand.ExecuteReader();
                    databaseConnector.Close();
                }

                // Check if Categories table exists and seed if none
                using (SqlConnection databaseConnector = new SqlConnection(recipeConnector))
                {
                    databaseConnector.Open();
                    SqlCommand categoriesCheckCommand = new SqlCommand(checkCategoriesExistQuery, databaseConnector);
                    SqlDataReader categoriesCheck = categoriesCheckCommand.ExecuteReader();
                    databaseConnector.Close();
                }

                // Check if Measures table exists and seed if none
                using (SqlConnection databaseConnector = new SqlConnection(recipeConnector))
                {
                    databaseConnector.Open();
                    SqlCommand measuresCheckCommand = new SqlCommand(checkMeasuresExistQuery, databaseConnector);
                    SqlDataReader measuresCheck = measuresCheckCommand.ExecuteReader();
                    databaseConnector.Close();
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Database Initialization and seeding\n" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Database Initialization and seeding\n" + error.ToString());
            }
        }
    }
}

/*

// Selected from article at: https://www.codeproject.com/articles/10213/create-an-sql-server-database-using-csharp
private void CreateDatabase(DatabaseParam DBParam)
{
    System.Data.SqlClient.SqlConnection tmpConn;
    string sqlCreateDBQuery;
    tmpConn = new SqlConnection();
    tmpConn.ConnectionString = "SERVER = " + DBParam.ServerName + 
                         "; DATABASE = master; User ID = sa; Pwd = sa";
    sqlCreateDBQuery = " CREATE DATABASE "
                       + DBParam.DatabaseName
                       + " ON PRIMARY " 
                       + " (NAME = " + DBParam.DataFileName +", "
                       + " FILENAME = '" + DBParam.DataPathName +"', " 
                       + " SIZE = 2MB,"
                       + " FILEGROWTH =" + DBParam.DataFileGrowth +") "
                       + " LOG ON (NAME =" + DBParam.LogFileName +", "
                       + " FILENAME = '" + DBParam.LogPathName + "', " 
                       + " SIZE = 1MB, "
                       + " FILEGROWTH =" + DBParam.LogFileGrowth +") ";
     
    SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, tmpConn);
    
    try
     {
         tmpConn.Open();
         MessageBox.Show(sqlCreateDBQuery);
         myCommand.ExecuteNonQuery();
         MessageBox.Show("Database has been created successfully!", 
                         "Create Database", MessageBoxButtons.OK, 
                         MessageBoxIcon.Information);
      }
     catch (System.Exception ex)
     {
         MessageBox.Show(ex.ToString(), "Create Database", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Information);
     }
     finally
     {
         tmpConn.Close();
     }

     return;
}
*/