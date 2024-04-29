using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.DTO;
using ProductsBackend.Models;
using ProductsBackend.Validations;

namespace ProductsBackend.Handlers
{

    public class ProductInputCommand : IRequest <ProductWithCategoryName>

    {
        public ProductInputDTO _product { get; set; }
      public ProductInputCommand(ProductInputDTO product)
        {
            _product = product;
            
        }
    }
    public class ProductInputHandler : IRequestHandler<ProductInputCommand , ProductWithCategoryName>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        List<Exception> errors = new List<Exception>();

        public ProductInputHandler(DataContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task <ProductWithCategoryName> Handle (ProductInputCommand command,CancellationToken cancellationToken)
        {
            command._product.Name = command._product.Name.ToUpper();
            PostAProductValidator validator = new();
            var ValidationModel = command._product;
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
            var Category_Exists_Or_Not = await _db.Categories.Where(x => x.CategoryId== command._product.CategoryId).FirstOrDefaultAsync();
            if(Category_Exists_Or_Not == null)
            {
                var ex = new Exception("This Category Does Not Exist");
                throw ex;
            }
            var NameExistsAlreadyOrNot = await _db.Products.Where(x => x.Name == command._product.Name).FirstOrDefaultAsync();
            if (NameExistsAlreadyOrNot != null)
                throw new Exception("Product Name Already Exists");
            var product = _mapper.Map<Product>(command._product);
            var result = await _db.Products.AddAsync(product);
            var x1 = product;
            if(result!=null)
            await _db.SaveChangesAsync();

            var p = await _db.Products.Where(x=> x.Id == product.Id).FirstOrDefaultAsync();

            var categories = await _db.Categories.ToListAsync();
            var categoryofproduct = categories.Where(x => x.CategoryId == p.CategoryId).FirstOrDefault();
            var products_plus_categories = new ProductWithCategoryName()
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                StartTime = p.StartTime,
                EndTime = p.EndTime,
                Description = p.Description,
                Carbohydrate = p.Carbohydrate,
                Energy = p.Energy,
                Quantity = p.Quantity,
                ImageURl = p.ImageURl,
                CategoryName = categoryofproduct.CategoryName,
                CategoryImage = categoryofproduct.CategoryImage,
                Price = p.Price,
                Sugar = p.Sugar,
                Fiber = p.Fiber,
                Fact = p.Fact,
                Protein = p.Protein,

            };
            return products_plus_categories;

            //var products_plus_categories = (from c in categories.AsQueryable()
            //                                join p in result.AsQueryable()
            //                                on c.CategoryId equals p.CategoryId
            //                                select new ProductWithCategoryName()
            //                                {
            //                                    Id = p.Id,
            //                                    Name = p.Name,
            //                                    CategoryId = p.CategoryId,
            //                                    StartTime = p.StartTime,
            //                                    EndTime = p.EndTime,
            //                                    Description = p.Description,
            //                                    Carbohydrate = p.Carbohydrate,
            //                                    Energy = p.Energy,
            //                                    Quantity = p.Quantity,
            //                                    ImageURl = p.ImageURl,
            //                                    CategoryName = c.CategoryName,
            //                                    CategoryImage = c.CategoryImage,
            //                                    Price = p.Price,
            //                                    Sugar = p.Sugar,
            //                                    Fiber = p.Fiber,
            //                                    Fact = p.Fact,
            //                                    Protein = p.Protein,
            //                                })
            //    .ToList();


            
        }

    }
}
