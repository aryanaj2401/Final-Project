using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.DTO;
using ProductsBackend.Models;
using System.Xml.Linq;

namespace ProductsBackend.Handlers
{
    public class GetProductsDetailsWithCategoriesQuery : IRequest<List<ProductWithCategoryName>>
    {

    }
    public class GetProductsDetailsWithCategoriesHandler : IRequestHandler<GetProductsDetailsWithCategoriesQuery, List<ProductWithCategoryName>>
    {
        private readonly DataContext _context;

        public GetProductsDetailsWithCategoriesHandler(DataContext context)
        {
            _context = context;
            
        }

        public async Task <List<ProductWithCategoryName>> Handle(GetProductsDetailsWithCategoriesQuery request, CancellationToken cancellationToken )
        {
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var products_plus_categories = (from c in categories.AsQueryable() 
                                            join p in products.AsQueryable()
                                            on c.CategoryId equals p.CategoryId select new ProductWithCategoryName()
                                            {   Id = p.Id,
                                                Name = p.Name,
                                                CategoryId = p.CategoryId,
                                                StartTime = p.StartTime,
                                                EndTime = p.EndTime,
                                                Description = p.Description,
                                                Carbohydrate = p.Carbohydrate,
                                                Energy = p.Energy,
                                                Quantity   = p.Quantity,
                                                ImageURl =  p.ImageURl,
                                                CategoryName =  c.CategoryName,
                                                CategoryImage = c.CategoryImage,
                                                Price = p.Price,
                                                Sugar   = p.Sugar,
                                                Fiber   = p.Fiber,
                                                Fact = p.Fact,
                                                Protein = p.Protein,
                                            })
                .ToList();

            return products_plus_categories;

        }
    }
}
