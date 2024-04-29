using BackendUsers.Models;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System;
using System.Text.RegularExpressions;
using System;
using System.Security.Cryptography;
using System.Text;


namespace BackendUsers.Validations


{
    public class PostAUserValidator : AbstractValidator <User>
    {

        public PostAUserValidator()
        {    // Required Fields
            RuleFor(o => o.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty().WithMessage("Email Cannot be Empty");
            RuleFor(o=> o.Password).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("Password Cannot be Empty");
            RuleFor(o => o.DateOfBirth).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("Date Of Birth Cannot be Empty");
            RuleFor(o => o.FullName).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("Full Name Cannot be Empty");
            //RuleFor(o => o.Pronoun).Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotNull().NotEmpty().WithMessage("Pronouns Cannot be Empty");

            //Common Rules for  Passwords
           // RuleFor(x => x.Password).Length(8, 24).WithMessage("Password must be Minimum 8 Characters and Max 24 Characters.");

            RuleFor(x => x.Password)
            .Matches(new Regex(@"^(?=(.*[a-z]){3,})(?=(.*[A-Z]){2,})(?=(.*[0-9]){2,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{8,}$"))
            .WithMessage("Password must Contain at least 3 lowercase letters." +
            "\r\nContains at least 2 uppercase letters." +
            "\r\nContains at least 2 digits.\r\nContains at least 1 special character from the set !@#$%^&*()\\-__+..\r\nHas a length between 8 and 24 characters.");

            //            ^ start anchor
            //(?= (.* [a - z]){ 3,})                lowercase letters. { 3,}
            //            indicates that you want 3 of this group
            //(?= (.* [A - Z]){ 2,})               uppercase letters. { 2,}
            //            indicates that you want 2 of this group
            //(?= (.* [0 - 9]){ 2,})               numbers. { 2,}
            //            indicates that you want 2 of this group
            //(?= (.* [!@#$%^&*()\-__+.]){1,})  all the special characters in the [] fields. The ones used by regex are escaped by using the \ or the character itself. {1,} is redundant, but good practice, in case you change that to more than 1 in the future. Also keeps all the groups consistent
            //{ 8,}
            //            indicates that you want 8 or more
            //$                               end anchor


            RuleFor(x => x.Email)
           .Matches(new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
           .WithMessage("Enter  A Valid Email");
                        //            [a-z0 - 9!#$%&'*+/=?^_{|}~-]+`:
                        //This part matches one or more characters from the set of lowercase letters(a - z), digits(0 - 9), and special characters(!#$%&'*+/=?^_{|}~-`).
                        //It allows for a mix of alphanumeric characters and specific special characters.
                        //(?:\.[a - z0 - 9!#$%&'*+/=?^_{|}~-]+)*`:
                        //The(?: ... ) syntax defines a non - capturing group.
                        //Inside this group:
                        //  \. matches a literal period(dot).
                        //[a - z0 - 9!#$%&'*+/=?^_{|}~-]+` matches one or more characters from the same set as in step 1.
                        //The * outside the group allows this entire group to repeat zero or more times.
                        //This part allows for domain subcomponents separated by periods(e.g., example.com, sub.example.co.uk).
                        //@(?: a - z0 - 9 ?\.) +a - z0 - 9 ?:
                        //                    This part matches the domain part of an email address.
                        //It consists of:
                        //@: The literal at symbol.
                        //(?: ... )+: A capturing group that repeats one or more times.
                        //Inside this group:
                        //            [a-z0 - 9]: Matches a single lowercase letter or digit.
                        //            (?:[a - z0 - 9 -] * [a - z0 - 9]) ?: An optional non-capturing group.
                        //            [a - z0 - 9 -] *: Matches zero or more lowercase letters, digits, or hyphens.
                        //            [a - z0 - 9]: Matches a single lowercase letter or digit.
                        //\.: Matches a literal period(dot).
                        //a - z0 - 9 ?: Similar to the previous group, but without the repetition.
                        //This ensures that the last part of the domain has at least one character before the dot.




        }


    }
}
