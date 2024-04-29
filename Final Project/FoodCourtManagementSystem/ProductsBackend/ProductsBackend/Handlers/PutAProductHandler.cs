using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.DTO;
using ProductsBackend.Models;

namespace ProductsBackend.Handlers
{
    public class PutAProductCommand : IRequest<ProductWithCategoryName>
    {
        public Product _product;
        public PutAProductCommand(Product product)
        {
            _product = product;
        }
    }
    public class PutAProductHandler : IRequestHandler<PutAProductCommand, ProductWithCategoryName>
    {
        private readonly DataContext _db;
        public PutAProductHandler(DataContext db)
        {
            _db = db;

        }
        public async Task<ProductWithCategoryName> Handle(PutAProductCommand command , CancellationToken cancellationToken)
        {
            command._product.Name = command._product.Name.ToUpper();
            var prod = await _db.Products.Where(x => x.Id == command._product.Id).FirstOrDefaultAsync();
            if(prod == null)
            {
                throw new Exception("Product id "+ command._product.Id + "Not Found");

            }

            var NameExistsAlreadyOrNot = await _db.Products.Where(x => x.Name == command._product.Name && x.Id!=command._product.Id).FirstOrDefaultAsync();
            if (NameExistsAlreadyOrNot != null)
                throw new Exception("Product Name Already Exists");
            prod.Name = command._product.Name;
            prod.StartTime = command._product.StartTime;
            prod.EndTime = command._product.EndTime;
            prod.Description = command._product.Description;
            prod.Price = command._product.Price;
            prod.Quantity = command._product.Quantity;
            prod.ImageURl = command._product.ImageURl;
            prod.Energy = command._product.Energy;
            prod.Sugar  = command._product.Sugar;
            prod.Carbohydrate = command._product.Carbohydrate;
            prod.Fiber = command._product.Fiber;
            prod.Protein = command._product.Protein;
            prod.Fact = command._product.Fact;
            await _db.SaveChangesAsync();

            var p = await _db.Products.Where(x => x.Id == command._product.Id).FirstOrDefaultAsync();

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





        }

    }
}
