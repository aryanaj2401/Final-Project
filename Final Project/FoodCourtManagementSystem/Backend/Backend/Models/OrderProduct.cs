using System.ComponentModel.DataAnnotations;

namespace BackendUsers.Models
{
    public class OrderProduct
    {
        [Key] public int Id { get; set; }
        public int ProductId { get; set; } // productId
        public int OrderId { get; set; }
   //     public Order Order { get; set; }

       // public string ProductId { get; set; }
       

        public int Quantity { get; set; }
    }
}
