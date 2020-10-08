using System.Collections.Generic;

namespace VendingMachine.Web.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Price { get; set; }
    }

    public class ProductList
    {
        public List<Product> Products { get; set; }
    }
}
