using BackendUsers.Models;
using BackendUsers.Validations;
using FluentValidation.TestHelper;

namespace UsersBackendTesting
{
    public class UnitTest1
    {

        PostAUserValidator validator;

        public UnitTest1()
        {
            validator = new PostAUserValidator();
        }
        [Fact]
        public void Test_For_Empty_Inputs_In_Post_A_User()
        { //arrange
            var WrongUser = new User
            {

                





            };
            //act
            var result1 = validator.TestValidate(WrongUser);

            //assert

            result1.ShouldHaveValidationErrorFor(y => y.Email);
            result1.ShouldHaveValidationErrorFor(y => y.DateOfBirth);
            result1.ShouldHaveValidationErrorFor(y => y.FullName);
            result1.ShouldHaveValidationErrorFor(y => y.Password);
            result1.ShouldHaveValidationErrorFor(y => y.Pronoun);


        }
        [Fact]
        public void Test_For_Incorrect_Password_In_Post_A_User()
        { //arrange
            var wrongpassworduser = new User
            {
                Id = 1,
                Email = "example@email.com",
                Password = "securePassword123",
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 5, 15),
                Pronoun = 0,
                IsAdmin = false
            };
            //act
            var result1 = validator.TestValidate(wrongpassworduser);

            //assert

         
            result1.ShouldHaveValidationErrorFor(y => y.Password);
        


        }
        [Fact]
        public void Test_For_Incorrect_Email_In_Post_A_User()
        { //arrange
            var wrongpassworduser = new User
            {
                Id = 1,
                Email = "dsaccaomcemail.com",
                Password = "securePassword123",
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 5, 15),
                Pronoun = 0,
                IsAdmin = false
            };
            //act
            var result1 = validator.TestValidate(wrongpassworduser);

            //assert


            result1.ShouldHaveValidationErrorFor(y => y.Email);



        }


    }
}