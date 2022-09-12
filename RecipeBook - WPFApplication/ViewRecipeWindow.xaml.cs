using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Syncfusion.Windows.Tools.Controls;

namespace RecipeBook
{
    public partial class ViewRecipeWindow : Window
    {
        readonly Recipe recInstance = new Recipe();
        readonly Product prodInstance = new Product();
        readonly Measure measInstance = new Measure();
        readonly Category catInstance = new Category();
        readonly Favourite faveInstance = new Favourite();

        List<Product> prodList;
        List<Measure> measList;
        List<Category> catList;
        List<Ingredient> ingrList;
        List<IngredientDisplay> displayList;
        List<Favourite> faveList;

        Recipe thisRecipe = new Recipe();
        Category thisCategory = new Category();

        Boolean isChecked = false;
        Boolean isEditMode = false;
        Boolean loadingComplete = false;
        Boolean isRecipeNameChanged = false;
        Boolean isCookTimeChanged = false;
        Boolean isCookTempChanged = false;
        Boolean isMethodChanged = false;
        Boolean isIngredientsChanged = false;

        int currentRecipe;
        int selectedProdNumber = 0;
        string selectedProdName = "";
        double selectedQuantity = 0;
        string selectedMeasure = "";

        public Point Location { get; internal set; }

        public ViewRecipeWindow(int recipe)
        {
            InitializeComponent();
            currentRecipe = recipe;
            thisRecipe = recInstance.ReadItemFromDatabase<Recipe>(currentRecipe);

            LoadLists();
            LoadComboBoxes();
            SetFavouriteFlag(currentRecipe);
            DisplayRecipe(currentRecipe);
        }

        private void LoadLists()
        {
            // Create product list
            prodList = prodInstance.ReadTableFromDatabase<Product>();

            // Create category list
            catList = catInstance.ReadTableFromDatabase<Category>();

            // Create favourites list 
            faveList = faveInstance.ReadFavouritesList();

            // Create measures list
            measList = measInstance.ReadTableFromDatabase<Measure>();

            // Create ingredients list
            ingrList = thisRecipe.Ingredients;
        }

        private void LoadComboBoxes()
        {
            // Load category combobox
            foreach (Category category in catList)
            {
                cmbCategory.Items.Add(category.Name);
            }

            // Load measures combobox
            foreach (Measure measure in measList)
            {
                cmbMeasures.Items.Add(measure.Unit);
            }
            cmbMeasures.SelectedIndex = -1;
        }

        private void SetFavouriteFlag(int recipe)
        {
            // Check if current recipe is in favourites list
            Favourite thisFave = faveList.FirstOrDefault(f => f.RecipeNum == recipe);

            // Display Favourite flag for this recipe
            if (thisFave != null)
            {
                btnLike.Visibility = Visibility.Hidden;
                btnUnlike.Visibility = Visibility.Visible;
            }
            else
            {
                btnLike.Visibility = Visibility.Visible;
                btnUnlike.Visibility = Visibility.Hidden;
            }
        }

        private void DisplayRecipe(int recipe)
        {
            // Read current recipe
            thisRecipe = recInstance.ReadItemFromDatabase<Recipe>(recipe);
            thisCategory = catInstance.ReadItemFromDatabase<Category>(thisRecipe.Category);
            
            // Load recipe ingredients
            LoadIngredients();

            // Display recipe details
            HideEditControls();
            lblPage.Content = "Page: " + recipe.ToString();
            txtRecipeName.Text = thisRecipe.Name;
            txtCookTime.Text = thisRecipe.Time.ToString();
            txtCookTemp.Text = thisRecipe.Temp.ToString();
            txtCategory.Text = thisCategory.Name.ToString();
            txtMethod.Text = thisRecipe.Method;

            // Load recipe ingredient details into DataGrid
            displayList = new List<IngredientDisplay>();
            displayList = (from i in ingrList
                           join p in prodList on i.ProdID equals p.ID into table1
                           from p in table1.ToList()
                           join m in measList on i.Measure equals m.ID into table2
                           from m in table2.ToList()
                           select new IngredientDisplay { Ingredient = p.Name, Quantity = (float)i.Quantity, Measure = m.Unit }).ToList();

            dgvIngredients.ItemsSource = displayList;
            loadingComplete = true;
        }

        private void LoadIngredients()
        {
            List<string> unselectedList = new List<string>();

            // Initialize ingredients list
            isIngredientsChanged = false;
            ingrList.Clear();
            ingrList = thisRecipe.Ingredients;
            lstIngredients.Items.Clear();

            // Load ingredients listbox
            foreach (Product selected in prodList)
            {
                // if this product is contained within ingrList ...
                if (ingrList.Find(i => i.ProdID == selected.ID) != null)
                {
                    // Add product to ingredient listbox, ingredientName list and mark checkbox  
                    lstIngredients.Items.Add(selected.Name);
                    lstIngredients.SelectedItems.Add(selected.Name);
                }
                else
                {
                    unselectedList.Add(selected.Name);
                }
            }

            // Add unselected products to ingredient listbox
            foreach (string unselected in unselectedList)
            {
                lstIngredients.Items.Add(unselected);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            lstIngredients.SelectedItem = -1;
            ShowEditControls();

            // Set category combobox selected item
            cmbCategory.SelectedItem = txtCategory.Text;

            isEditMode = true;
        }

        private void HideEditControls()
        {
            // Show Recipe display DatGrid
            dgvIngredients.Visibility = Visibility.Visible;

            // Reset textboxes to readonly
            txtRecipeName.IsReadOnly = true;
            txtCookTime.IsReadOnly = true;
            txtCookTemp.IsReadOnly = true;
            txtMethod.IsReadOnly = true;

            // Hide/Show controls
            btnEdit.Visibility = Visibility.Visible;
            btnQuit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            btnClose.Visibility = Visibility.Visible;
            cmbCategory.Visibility = Visibility.Hidden;
            txtCategory.Visibility = Visibility.Visible;
            lstIngredients.Visibility = Visibility.Hidden;
            lblQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
            lblMeasures.Visibility = Visibility.Hidden;
            cmbMeasures.Visibility = Visibility.Hidden;
            btnSaveIngredient.Visibility = Visibility.Hidden;
        }

        private void ShowEditControls()
        {
            // Hide Recipe display DataGrid            
            dgvIngredients.Visibility = Visibility.Hidden;

            // Show ingredient listbox and textboxes
            lstIngredients.Visibility = Visibility.Visible;

            // Set textboxes to editable
            txtRecipeName.IsReadOnly = false;
            txtCookTime.IsReadOnly = false;
            txtCookTemp.IsReadOnly = false;
            txtMethod.IsReadOnly = false;

            // Change Category textbox to combo box
            txtCategory.Visibility = Visibility.Hidden;
            cmbCategory.Visibility = Visibility.Visible;

            // Hide/Show buttons
            btnEdit.Visibility = Visibility.Hidden;
            btnQuit.Visibility = Visibility.Visible;
            btnClose.Visibility = Visibility.Hidden;
        }

        private void BtnLike_Click(object sender, RoutedEventArgs e)
        {
            faveList.Add(new Favourite(currentRecipe));
            faveInstance.WriteFavouritesList(faveList);
            btnLike.Visibility = Visibility.Hidden;
            btnUnlike.Visibility = Visibility.Visible;
        }

        private void BtnUnlike_Click(object sender, RoutedEventArgs e)
        {
            Favourite thisFave = faveList.FirstOrDefault(f => f.RecipeNum == currentRecipe);
            faveList.Remove(thisFave);
            faveInstance.WriteFavouritesList(faveList);
            btnLike.Visibility = Visibility.Visible;
            btnUnlike.Visibility = Visibility.Hidden;
        }

        // This method sets and unsets tick in the checkbox
        private void LstIngredients_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // Prevents automatic updating of ingredients on set-up
            if (loadingComplete == false)
            {
                return;           
            }

            selectedQuantity = 0;
            selectedMeasure = "";
            txtQuantity.Text = "";
            cmbMeasures.SelectedItem = -1;

            selectedProdName = (string)e.Item;

            selectedProdNumber = (from p in prodList
                                  where p.Name.Equals(selectedProdName)
                                  select p.ID).FirstOrDefault();

            if (!e.Checked)
            {
                // Remove ingredient from ingrList
                ingrList.Remove(ingrList.Find(i => i.ProdID == selectedProdNumber));
                DelayMessage(lblAddRemove, 3, "Ingredient Removed");
                isIngredientsChanged = true;
                ShowSaveButton();

                if (ingrList.Count == 0)
                {
                    DelayMessage(lblAddRemove, 3, "No ingredients selected");
                    btnSave.Visibility = Visibility.Hidden;
                }
            }
        }

        // This method displays/hides the ingredient controls
        private void LstIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Exit method if item selected is newly-checked ingredient
            if (lstIngredients.SelectedItem is CheckListBoxItem)
            {
                return;
            }

            btnSaveIngredient.Visibility = Visibility.Hidden;

            selectedProdName = (string)lstIngredients.SelectedItem;

            selectedProdNumber = (from p in prodList
                                  where p.Name.Equals(selectedProdName)
                                  select p.ID).FirstOrDefault();

            selectedQuantity = (from i in ingrList
                                where i.ProdID.Equals(selectedProdNumber)
                                select (double)i.Quantity).FirstOrDefault();

            selectedMeasure = (from m in measList
                               join i in ingrList on m.ID equals i.Measure into table1
                               from i in table1.ToList()
                               where i.ProdID.Equals(selectedProdNumber)
                               select m.Unit).FirstOrDefault();

            foreach (string product in lstIngredients.SelectedItems)
            {
                if (product == selectedProdName)
                {
                    isChecked = true;
                    break;
                }
                else
                {
                    isChecked = false;
                }
            }

            if (isChecked == true)
            {
                cmbMeasures.SelectedItem = selectedMeasure;
                lblQuantity.Visibility = Visibility.Visible;
                txtQuantity.Visibility = Visibility.Visible;
                lblMeasures.Visibility = Visibility.Visible;
                cmbMeasures.Visibility = Visibility.Visible;

                if (selectedQuantity == 0)
                {
                    txtQuantity.Text = "";
                }
                else
                {
                    txtQuantity.Text = selectedQuantity.ToString();
                }
            }
            else
            {
                lblQuantity.Visibility = Visibility.Hidden;
                txtQuantity.Visibility = Visibility.Hidden;
                lblMeasures.Visibility = Visibility.Hidden;
                cmbMeasures.Visibility = Visibility.Hidden;
            }
        }

        private void TxtQuantity_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            // Check to ensure Quantity textbox entry is numeric
            if (!double.TryParse(txtQuantity.Text, out double value))
            {
                DelayMessage(lblAddRemove, 3, "Quantity must be numeric");
                
                if (selectedQuantity == 0)
                {
                    txtQuantity.Text = "";
                }
                else
                {
                    txtQuantity.Text = selectedQuantity.ToString();
                }

                return;
            }

            // Check to ensure Quantity textbox entry is not negative
            if (Convert.ToDouble(txtQuantity.Text) < 0)
            {
                DelayMessage(lblAddRemove, 3, "Quantity cannot be negative");

                if (selectedQuantity == 0)
                {
                    txtQuantity.Text = "";
                }
                else
                {
                    txtQuantity.Text = selectedQuantity.ToString();
                }

                return;
            }

            // Check to ensure Quantity textbox entry is not zero
            if (Convert.ToDouble(txtQuantity.Text) == 0)
            {
                DelayMessage(lblAddRemove, 3, "Quantity cannot be zero - Uncheck ingredient");

                if (selectedQuantity == 0)
                {
                    txtQuantity.Text = "";
                }
                else
                {
                    txtQuantity.Text = selectedQuantity.ToString();
                }

                return;
            }

            if (cmbMeasures.SelectedIndex != -1 && txtQuantity.Text != "")
            {
                btnSaveIngredient.Visibility = Visibility.Visible;
            }
        }

        private void CmbMeasures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMeasures.SelectedIndex != -1 && txtQuantity.Text != "")
            {
                btnSaveIngredient.Visibility = Visibility.Visible;
            }
        }

        private void BtnSaveIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient existingIngredient = ingrList.Find(i => i.ProdID == selectedProdNumber);

            if (existingIngredient == null)
            {
                // Add ingredient to ingrList 
                ingrList.Add(new Ingredient { RecipeID = currentRecipe, ProdID = selectedProdNumber, Quantity = Convert.ToDecimal(txtQuantity.Text), Measure = cmbMeasures.SelectedIndex + 1 });
            }
            else
            {
                // Update existing ingredient
                existingIngredient.RecipeID = currentRecipe;
                existingIngredient.ProdID = selectedProdNumber;
                existingIngredient.Quantity = Convert.ToDecimal(txtQuantity.Text);
                existingIngredient.Measure = cmbMeasures.SelectedIndex + 1;
            }

            // Hide controls, ready for next ingredient
            txtQuantity.Text = "";
            lblQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
            cmbMeasures.SelectedIndex = -1;
            lblMeasures.Visibility = Visibility.Hidden;
            cmbMeasures.Visibility = Visibility.Hidden;
            btnSaveIngredient.Visibility = Visibility.Hidden;
            isIngredientsChanged = true;
            DelayMessage(lblAddRemove, 3, "Ingredient Added");
            ShowSaveButton();
        }

        // Check to ensure Recipe name does not already exist in database
        private void TxtRecipeName_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (isEditMode)
            {
                if (isRecipeNameChanged)
                {
                    List<Recipe> recipeList = new List<Recipe>();
                    Boolean recipeExists = false;

                    recipeList = recInstance.ReadTableFromDatabase<Recipe>();
                    recipeExists = recipeList.AsEnumerable().Where(r => r.Name.Equals(txtRecipeName.Text)).Count() > 0;

                    if (recipeExists)
                    {
                        DelayMessage(lblErrorMessage, 3, "That recipe name already exists ");
                        txtRecipeName.Text = thisRecipe.Name;
                    }

                    if (txtRecipeName.Text == "")
                    {
                        txtRecipeName.Text = thisRecipe.Name;
                    }

                    isRecipeNameChanged = false;
                }
            }
        }

        private void TxtRecipeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isEditMode)
            {
                isRecipeNameChanged = true;
                ShowSaveButton();
            }
        }

        // Check to ensure CookTime textbox entry is numeric
        private void TxtCookTime_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (isEditMode)
            {
                if (isCookTimeChanged)
                {
                    if (!int.TryParse(txtCookTime.Text, out int value))
                    {
                        DelayMessage(lblErrorMessage, 3, "Cook Time must be numeric");
                        txtCookTime.Text = thisRecipe.Time.ToString();
                    }

                    isCookTimeChanged = false;
                }
            }
        }

        private void TxtCookTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isEditMode)
            {
                isCookTimeChanged = true;
                ShowSaveButton();
            }
        }

        // Check to ensure CookTemp textbox entry is numeric
        private void TxtCookTemp_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (isEditMode)
            {
                if (isCookTempChanged)
                {
                    if (!int.TryParse(txtCookTemp.Text, out int value))
                    {
                        DelayMessage(lblErrorMessage, 3, "Cook Temp must be numeric");
                        txtCookTemp.Text = thisRecipe.Temp.ToString();
                    }

                    isCookTempChanged = false;
                }
            }
        }
        private void TxtCookTemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isEditMode)
            {
                isCookTempChanged = true;
                ShowSaveButton();
            }
        }

        private void TxtMethod_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (isEditMode)
            {
                if (isMethodChanged)
                {
                    if (txtMethod.Text == "")
                    {
                        DelayMessage(lblErrorMessage, 3, "Method must contain instructions");
                        txtMethod.Text = thisRecipe.Method;
                    }

                    isMethodChanged = false;
                }
            }
        }

        private void TxtMethod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isEditMode)
            {
                isMethodChanged = true;
                ShowSaveButton();
            }
        }

        // Change Category textbox value to category combobox selected item 
        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Display selected category to textbox
            txtCategory.Text = cmbCategory.SelectedItem.ToString();
            ShowSaveButton();
        }

        // If all requirements are met, set Save button to visible
        private void ShowSaveButton()
        {
            // If no changes have been made, then exit this method
            if (txtRecipeName.Text == thisRecipe.Name && txtCookTime.Text == thisRecipe.Time.ToString() && txtCookTemp.Text == thisRecipe.Temp.ToString() && (cmbCategory.SelectedIndex + 1)  == thisRecipe.Category && isIngredientsChanged == false && txtMethod.Text == thisRecipe.Method)
            {
                btnSave.Visibility = Visibility.Hidden;
                return;
            }

            if (txtRecipeName.Text != "" && txtCookTime.Text != "" && txtCookTemp.Text != "" && txtCategory.Text != "" && lstIngredients.Items.Count != 0 && txtMethod.Text != "")
            {
                btnSave.Visibility = Visibility.Visible;
            }
            else
            {
                btnSave.Visibility = Visibility.Hidden;
            }
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            isEditMode = false;
            HideEditControls();
            DelayMessage(lblSuccessMessage, 3, "Editing abandoned");
            DisplayRecipe(currentRecipe);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            isEditMode = false;
            HideEditControls();

            // Change Category combobox to textbox
            txtCategory.Visibility = Visibility.Visible;
            cmbCategory.Visibility = Visibility.Hidden;

            // Update Recipe
            thisRecipe = new Recipe(currentRecipe, txtRecipeName.Text, txtMethod.Text, Convert.ToInt32(txtCookTime.Text), Convert.ToInt32(txtCookTemp.Text), cmbCategory.SelectedIndex + 1, ingrList);
            bool isSuccess = recInstance.UpdateItemInDatabase<Recipe>(thisRecipe);

            // Display Success/Fail message
            if (isSuccess)
            {
                DelayMessage(lblSuccessMessage, 3, "Recipe has been updated");
            }
            else
            {
                DelayMessage(lblErrorMessage, 3, "Recipe update has failed");
            }

            loadingComplete = false;
            btnSave.Visibility = Visibility.Hidden;
            DisplayRecipe(currentRecipe);
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

        // Return to MainWindow
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
