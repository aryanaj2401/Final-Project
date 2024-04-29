using BackendUsers.Models;

namespace BackendUsers.DTO.OrderDTOs
{
    public class OrderOutputDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string email { get; set; }

        public List<productDetails> ProductIdandQuantity { get; set; }

        public string Status_Of_Request { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime AcceptedTime { get; set; } = DateTime.Now;

        public List<Product> products { get; set; }
    }
}
