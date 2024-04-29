using System.ComponentModel.DataAnnotations;

namespace ProductsBackend.Models
{
    public class Category
    {
        
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string CategoryImage { get; set; }

        public bool Deleted { get; set; } = false;



    }
}
