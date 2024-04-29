using BackendUsers.Data;
using BackendUsers.DTO;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Models;
using BackendUsers.Validations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendUsers.Handlers.OrderHandler
{
    public class UpdatePasswordCommand: IRequest<UserOutputDTO>
    {
        public UpdatePasswordDTO _updatepass {  get; set; }
        public UpdatePasswordCommand(UpdatePasswordDTO updatepass)
        {
            _updatepass = updatepass;
        }

        public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand, UserOutputDTO>
        {
            List<Exception> errors = new List<Exception>();
            private readonly DataContext _db;
            public UpdatePasswordHandler(DataContext db)
            {
                _db = db;
                
            }

            public async Task<UserOutputDTO> Handle (UpdatePasswordCommand command, CancellationToken cancellationToken)
            {

                var x = await _db.Users.Where(x=> x.Email == command._updatepass.Email).FirstOrDefaultAsync();

                UpdatePasswordValidator validator = new();
                var ValidationModel = command._updatepass;
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

                if (x==null )
                {
                    throw new Exception("Email not found");
                }
                if(x.Password != command._updatepass.CurrentPassword)
                {
                    throw new Exception("Incorrect password");
                }

                x.Password = command._updatepass.NewPassword;
                // put validation on this 


               var u1 = await  _db.SaveChangesAsync();

                return new UserOutputDTO
                {
                    Email = command._updatepass.Email,
                    FullName = x.FullName,
                    DateOfBirth = x.DateOfBirth,
                    IsAdmin = x.IsAdmin,
                    ImageUrl = x.ImageUrl,
                    Id = x.Id,


                };

            }
        }



    }
}
