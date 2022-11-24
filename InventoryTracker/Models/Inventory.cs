using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InventoryTracker.Models
{
    public enum Category
    {
        Pantry,
        Dairy,
        Supplies,
        Frozen,
        Fruits,
        Vegetables,
        Drinks,
        Bakery,
        Snacks,
        Other
    }

    public class Inventory
    {
        private List<string> suppliers = new List<string> { "ABC", "Costco", "Walmart", "Bulk Barn", "Other" };

        private List<Item> items; // Maybe readonly

        public Inventory()
        {
            items = new List<Item>();
        }

        public List<Item> Items
        {
            get { return items; }
            private set { items = value; }
        }

        public List<string> Suppliers
        {
            get { return suppliers; }
            private set { suppliers = value; }
        }

        public void AddItem(Item newItem) // Pass the new item into method
        {
            this.Items.Add(newItem);
        }

        public void RemoveItem(Item currentItem) // Pass the item you want to be remove into method
        {
            this.Items.Remove(currentItem);
        }

        public void UpdateItem(Item currentItem, Item newItem) // Pass fields to update item
        {
            int index = Items.IndexOf(currentItem);
            Items[index] = newItem;
        }

        public string GenerateReport()
        {
            StringBuilder output = new StringBuilder();
            //output.AppendLine("INVENTORY REPORT");
            //output.AppendLine();

            foreach (Item item in this.Items)
            {
                output.AppendLine(item.ToString());
            }
            return output.ToString();
        }

        public string ShoppingList()
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("The following items need to be purchased: ");
            int urgentDifference = 10;
            foreach (Item item in this.Items)
            {
                if (item.AvailableQuantity < item.MinimumQuantity) // If the item needs to be purchased
                {
                    if ((item.MinimumQuantity - item.AvailableQuantity) > urgentDifference) // Checks if lack of items is big enough to require urgent notice
                        output.Append("--URGENT-- ");
                    output.AppendLine(item.ToString());
                }
            }

            output.AppendLine();
            return output.ToString();
        }

        public bool SaveItems(string filename) // I made this a bool so that later on it can be used to provide an error if an issue occurs
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(filename))
                {
                    foreach (Item item in this.Items)
                    {                
                        streamWriter.WriteLine(item.CSVData);
                    }
                    streamWriter.Close();
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool LoadItems(string filename)
        {
            try
            {
                this.ClearItems();
                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        Item tempItem = new Item();
                        tempItem.CSVData = line;
                        this.AddItem(tempItem);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public void ClearItems()
        {
            this.Items.Clear();
        }
    }
}
