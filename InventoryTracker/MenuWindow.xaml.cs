using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InventoryTracker.Models;

namespace InventoryTracker
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public Item MyItem { get; private set; }
        private Item editItem;

        private const int DEFAULT_INDEX = 0;
        public MenuWindow()
        {
            InitializeComponent();

            foreach (string cat in Enum.GetNames(typeof(Category)))
            {
                ComboBoxItem cbCategoryItem = new ComboBoxItem();
                cbCategoryItem.Content = cat;
                cbCategory.Items.Add(cbCategoryItem);
            }

        }

        public MenuWindow(Item item)
        {
            InitializeComponent();
            
            editItem = item;
            this.DataContext = editItem;

            
            foreach (string cat in Enum.GetNames(typeof(Category)))
            {
                ComboBoxItem cbCategoryItem = new ComboBoxItem();
                cbCategoryItem.Content = cat;
                cbCategory.Items.Add(cbCategoryItem);
            }
        }

        private void button_click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyItem = new Item();
                // Location can be null;
                if (String.IsNullOrEmpty(txtname.Text) || String.IsNullOrEmpty(txtavailbleQ.Text) || String.IsNullOrEmpty(txtminimumQ.Text) || cbSupplier.SelectedIndex == DEFAULT_INDEX || cbCategory.SelectedIndex == DEFAULT_INDEX)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Missing Fields: ");
                    if (String.IsNullOrEmpty(txtname.Text))
                        errorMessage.AppendLine("   - Name");
                    if (String.IsNullOrEmpty(txtavailbleQ.Text))
                        errorMessage.AppendLine("   - Available Quantity");
                    if (String.IsNullOrEmpty(txtminimumQ.Text))
                        errorMessage.AppendLine("   - Minimum Quantity");
                    if (cbSupplier.SelectedIndex == DEFAULT_INDEX)
                        errorMessage.AppendLine("   - Supplier");
                    if (cbCategory.SelectedIndex == DEFAULT_INDEX)
                        errorMessage.AppendLine("   - Category");

                    errorMessage.AppendLine("Please enter the previous fields.");
                    throw new ArgumentException(errorMessage.ToString());
                }
                else
                {
                    MyItem.Name = txtname.Text;
                    MyItem.AvailableQuantity = Convert.ToInt32(txtavailbleQ.Text);
                    MyItem.MinimumQuantity = Convert.ToInt32(txtminimumQ.Text);
                  
                    if (String.IsNullOrEmpty(txtLocation.Text))
                        MyItem.Location = "None";
                    else
                        MyItem.Location = txtLocation.Text;
                   
                    MyItem.Supplier = cbSupplier.Text;
                    MyItem.ItemCategory = (Category)Enum.Parse(typeof(Category), cbCategory.Text);
                    this.Close();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
