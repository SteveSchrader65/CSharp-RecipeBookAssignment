using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace RecipeBook
{
    public partial class AddProductWindow : Window
    {
        public Point Location { get; internal set; }
        readonly Product prodInstance = new Product();

        List<Product> prodList;

        public AddProductWindow()
        {
            InitializeComponent();
            lstProducts.SelectedIndex = -1;
            LoadProductList();
        }

        // Load list of available ingredient products from database
        private void LoadProductList()
        {
            lstProducts.Items.Clear();
            prodList = prodInstance.ReadTableFromDatabase<Product>();

            // Sort list of products alphabetically
            prodList = prodList.OrderBy(p => p.Name).ToList();

            // Add products to listbox
            foreach (Product ingredient in prodList)
            {
                lstProducts.Items.Add(ingredient.Name);
            }
        }

        // Add newly entered product to product list
        private void BtnAddToList_Click(object sender, RoutedEventArgs e)
        {
            // If textbox entry is not empty ...
            if (txtNewIngredient.Text != "")
            {
                bool ingredientExists = prodList.AsEnumerable().Where(p => p.Name.Equals(txtNewIngredient.Text)).Count() > 0;
  
                // Check if textbox entry already exists in database and set message content to appropriate message    
                if (ingredientExists)
                {
                    DelayMessage(lblErrorMessage, 3, "That ingredient exists already");
                }

                // ... otherwise add new product to database
                else
                {
                    Product newProduct = new Product(txtNewIngredient.Text);
                    bool isSuccess = prodInstance.AddNewItemToDatabase<Product>(newProduct);

                    if (isSuccess)
                    {
                        DelayMessage(lblSuccessMessage, 3, "New Ingredient Added");
                    }
                    else
                    {
                        DelayMessage(lblErrorMessage, 3, "Ingredient save failed");
                    }
                }
            }
            else
            {
                DelayMessage(lblErrorMessage, 3, "No ingredient entered");
            }

            txtNewIngredient.Text = "";
            LoadProductList();
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Return to MainWindow
            Close();
        }
    }
}
