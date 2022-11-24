using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryTracker.Models;

namespace InventoryTracker.Models
{
    public class Item
    {
        private string name;
        private int availableQuantity;
        private int minimumQuantity;
        private string location;
        private string supplier;
        private Category itemCategory;

        private const int INDEX_WITH_LOCATION = 6;
        private const int INDEX_WITHOUT_LOCATION = 5;
        public Item()
        {
        }

        public Item(string name, int availableQuantity, int minimumQuantity, string location, string supplier, Category category)
        {
            this.Name = name;
            this.AvailableQuantity = availableQuantity;
            this.MinimumQuantity = minimumQuantity;
            this.Location = location;
            this.Supplier = supplier;
            this.ItemCategory = category;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int AvailableQuantity
        {
            get { return availableQuantity; }
            set
            {
                if (value >= 0)
                    availableQuantity = value;
                else
                    throw new ArgumentException("Cannot have a negative availible quantity");
            }
        }

        public int MinimumQuantity
        {
            get { return minimumQuantity; }
            set
            {
                if (value >= 1)
                    minimumQuantity = value;
                else
                    throw new ArgumentException("The minimum quantity cannot be less than 1");
            }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Supplier
        {
            get { return supplier; }
            set { supplier = value; }
        }

        public Category ItemCategory
        {
            get { return itemCategory; }
            set { itemCategory = value; }
        }
        // Property used to convert from csv to item and item to csv
        public string CSVData
        {
            get
            {
                return String.Format($"{Name}, {AvailableQuantity}, {MinimumQuantity}, {Location}, {Supplier}, {ItemCategory}");
            }
            set
            {
                string[] fileData = value.Split(',');
                try
                {
                    // Checks if CSV has location is provided or not
                    if (fileData.Length == INDEX_WITH_LOCATION)
                    {
                        this.Name = fileData[0];
                        this.AvailableQuantity = int.Parse(fileData[1]);
                        this.MinimumQuantity = int.Parse(fileData[2]);
                        this.Location = fileData[3];
                        this.Supplier = fileData[4];
                        Category loadedCategory;
                        if (!Enum.TryParse(fileData[5], out loadedCategory))
                        {
                            throw new ArgumentException("Category read from file is not valid.");
                        }
                        this.ItemCategory = loadedCategory;
                    }
                    else if (fileData.Length == INDEX_WITHOUT_LOCATION)
                    {
                        this.Name = fileData[0];
                        this.AvailableQuantity = int.Parse(fileData[1]);
                        this.MinimumQuantity = int.Parse(fileData[2]);
                        this.Location = "None";
                        this.Supplier = fileData[3];
                        Category loadedCategory;
                        if (!Enum.TryParse(fileData[4], out loadedCategory))
                        {
                            throw new ArgumentException("Category read from file is not valid.");
                        }
                        this.ItemCategory = loadedCategory;
                    }
                    else
                    {
                        throw new ArgumentException("Not enough arguments in CSV file.");
                    }      
                }
                catch
                {
                    throw;
                }
            }
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(this.Location))
                return String.Format("Name: {0}, Available Quantity: {1}, Minimum Quantity: {2}, Supplier: {3}, Category: {4}", this.Name, this.AvailableQuantity, this.MinimumQuantity, this.Supplier, this.ItemCategory);
            else
                return String.Format("Name: {0}, Available Quantity: {1}, Minimum Quantity: {2}, Location: {3}, Supplier: {4}, Category: {5}", this.Name, this.AvailableQuantity, this.MinimumQuantity, this.Location, this.Supplier, this.ItemCategory);
        }
    }
}
