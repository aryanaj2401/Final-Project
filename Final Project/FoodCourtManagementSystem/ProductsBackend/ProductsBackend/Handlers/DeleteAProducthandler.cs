using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;

namespace ProductsBackend.Handlers
{
    public class DeleteAProductQuery : IRequest<Unit> // Use Unit for deletion without return value
    {
        public int ProductId { get; set; }

        public DeleteAProductQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class DeleteAProductHandler : IRequestHandler<DeleteAProductQuery, Unit>
    {
        private readonly DataContext _db;

        public DeleteAProductHandler(DataContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteAProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _db.Products.Where(p => p.Id == request.ProductId)
                                             .FirstOrDefaultAsync();

            product.Deleted = true;

            
            await _db.SaveChangesAsync();

            return Unit.Value; // Indicate successful deletion
        }
    }
}
