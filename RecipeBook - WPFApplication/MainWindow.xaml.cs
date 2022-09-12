using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecipeBook
{
    public partial class MainWindow : Window
    {
        readonly Recipe recInstance = new Recipe();
        readonly Category catInstance = new Category();
        readonly Product prodInstance = new Product();
        readonly Ingredient ingrInstance = new Ingredient();
        readonly Favourite faveInstance = new Favourite();

        List<Recipe> recipeList;
        List<Category> catList;
        List<Product> prodList;
        List<Ingredient> ingrList;
        List<Favourite> faveList;

        static string criteria;
        Boolean isOptionSearch = true;

        public MainWindow()
        {
            InitializeComponent();
            lstRecipes.SelectedIndex = -1;
            lstSearch.SelectedIndex = -1;
            DisplaySearchOptions();
        }

        // Display recipe search criteria to search listbox
        private void DisplaySearchOptions()
        {
            List<string> criteriaList = new List<string>
            {
                "Name",
                "Category",
                "Ingredient",
                "Favourites"
            };

            lblSearch.Content = "Search Recipes by ...";
            lstSearch.Items.Clear();

            foreach (string criteria in criteriaList)
            {
                lstSearch.Items.Add(criteria);
            }
        }

        // Display chosen criteria options to search listbox
        private void DisplaySearchList(string searchCriteria)
        {
            catList = catInstance.ReadTableFromDatabase<Category>();
            prodList = prodInstance.ReadTableFromDatabase<Product>();
            
            lstSearch.Items.Clear();
            isOptionSearch = false;
            criteria = searchCriteria;

            switch (searchCriteria)
            {
                case "Name":
                    DisplayRecipeList("Name", -1);
                    break;

                case "Category":
                    lblSearch.Content = "Search Recipes from ...";
                    foreach (Category category in catList)
                    {
                        lstSearch.Items.Add(category.Name);
                    }
                    break;

                case "Ingredient":    
                    lblSearch.Content = "Search Recipes with ...";
                    foreach (Product ingredient in prodList)
                    {
                        lstSearch.Items.Add(ingredient.Name);
                    }                    
                    break;

                case "Favourites":
                    lblSearch.Content = "Select from 'Faves'";
                    DisplayRecipeList("Favourites", 1);
                    break;
            }
        }

        // Display recipe list for selected search criteria to recipe listbox
        private void DisplayRecipeList(string criteria, int selected)
        {
            string selection = "";
            recipeList = recInstance.ReadTableFromDatabase<Recipe>();

            switch (criteria)
            {
                // All recipes
                case "Name":
                    break;

                // Recipes of selected category
                case "Category":
                    recipeList = (from r in recipeList 
                                  where r.Category == selected
                                  select new Recipe { ID = r.ID, Name = r.Name }).ToList();

                    selection = (from c in catList
                                 where c.ID == selected
                                 select c.Name).FirstOrDefault();                        
                    break;

                // Recipes containing selected ingredient
                case "Ingredient":
                    prodList = prodInstance.ReadTableFromDatabase<Product>();
                    ingrList = ingrInstance.ReadTableFromDatabase<Ingredient>();

                    recipeList = (from r in recipeList
                                  join i in ingrList on r.ID equals i.RecipeID into table1
                                  from i in table1.ToList()
                                  join p in prodList on i.ProdID equals p.ID into table2
                                  from p in table2.ToList()
                                  where p.ID == selected
                                  select new Recipe { ID = r.ID, Name = r.Name }).Distinct<Recipe>().ToList();

                    selection = (from p in prodList
                                 where p.ID == selected
                                 select p.Name).FirstOrDefault();
                    break;

                // Recipes flagged as 'Faves' added to list
                case "Favourites":
                    faveList = faveInstance.ReadFavouritesList();
                    List<int> faveIDNumbers = faveList.Select(f => f.RecipeNum).ToList();

                    if (faveList.Count > 0)
                    {
                        recipeList = (from r in recipeList
                                      where faveIDNumbers.Contains(r.ID)
                                      select new Recipe { ID = r.ID, Name = r.Name }).ToList();
                    }
                    else
                    {
                        recipeList = new List<Recipe>();
                    }
                    break;
            }

            // Display correct spacing based upon length of recipe name
            foreach (Recipe recipe in recipeList)
            {                
                if (recipe.Name.Length <= 10)
                {
                    lstRecipes.Items.Add(recipe.Name + "\t\t\t\tpg" + recipe.ID);
                }
                else if (recipe.Name.Length <= 18)
                {
                    lstRecipes.Items.Add(recipe.Name + "\t\t\tpg" + recipe.ID);
                }
                else
                {
                    lstRecipes.Items.Add(recipe.Name + "\t\tpg" + recipe.ID);
                }
            }

            lstSearch.Items.Clear();
            ResetSearchList();
            CountRecipes(criteria, selection);
        }

        // Clear search listbox and re-display search criteria
        private void ResetSearchList()
        {
            isOptionSearch = true;
            criteria = "";            
            DisplaySearchOptions();
        }

        // Count recipes in recipe listbox and update UI message
        private void CountRecipes(string criteria, string selectedString)
        {
            string noRecipesString = "";

            if (lstRecipes.Items.Count == 0)
            {
                switch (criteria)
                {
                    case "Name":
                        noRecipesString = "There are no recipes to display";
                        break;

                    case "Category":
                        noRecipesString = "There are no " + selectedString + " recipes";
                        break;

                    case "Ingredient":
                        noRecipesString = "There are no recipes with " + selectedString;
                        break;

                    case "Favourites":
                        noRecipesString = "There are no Favourites selected";
                        break;
                }

                DelayMessage(lblErrorMessage, 3, noRecipesString);
                lblInstructions.Visibility = Visibility.Hidden;
            }
            else
            {
                lblInstructions.Visibility = Visibility.Visible;
            }
        }

        // Displays Add New Recipe modal box
        private void BtnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWindow newRecipeModal = new AddRecipeWindow()
            {
                Location = new Point(1500, 400)
            };

            newRecipeModal.ShowDialog();

            // Reset listboxes
            ResetSearchList();
            lstRecipes.Items.Clear();
        }

        // Add New Ingredient to Product table
        private void BtnNewIngredient_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow newProductModal = new AddProductWindow()
            {
                Location = new Point(1500, 400)
            };

            newProductModal.ShowDialog();
            ResetSearchList();
        }

        // Convert selected recipe to integer value and open DeleteWindow modal
        private void BtnDeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            int recipeNum = Convert.ToInt32(Regex.Replace(lstRecipes.SelectedItem.ToString(), "[^0-9]", ""));

            DeleteRecipeWindow deleteModal = new DeleteRecipeWindow(recipeNum)
            {
                Location = new Point(1500, 400)
            };

            deleteModal.ShowDialog();
            btnDeleteRecipe.Visibility = Visibility.Hidden;

            // Clear Recipe listbox
            lstRecipes.Items.Clear();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Left listbox. Used to display recipes meeting search criteria
        private void LstRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRecipes.SelectedIndex == -1)
            {
                // Remove UI message when listbox is cleared
                lblInstructions.Visibility = Visibility.Hidden;
            }
            else
            {
                // Show delete button when recipe selected
                btnDeleteRecipe.Visibility = Visibility.Visible;
            }
        }

        // Mouse double-click event on selected recipe 
        private void LstRecipes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstRecipes.SelectedItem.ToString() != "There are no recipes to display")
            {
                // Hide delete button ...
                btnDeleteRecipe.Visibility = Visibility.Hidden;

                // Convert selected recipe to integer value and open View/Edit modal
                int recipeNum = Convert.ToInt32(Regex.Replace(lstRecipes.SelectedItem.ToString(), "[^0-9]", ""));

                ViewRecipeWindow viewModal = new ViewRecipeWindow(recipeNum)
                {
                    Location = new Point(1500, 400)
                };

                viewModal.ShowDialog();
                lstRecipes.Items.Clear();
            }
        }

        // Right listbox. Used to display search criteria items 
        private void LstSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Display list of search criteria 
            if (isOptionSearch)
            {
                criteria = (string)lstSearch.SelectedItem;
                lstRecipes.Items.Clear();
                lblInstructions.Visibility = Visibility.Hidden;
                btnDeleteRecipe.Visibility = Visibility.Hidden;
                DisplaySearchList(criteria);
            }

            // Display list of recipes meeting search criteria    
            else
            {
                lblInstructions.Visibility = Visibility.Hidden;
                DisplayRecipeList(criteria, (int)lstSearch.SelectedIndex + 1);
            }
        }

        private async void DelayMessage(Label labelName, int duration, string message)
        {
            // Display label
            labelName.Visibility = Visibility.Visible;
            labelName.Content = message;

            // Wait for duration seconds
            duration *= 1000;
            await Task.Delay(duration);

            // Clear label
            labelName.Visibility = Visibility.Hidden;
            labelName.Content = "";
        }
    }
}
