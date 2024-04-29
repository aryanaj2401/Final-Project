using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.Models;

namespace ProductsBackend.Handlers
{
    public class GetAllCategoriesQuery : IRequest<List<Category>>
    {

    }
    public class GetAllCategoriesHandler :  IRequestHandler<GetAllCategoriesQuery, List<Category>>
    {
        private readonly DataContext _context;
    public GetAllCategoriesHandler(DataContext context)
    {
        _context = context;
    }
    public async Task<List<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        //  Console.WriteLine(await _db._bookCollection.Find(new BsonDocument()).ToListAsync());
        var categories = await _context.Categories.Where(x=> x.Deleted==false).ToListAsync();



        return categories;


    }
    }
}
