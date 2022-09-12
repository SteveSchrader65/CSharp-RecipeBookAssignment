using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RecipeBook
{
    public class Measure : IRecipesDatabase
    {
        public int ID { get; set; }
        public string Unit { get; set; }

        public Measure()
        {

        }

        public Measure(int id)
        {
            this.ID = id;
        }

        public Measure(int id, string unit)
        {
            this.ID = id;
            this.Unit = unit;
        }

        // Create list of all measures details from database
        public List<T> ReadTableFromDatabase<T>()
        {
            List<Measure> measureList = new List<Measure>();
            string readMeasuresQuery = "SELECT * FROM [RecipeBook].[dbo].Measures";

            try
            {
                using (SqlConnection connector = new SqlConnection(App.recipeConnector))
                {
                    connector.Open();
                    SqlCommand command = new SqlCommand(readMeasuresQuery, connector);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Measure thisMeasure = new Measure
                        {
                            ID = Convert.ToInt32(reader["MeasureID"]),
                            Unit = (string)reader["Unit"]
                        };

                        measureList.Add(thisMeasure);
                    }

                    connector.Close();
                }
            }
            catch (SqlException error)
            {
                Console.WriteLine("SQL error - Read Measure List\n" + error.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine("An error occurred - Read Measure List\n" + error.ToString());
            }

            return measureList as List<T>;
        }

        // Read details of selected measure from database
        public T ReadItemFromDatabase<T>(int measureNum)
        {
            List<Measure> allMeasures = ReadTableFromDatabase<Measure>();

            Measure thisMeasure = (from m in allMeasures
                                   where m.ID == measureNum
                                   select new Measure { ID = m.ID, Unit = m.Unit }).FirstOrDefault();

            return (T)(thisMeasure as object);
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
