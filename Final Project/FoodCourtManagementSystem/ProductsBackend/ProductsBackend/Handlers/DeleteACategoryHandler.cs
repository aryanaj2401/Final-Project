using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;

namespace ProductsBackend.Handlers
{


    public class DeleteACategoryCommand : IRequest<Unit>
    {
   
        public int _id { get; set; }
        public DeleteACategoryCommand(int id)
        {
         _id = id;
         
            
        }

    }
    

    



    
        public class DeleteACategoryHandler : IRequestHandler<DeleteACategoryCommand,Unit>
        {
        private readonly DataContext _db;
        public DeleteACategoryHandler(DataContext db)
        {
            _db = db;
            
        }

        public async Task<Unit> Handle (DeleteACategoryCommand command,CancellationToken cancellationToken)
        {

            var x = await _db.Categories.Where(x => x.CategoryId == command._id).FirstOrDefaultAsync();
            if(x == null)
            {
                throw new Exception("Category does not exist");

            }

          var productstodelete = await  _db.Products.Where(x => x.CategoryId == command._id).ToListAsync();
          
          foreach(var producttodelete in productstodelete)
            {
                producttodelete.Deleted = true;
            }
            
            
                var u = x.Deleted = true;
                await _db.SaveChangesAsync();

            

            await _db.SaveChangesAsync();

            return Unit.Value;


        }




        }
}
