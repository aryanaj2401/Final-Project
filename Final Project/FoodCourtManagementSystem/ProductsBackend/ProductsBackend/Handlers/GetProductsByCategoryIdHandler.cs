using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.Models;

namespace ProductsBackend.Handlers
{
    public class GetProductsByCategoryQuery: IRequest <List<Product>>
    {   public int _id { get; set; }
        public GetProductsByCategoryQuery(int id)
        {
            _id = id;
        }
    }
    public class GetProductsByCategoryIdHandler  : IRequestHandler<GetProductsByCategoryQuery,List<Product>>
    {
        private readonly DataContext _db;
        public GetProductsByCategoryIdHandler(DataContext dataContext)
        {
            _db = dataContext;
        }

        public async Task<List<Product>> Handle (GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var categoryids = await _db.Categories.Select(x=> x.CategoryId).ToListAsync();
            if (categoryids.Contains(query._id))
            {


                var foodlist = await _db.Products.Where(x => x.CategoryId == query._id).ToListAsync();

                if (foodlist.Count > 0)
                {
                    return foodlist;
                }
                else
                    throw new Exception("No Products in this Category");
            }
            else
            {
                throw new Exception("No Such Category Exists");

            }
        }
    }
}
