namespace BackendUsers.DTO.OrderDTOs
{
    public class UpdatePasswordDTO
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
