using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecipeBook
{
    public partial class DeleteRecipeWindow : Window
    {
        public Point Location { get; internal set; }
        private int recipeNum;

        readonly Recipe recInstance = new Recipe();

        // Display Delete Recipe UI
        public DeleteRecipeWindow(int num)
        {
            InitializeComponent();
            recipeNum = num;
            Recipe recipeToDelete = recInstance.ReadItemFromDatabase<Recipe>(recipeNum);
            lblRecipeName.Content = "pg" + recipeNum + "  " + recipeToDelete.Name;
        }

        private void BtnConfirmDelete_Click(object sender, RoutedEventArgs e)
        {
            // Call interface method to delete record
            bool isSuccess = recInstance.DeleteItemFromDatabase<Recipe>(recipeNum);

            // Hide label and buttons
            lblConfirm.Visibility = Visibility.Hidden;
            btnConfirmDelete.Visibility = Visibility.Hidden;
            btnDoNotDelete.Visibility = Visibility.Hidden;

            // Display "Recipe deleted message"
            if (isSuccess)
            {
                DelayMessage(lblSuccessMessage, 3, "Recipe deleted");
            }
            else
            {
                DelayMessage(lblErrorMessage, 3, "Recipe delete failed");
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

        private void BtnDoNotDelete_Click(object sender, RoutedEventArgs e)
        {
            // Return to MainWindow
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Return to MainWindow
            Close();
        }
    }
}
