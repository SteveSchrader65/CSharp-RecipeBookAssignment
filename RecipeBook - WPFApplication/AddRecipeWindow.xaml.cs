using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Syncfusion.Windows.Tools.Controls;

namespace RecipeBook
{
    public partial class AddRecipeWindow : Window
    {
        readonly Recipe recInstance = new Recipe();
        readonly Product prodInstance = new Product();
        readonly Measure measInstance = new Measure();
        readonly Category catInstance = new Category();

        List<Recipe> recipeList;
        List<Product> prodList;
        List<Measure> measList;
        List<Category> catList;
        List<Ingredient> ingrList;
        List<IngredientDisplay> displayList;

        Recipe thisRecipe = new Recipe();

        int recipeNum = 0;
        int selectedProdNumber = 0;
        string selectedProdName = "";
        double selectedQuantity = 0;
        string selectedMeasure = "";
        Boolean isChecked = false;

        public Point Location { get; internal set; }

        public AddRecipeWindow()
        {
            InitializeComponent();
            LoadLists();
            NewRecipe();
        }

        private void NewRecipe()
        {
            LoadProducts();
            LoadComboBoxes();
            ingrList = new List<Ingredient>();

            // Show/Hide Controls
            btnSave.Visibility = Visibility.Hidden;
            btnClear.Visibility = Visibility.Hidden;
            btnClose.Visibility = Visibility.Visible;
            cmbCategory.Visibility = Visibility.Visible;
            txtCategory.Visibility = Visibility.Hidden;
            lstIngredients.Visibility = Visibility.Visible;
            lblQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
            lblMeasures.Visibility = Visibility.Hidden;
            cmbMeasures.Visibility = Visibility.Hidden;
            btnSaveIngredient.Visibility = Visibility.Hidden;
            dgvIngredients.Visibility = Visibility.Hidden;

            // Get recipe number for this recipe
            recipeList = recInstance.ReadTableFromDatabase<Recipe>();
            recipeList = (List<Recipe>)recipeList.OrderBy(r => r.ID).ToList();

            recipeNum = (from r in recipeList
                             select r.ID).Last();

            recipeNum++;
            lblPage.Content = "Page: " + recipeNum.ToString();

            // Set textboxes to editable
            txtRecipeName.IsReadOnly = false;
            txtCookTime.IsReadOnly = false;
            txtCookTemp.IsReadOnly = false;
            txtMethod.IsReadOnly = false;
        }

        private void LoadLists()
        {
            // Create product list
            prodList = prodInstance.ReadTableFromDatabase<Product>();

            // Create category list
            catList = catInstance.ReadTableFromDatabase<Category>();

            // Create measures list
            measList = measInstance.ReadTableFromDatabase<Measure>();
        }

        private void LoadProducts()
        {
            // Load ingredients listbox
            lstIngredients.Items.Clear();
            foreach (Product ingredient in prodList)
            {
                // Add product to ingredient listbox  
                lstIngredients.Items.Add(ingredient.Name);
            }
        }

        private void LoadComboBoxes()
        {
            // Load category combobox
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Select");
            foreach (Category category in catList)
            {
                cmbCategory.Items.Add(category.Name);
            }
            cmbCategory.SelectedIndex = 0;

            // Load measures combobox
            cmbMeasures.Items.Clear();
            foreach (Measure measure in measList)
            {
                cmbMeasures.Items.Add(measure.Unit);
            }
            cmbMeasures.SelectedIndex = -1;
        }

        private void DisplayNewRecipe()
        {
            // Reset textboxes to readonly
            txtRecipeName.IsReadOnly = true;
            txtCookTime.IsReadOnly = true;
            txtCookTemp.IsReadOnly = true;
            txtMethod.IsReadOnly = true;

            // Hide/Show controls
            btnSave.Visibility = Visibility.Hidden;
            btnClear.Visibility = Visibility.Hidden;
            btnClose.Visibility = Visibility.Visible;
            cmbCategory.Visibility = Visibility.Hidden;
            txtCategory.Visibility = Visibility.Visible;
            lstIngredients.Visibility = Visibility.Hidden;
            lblQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
            lblMeasures.Visibility = Visibility.Hidden;
            cmbMeasures.Visibility = Visibility.Hidden;
            btnSaveIngredient.Visibility = Visibility.Hidden;
            dgvIngredients.Visibility = Visibility.Visible;

            // Load recipe ingredient details into DataGrid
            displayList = new List<IngredientDisplay>();
            displayList = (from i in ingrList
                           join p in prodList on i.ProdID equals p.ID into table1
                           from p in table1.ToList()
                           join m in measList on i.Measure equals m.ID into table3
                           from m in table3.ToList()
                           select new IngredientDisplay { Ingredient = p.Name, Quantity = (float)i.Quantity, Measure = m.Unit }).ToList();

            dgvIngredients.ItemsSource = displayList;
        }

        // This method sets and unsets tick in the checkbox
        private void LstIngredients_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            selectedProdName = (string)e.Item;

            selectedProdNumber = (from p in prodList
                                  where p.Name.Equals(selectedProdName)
                                  select p.ID).FirstOrDefault();

            if (!e.Checked)
            {
                // Remove ingredient from ingrList
                ingrList.Remove(ingrList.Find(i => i.ProdID == selectedProdNumber));
                DelayMessage(lblAddRemove, 3, "Ingredient Removed");

                if (ingrList.Count != 0)
                {
                    btnClear.Visibility = Visibility.Visible;
                    ShowSaveButton();
                }
                else
                {
                    DelayMessage(lblAddRemove, 3, "No ingredients selected");
                }
            }
        }

        // This method displays/hides the ingredient controls
        private void LstIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChecked = false;
            selectedQuantity = 0;
            selectedMeasure = "";
            txtQuantity.Text = "";
            cmbMeasures.SelectedItem = -1;

            btnSaveIngredient.Visibility = Visibility.Hidden;

            if (lstIngredients.SelectedItems.Count != 0)
            {
                selectedProdName = lstIngredients.SelectedItem.ToString();

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
            }

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
                btnSaveIngredient.Visibility = Visibility.Hidden;

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
                ingrList.Add(new Ingredient { RecipeID = recipeNum, ProdID = selectedProdNumber, Quantity = Convert.ToDecimal(txtQuantity.Text), Measure = cmbMeasures.SelectedIndex + 1 });
            }
            else
            {
                // Update existing ingredient
                existingIngredient.RecipeID = recipeNum;
                existingIngredient.ProdID = selectedProdNumber;
                existingIngredient.Quantity = Convert.ToDecimal(txtQuantity.Text);
                existingIngredient.Measure = cmbMeasures.SelectedIndex + 1;
            }

            // Hide controls, ready for next ingredient
            lblQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Visibility = Visibility.Hidden;
            txtQuantity.Text = "";
            lblMeasures.Visibility = Visibility.Hidden;
            cmbMeasures.Visibility = Visibility.Hidden;
            cmbMeasures.SelectedIndex = -1;
            btnSaveIngredient.Visibility = Visibility.Hidden;
            ShowSaveButton();
            btnClear.Visibility = Visibility.Visible;
            DelayMessage(lblAddRemove, 3, "Ingredient Added");    
        }

        // Check to ensure Recipe name does not already exist in database
        private void TxtRecipeName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            List<Recipe> recipeList = recInstance.ReadTableFromDatabase<Recipe>();
            bool recipeExists = recipeList.AsEnumerable().Where(r => r.Name.Equals(txtRecipeName.Text)).Count() > 0;

            if (recipeExists)
            {
                DelayMessage(lblErrorMessage, 3, "That recipe name already exists ");
                txtRecipeName.Text = thisRecipe.Name;
            }

            if (txtRecipeName.Text == "")
            {
                txtRecipeName.Text = thisRecipe.Name;
            }
        }

        private void TxtRecipeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtRecipeName.Text != "")
            {
                btnClear.Visibility = Visibility.Visible;
            }

            ShowSaveButton();
        }

        // Check to ensure CookTime textbox entry is numeric
        private void TxtCookTime_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!int.TryParse(txtCookTime.Text, out int value))
            {
                DelayMessage(lblErrorMessage, 3, "Cook Time must be numeric");
                txtCookTime.Text = thisRecipe.Time.ToString();
            }
        }

        private void TxtCookTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCookTime.Text != "")
            {
                btnClear.Visibility = Visibility.Visible;
            }

            ShowSaveButton();
        }

        // Check to ensure CookTemp textbox entry is numeric
        private void TxtCookTemp_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!int.TryParse(txtCookTemp.Text, out int value))
            {
                DelayMessage(lblErrorMessage, 3, "Cook Temp must be numeric");
                txtCookTemp.Text = thisRecipe.Temp.ToString();
            }
        }

        private void TxtCookTemp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCookTemp.Text != "")
            {
                btnClear.Visibility = Visibility.Visible;
            }

            ShowSaveButton();
        }

        // Check to ensure method instructions have been entered
        private void TxtMethod_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtMethod.Text == "")
            {
                DelayMessage(lblErrorMessage, 3, "Method must contain instructions");
                txtMethod.Text = thisRecipe.Method;
            }
        }

        private void TxtMethod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMethod.Text != "")
            {
                btnClear.Visibility = Visibility.Visible;
            }

            ShowSaveButton();
        }

        // Change Category textbox value to category combobox selected item 
        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategory.SelectedIndex != -1)
            {
                btnClear.Visibility = Visibility.Visible;
            }

            // Display selected category to textbox
            if (cmbCategory.SelectedIndex > 0)
            {
                txtCategory.Text = cmbCategory.SelectedItem.ToString();
                ShowSaveButton();
            }
        }

        // If all requirements are met, set Save button to visible
        private void ShowSaveButton()
        {
            if (txtRecipeName.Text != "" && txtCookTime.Text != "" && txtCookTemp.Text != "" && txtCategory.Text != "" && lstIngredients.Items.Count != 0 && txtMethod.Text != "")
            {
                btnClear.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
            }
            else
            {
                btnSave.Visibility = Visibility.Hidden;
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtRecipeName.Text = "";
            txtCookTime.Text = "";
            txtCookTemp.Text = "";
            txtCategory.Text = "";
            txtMethod.Text = "";
            cmbCategory.SelectedIndex = 0;
            lstIngredients.SelectedItems.Clear();
            ingrList.Clear();
            lblAddRemove.Visibility = Visibility.Hidden;

            DelayMessage(lblSuccessMessage, 3, "Restarting ...");        
            NewRecipe();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Hide buttons
            btnClear.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Hidden;

            // Add new recipe to database
            Recipe newRecipe = new Recipe();
            newRecipe = new Recipe(recipeNum, txtRecipeName.Text, txtMethod.Text, Convert.ToInt32(txtCookTime.Text), Convert.ToInt32(txtCookTemp.Text), cmbCategory.SelectedIndex /* + 1*/, ingrList);
            bool isSuccess = recInstance.AddNewItemToDatabase<Recipe>(newRecipe);

            // Display Success/Fail message
            if (isSuccess)
            {
                DelayMessage(lblSuccessMessage, 3, "Recipe has been saved");
            }
            else
            {
                DelayMessage(lblErrorMessage, 3, "Recipe save has failed");
            }

            DisplayNewRecipe();
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
