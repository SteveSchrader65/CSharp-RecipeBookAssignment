using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;

namespace RecipeBook 
{
    [Serializable]
    public class Favourite
    {
        public int RecipeNum { get; set; }

        public Favourite()
        {

        }

        public Favourite(int num)
        {
            RecipeNum = num;
        }

        // Load list of favourites from data file. Create new file if none exists.
        public List<Favourite> ReadFavouritesList()
        {
            string fileName = "FaveList.bin";
            List<Favourite> faveList = new List<Favourite>();
            Stream file = File.Open(fileName, FileMode.OpenOrCreate);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                faveList = (List<Favourite>)formatter.Deserialize(file);
            }
            catch (IOException e)
            {
                Console.WriteLine("File access error. " + e.Message);
                faveList = new List<Favourite>();
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Serialization error.\n" + e.Message);
                faveList = new List<Favourite>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Update file. " + e.Message);
                faveList = new List<Favourite>();
            }
            finally
            {
                file.Close();
            }

            return faveList;
        }

        // Create new file of favourites list
        public void WriteFavouritesList(List<Favourite> faveList)
        {
            string fileName = "FaveList.bin";
            Stream file = File.Open(fileName, FileMode.Create);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, faveList);
            }
            catch (IOException e)
            {
                Console.WriteLine("File access error.\n" + e.Message);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Serialization error.\n" + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Write to file.\n" + e.Message);
            }
            finally
            {
                file.Close();
            }

            return;
        }

        public void DeleteFavourite(int num)
        {
            // Get FavouritesList
            List<Favourite> faveList = ReadFavouritesList();

            // Remove recipe from FavouritesList
            Favourite thisFave = faveList.FirstOrDefault(f => f.RecipeNum == num);
            faveList.Remove(thisFave);

            // Write FavouritesList
            WriteFavouritesList(faveList);

            return;
        }
    }
}
