using FluentValidation;
using ProductsBackend.DTO;
using ProductsBackend.Models;
using System.Text.RegularExpressions;

namespace ProductsBackend.Validations
{
    public class PostAProductValidator : AbstractValidator <ProductInputDTO>
    {
        public PostAProductValidator()
        {
            // Required Fields
            RuleFor(o => o.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("Product Name cannot be empty")
                .Length(2,50).WithMessage("Name Must be Between 2 and 50 characters")
                
                ;

            RuleFor(o => o.CategoryId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().GreaterThan(0).WithMessage("Please select a valid category");

            RuleFor(o => o.StartTime)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .LessThan(o => o.EndTime).WithMessage("Start time must be before end time");
            RuleFor(o => o.Name).Matches("^[a-zA-Z ]+$")
              .WithMessage("Product Name Must Have only Alphabets and spaces")
              ;


            RuleFor(o => o.Quantity)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(o => o.Price)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .InclusiveBetween(0, decimal.MaxValue).WithMessage("Price must be a valid positive value");

            



        }

    }
}
