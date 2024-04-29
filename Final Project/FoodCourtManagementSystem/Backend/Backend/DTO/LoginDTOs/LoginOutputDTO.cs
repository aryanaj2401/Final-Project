namespace BackendUsers.DTO.LoginDTOs
{
    public class LoginOutputDTO
    {
        public int Id;
        public string token { get; set; }

        public string Email { get; set; }
        public bool isAdmin { get; set; } = false;
        public string FullName { get; set; }




    }
}
