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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InventoryTracker.Models;
using Microsoft.Win32;
using System.IO;

namespace InventoryTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Inventory inventory = new Inventory();
        private string saveLocation = String.Empty;
        private bool saved = false;
        public MainWindow()
        {
            InitializeComponent();
            dgInventory.ItemsSource = inventory.Items; // Binding
            tbQuantity.Text = "Quantity: " + 0;
        }

        private void onClick_Add(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow(); // Opens new MenuWindow
            menuWindow.ShowDialog(); 
            if (menuWindow.MyItem != null)
            {
                inventory.AddItem(menuWindow.MyItem);
                dgInventory.Items.Refresh();
                saved = false;
            }

        }

        private void onClick_Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void onClick_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckBeforeOpening())
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.Filter = "CSV Files|*.csv";
                    if (open.ShowDialog() == true)
                    {
                        saveLocation = open.FileName;
                        saved = true;
                        inventory.ClearItems(); // Overwrites current inventory list
                        inventory.LoadItems(saveLocation);
                        dgInventory.Items.Refresh();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void Save()
        {
            if (String.IsNullOrEmpty(saveLocation))
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "CSV Files|*.csv";
                if (save.ShowDialog() == true)
                    saveLocation = save.FileName;
                else return;
            }
            inventory.SaveItems(saveLocation); // Calls inventory method to save items
            saved = true;
        }
        private bool CheckBeforeOpening()
        {
            if (inventory.Items.Count == 0)
                return true;
            if (saved)
                return true;
            MessageBoxResult result = MessageBox.Show("Would you like to save changes?", "Unsaved data", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return true;
            if (result == MessageBoxResult.Cancel)
                return false;
            if (result == MessageBoxResult.Yes)
                Save();
            return saved;
        }

        private bool CheckBeforeDeleting()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you would like to delete this item?", "Deleting data", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                return true;
            else
                return false;
        }

        private void onClick_Edit(object sender, RoutedEventArgs e)
        {

            Item editItem = dgInventory.SelectedItem as Item;
            if (editItem != null)
            {
                MenuWindow editWindow = new MenuWindow(editItem);
                editWindow.ShowDialog();
                if (editWindow.MyItem != null)
                {
                    inventory.UpdateItem(editItem, editWindow.MyItem);
                    dgInventory.Items.Refresh();
                    saved = false;
                }
            }
        }

        private void onClick_delete(object sender, RoutedEventArgs e)
        {
            Item selectedItem = (Item) dgInventory.SelectedItem; // Cast dgInventory item to Item
            if(selectedItem != null)
            {
                if (CheckBeforeDeleting() == true)
                    inventory.RemoveItem(selectedItem);
            }
            dgInventory.Items.Refresh();
            saved = false;
        }

        private void onClick_GenerateReport(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(inventory);
            reportWindow.ShowDialog();
        }

        private void onClick_ShoppingList(object sender, RoutedEventArgs e)
        {
            ShoppingListWindow shoppingListWindow = new ShoppingListWindow(inventory);
            shoppingListWindow.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !CheckBeforeOpening();
        }

        private void onClick_Decrease(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem != null)
            {
                Item editItem = dgInventory.SelectedItem as Item;
                int index = inventory.Items.IndexOf(editItem);
                inventory.Items[index].AvailableQuantity--;
                dgInventory.Items.Refresh();
                saved = false;
                tbQuantity.Text = "Quantity: " + inventory.Items[index].AvailableQuantity;
            }
        }

        private void onClick_Increase(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem != null)
            {
                Item editItem = dgInventory.SelectedItem as Item;
                int index = inventory.Items.IndexOf(editItem);
                inventory.Items[index].AvailableQuantity++;
                dgInventory.Items.Refresh();
                saved = false;
                tbQuantity.Text = "Quantity: " + inventory.Items[index].AvailableQuantity;
            }
        }

        private void dgInventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Item editItem = dgInventory.SelectedItem as Item;
            int index = inventory.Items.IndexOf(editItem);
            tbQuantity.Text = "Quantity: " + inventory.Items[index].AvailableQuantity;
        }

    }
}
