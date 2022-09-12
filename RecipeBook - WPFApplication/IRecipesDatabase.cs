using System.Collections.Generic;

namespace RecipeBook
{
    public interface IRecipesDatabase
    {
        // BROWSE all items in table 
        List<T> ReadTableFromDatabase<T>();

        // READ single item from table
        T ReadItemFromDatabase<T>(int ID);

        // EDIT item in table
        bool UpdateItemInDatabase<T>(T item);

        // ADD item to table
        bool AddNewItemToDatabase<T>(T item);

        // DELETE item from table
        bool DeleteItemFromDatabase<T>(int ID);
    }
}
