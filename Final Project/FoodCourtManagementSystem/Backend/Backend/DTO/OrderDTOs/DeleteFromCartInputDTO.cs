namespace BackendUsers.DTO.OrderDTOs
{
    public class DeleteFromCartInputDTO
    {
        public string Email {  get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }
    }
}
