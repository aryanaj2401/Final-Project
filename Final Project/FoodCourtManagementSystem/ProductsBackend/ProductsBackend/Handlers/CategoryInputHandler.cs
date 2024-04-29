using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.DTO;
using ProductsBackend.Models;
using ProductsBackend.Validations;

namespace ProductsBackend.Handlers
{

    public class CategoryInputCommand : IRequest<Category>
    {
        public CategoriesInputDTO _categoriesInputDTO;
        // public DataContext _db;
        public CategoryInputCommand(CategoriesInputDTO categoriesInputDTO)
        {
            _categoriesInputDTO = categoriesInputDTO;
            
        }

    }
    public class CategoryInputHandler: IRequestHandler<CategoryInputCommand, Category>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        List<Exception> errors = new List<Exception>();

        public CategoryInputHandler(DataContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task <Category> Handle (CategoryInputCommand command, CancellationToken cancellationToken)
        {
            command._categoriesInputDTO.CategoryName = command._categoriesInputDTO.CategoryName.ToUpper();
            PostACategoryValidator validator = new();
            var ValidationModel = command._categoriesInputDTO;
            FluentValidation.Results.ValidationResult resultval = validator.Validate(ValidationModel);
            FluentValidation.Results.ValidationResult resultsval = validator.Validate(ValidationModel);
            if (resultsval.IsValid == false)
            {
                foreach (FluentValidation.Results.ValidationFailure failure in resultsval.Errors)
                {
                    errors.Add(new Exception(failure.ErrorMessage));
                }
                throw new AggregateException(errors);
            }

            var categories = await _db.Categories.Where(x=> x.CategoryName == command._categoriesInputDTO.CategoryName).FirstOrDefaultAsync();
            if(categories.Deleted == true)
            {
                var ex = new Exception("Category Deleted");
                throw ex;

            }
            if (categories != null)
            {
                var ex = new Exception("Category Name Already in Use");
                throw ex;

            }
            var category1 = _mapper.Map<Category>(command._categoriesInputDTO);
            var result2 = await _db.Categories.AddAsync(category1);
            var x1 = category1;

            if (result2 != null)
                await _db.SaveChangesAsync();

            return category1;

        }
    }
}
