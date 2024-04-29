namespace BackendUsers.DTO
{    
    public class UserInputDTOenum
    {
        public enum Gender
        {
            MALE,
            FEMALE,
            OTHER
        };
    }
    public class UserInputDTO : UserInputDTOenum
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Pronoun { get; set; } 

        public bool IsAdmin { get; set; }

        //  public string? ImageTitle { get; set; }

        public string ImageUrl { get; set; }
    }
}
