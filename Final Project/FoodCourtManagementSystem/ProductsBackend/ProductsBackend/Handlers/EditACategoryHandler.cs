using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.Models;

namespace ProductsBackend.Handlers
{
    public class EditACategoryQuery : IRequest <Category>
    {
        public Category _category {  get; set; }
        public EditACategoryQuery(Category category)
        {
            _category = category;
        }
    }
    public class EditACategoryHandler : IRequestHandler<EditACategoryQuery,Category>
    {

        private readonly DataContext _db;
        public EditACategoryHandler(DataContext db)
        {
            _db = db;
            
        }

        public async Task <Category> Handle (EditACategoryQuery command, CancellationToken cancellationToken)
        {
            command._category.CategoryName = command._category.CategoryName.ToUpper();
            var categ = await _db.Categories.Where(x => x.CategoryId == command._category.CategoryId).FirstOrDefaultAsync();

            if (categ == null) {
                throw new Exception("Category Not Found");

            }

            categ.CategoryName = command._category.CategoryName;
            categ.CategoryImage = command._category.CategoryImage;

            await _db.SaveChangesAsync();

            return categ;

        }

    }
}
