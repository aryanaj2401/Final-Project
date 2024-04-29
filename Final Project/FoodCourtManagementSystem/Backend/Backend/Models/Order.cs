using System.ComponentModel.DataAnnotations;

namespace BackendUsers.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public List<OrderProduct> Products_of_this_order { get; set; }

     

        public string email { get; set; }

        public string Status_Of_Request { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime AcceptedTime { get; set; } = DateTime.Now;





    }
}
