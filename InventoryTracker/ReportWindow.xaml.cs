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
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow(Inventory inventory)
        {
            InitializeComponent();
            
            // Programmatically creating a treeview of items sorted by category
            foreach (string catHead in Enum.GetNames(typeof(Category)))
            {
                TreeViewItem categoryParent = new TreeViewItem();
                categoryParent.Header = catHead;
                trView.Items.Add(categoryParent);
                foreach (Item item in inventory.Items)
                {
                    String category = item.ItemCategory.ToString();
                    if (category == catHead)
                    {
                        TreeViewItem itemChild = new TreeViewItem();
                        itemChild.Header = item.ToString();
                        categoryParent.Items.Add(itemChild);
                    } 
                }
            }

        }
    }
}
