using BackendUsers.DTO.OrderDTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BackendUsers.Validations
{

    public class UpdatePasswordValidator : AbstractValidator <UpdatePasswordDTO>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(o => o.Email)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull()
               .NotEmpty().WithMessage("Email Cannot be Empty");
            RuleFor(o => o.CurrentPassword).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("Password Cannot be Empty");
            RuleFor(o => o.NewPassword).Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().NotEmpty().WithMessage("Password Cannot be Empty");

            RuleFor(x => x.NewPassword)
           .Matches(new Regex(@"^(?=(.*[a-z]){3,})(?=(.*[A-Z]){2,})(?=(.*[0-9]){2,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{8,}$"))
           .WithMessage("Password must be Contains at least 3 lowercase letters." +
           "\r\nContains at least 2 uppercase letters." +
           "\r\nContains at least 2 digits.\r\nContains at least 1 special character from the set !@#$%^&*()\\-__+..\r\nHas a length between 8 and 24 characters.");

            //            ^ start anchor
            //(?= (.* [a - z]){ 3,})                lowercase letters. { 3,}
            //            indicates that you want 3 of this group
            //(?= (.* [A - Z]){ 2,})               uppercase letters. { 2,}
            //            indicates that you want 2 of this group
            //(?= (.* [0 - 9]){ 2,})               numbers. { 2,}
            //            indicates that you want 2 of this group
            //(?= (.* [!@#$%^&*()\-__+.]){1,})  all the special characters in the [] fields. The ones used by regex are escaped by using the \ or the character itself.
            //{1,} is redundant, but good practice, in case you change that to more than 1 in the future. Also keeps all the groups consistent
            //{ 8,}
            //            indicates that you want 8 or more
            //$                               end anchor

        }
    }
}
