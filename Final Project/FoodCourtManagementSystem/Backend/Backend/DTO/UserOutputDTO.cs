namespace BackendUsers.DTO
{

    public class UserOutputDTOenum
    {
        public enum Gender
        {
            MALE,
            FEMALE,
            OTHER
        };
    }
    public class UserOutputDTO : UserOutputDTOenum
    {
        public int Id { get; set; }
        public string Email { get; set; }

      

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Pronoun { get; set; }

        public bool IsAdmin { get; set; }

        //  public string? ImageTitle { get; set; }

        public string ImageUrl { get; set; }
    }
}
