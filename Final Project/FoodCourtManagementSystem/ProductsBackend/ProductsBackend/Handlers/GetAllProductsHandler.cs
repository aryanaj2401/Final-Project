using MediatR;
using ProductsBackend.Data;
using ProductsBackend.Models;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ProductsBackend.Handlers
{
    public class GetAllProductsQuery : IRequest<List<Product>>
        
    {
        
    }

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly DataContext _context;
        public GetAllProductsHandler(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            //  Console.WriteLine(await _db._bookCollection.Find(new BsonDocument()).ToListAsync());
            var products = await _context.Products.ToListAsync();


                
            return products;


        }
    }
}
