using BackendUsers.Models;

namespace BackendUsers.DTO.OrderDTOs
{
    public class OrderSummaryOutputDTO
    {
        public int Id { get; set; }
        public string email { get; set; }

        public string Status_Of_Request { get; set; }

        

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime AcceptedTime { get; set; } = DateTime.Now;


        public List <OrderProduct> orderProducts { get; set; }


    }
}
