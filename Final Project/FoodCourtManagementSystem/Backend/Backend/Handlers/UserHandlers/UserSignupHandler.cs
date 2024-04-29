using AutoMapper;
using BackendUsers.Data;
using BackendUsers.DTO;
using BackendUsers.Models;
using BackendUsers.Validations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BackendUsers.Handlers.UserHandlers
{



    public class UserInputCommand : IRequest <User>
    {
        public UserInputDTO _user;
       // public DataContext _db;
        public UserInputCommand(UserInputDTO user /*, DataContext data*/)
        {
            _user = user;
          //  _db = data;

        }

    }
    public class UserSignupHandler : IRequestHandler<UserInputCommand, User>
    {
        List<Exception> errors = new List<Exception>();
        string eSourceData;
        byte[] tmpSource;
        byte[] tmpHash; 
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        //sSourceData = "MySourceData";
        ////Create a byte array from source data.
        //tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
        public UserSignupHandler(DataContext db, IMapper mapper)
        {   _mapper = mapper;
            _db = db;
        }

        public async Task <User> Handle (UserInputCommand command, CancellationToken cancellationToken)
        {
            command._user.Email = command._user.Email.ToUpper();
            command._user.FullName = command._user.FullName.ToUpper();
            PostAUserValidator validator = new();
            var ValidationModel = _mapper.Map<User>(command._user);
            FluentValidation.Results.ValidationResult result = validator.Validate(ValidationModel);
            FluentValidation.Results.ValidationResult results = validator.Validate(ValidationModel);
            if (results.IsValid == false)
            {
                foreach (FluentValidation.Results.ValidationFailure failure in results.Errors)
                {
                    errors.Add(new Exception(failure.ErrorMessage));
                }
                throw new AggregateException(errors);
            }
            var users = await _db.Users.Where(x => x.Email == command._user.Email).FirstOrDefaultAsync();
            if(users != null)
            { var ex = new Exception("Email Already in Use");
                throw ex;
            }
            var user1 = _mapper.Map<User>( command._user );
          var result1=  await _db.Users.AddAsync( user1 );
            var x1 = user1;

            if(result1 != null)
           await _db.SaveChangesAsync();

            var x = new User
            {
                Email = user1.Email,
                FullName = user1.FullName,
                ImageUrl = user1.ImageUrl,
                Password = user1.Password,
                DateOfBirth = user1.DateOfBirth,
                Pronoun = user1.Pronoun,
                IsAdmin = user1.IsAdmin,
                Id = user1.Id

            };
            return x;
            //var model = new UserInputDTO()
            //{
            //    Email = command._user.Email,
            //    FullName = command._user.FullName,
            //    ImageUrl = command._user.ImageUrl,
            //    Password = command._user.Password,
            //    DateOfBirth = command._user.DateOfBirth,
            //    Pronoun = command._user.Pronoun,
            //    IsAdmin  = command._user.IsAdmin,



            //};
            //var user = _mapper.Map<User>(model);
            //await _db.Users.AddAsync(user);
            //var x = user;
            //await _db.SaveChangesAsync();
            //return user;


        }

    }
}
