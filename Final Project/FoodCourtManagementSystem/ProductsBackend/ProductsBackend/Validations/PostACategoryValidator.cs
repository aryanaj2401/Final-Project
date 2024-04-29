using FluentValidation;
using ProductsBackend.DTO;

namespace ProductsBackend.Validations
{
    public class PostACategoryValidator: AbstractValidator<CategoriesInputDTO>
    {

        public PostACategoryValidator()
        {
            RuleFor(o => o.CategoryName)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull().NotEmpty().WithMessage("Product Name cannot be empty")
              .Length(2, 50).WithMessage("Name Must be Between 2 and 50 characters")

              ;

            RuleFor(o => o.CategoryName).Matches("^[a-zA-Z ]+$")
                .WithMessage("Category Name Must Have only Alphabets and spaces")
                ;
        }
    }
}
