namespace BackendUsers.DTO.OrderDTOs
{
    public class productDetails
    {
        public int quantity { get; set; }
        public int productId { get; set; }
    }
    public class OrderInputDTO
    {


        public List<productDetails> ProductIds { get; set; }
        public string email { get; set; }
        public string Status_Of_Request { get; set; }

        //public DateTime CreatedTime { get; set; } = DateTime.Now;

        //public DateTime AcceptedTime { get; set; } = DateTime.Now;

    }
    
}
