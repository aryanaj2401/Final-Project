using System.ComponentModel.DataAnnotations;

namespace BackendUsers.Models
{
    public class Userenum
    {
        public enum Gender
        {
            MALE,
            FEMALE,
            OTHER
        };
    }
    public class User : Userenum
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Pronoun { get; set; }

        public bool IsAdmin { get; set; }   

      //  public string? ImageTitle { get; set; }

        public string ImageUrl { get; set; } 

      //  public string 



    }
}
