using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsBackend.Data;
using ProductsBackend.Models;

namespace ProductsBackend.Handlers
{
    public class ApproveAProductCommand : IRequest
    {
        public int[] ProductId { get; set; }
        public int[] QuantityApproved { get; set; }

        public ApproveAProductCommand(int[] productid, int[] quantityapproved)
        {
            ProductId = productid;
            QuantityApproved = quantityapproved;
        }
    }

    public class ApproveAProductHandler : IRequestHandler<ApproveAProductCommand>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public ApproveAProductHandler(DataContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task Handle(ApproveAProductCommand command, CancellationToken cancellationToken)
        {
            
            var product = await _db.Products.ToListAsync();
            var productids = product.Select(x=> x.Id).ToList();
            for(var i = 0;i<command.ProductId.Length; i++)
            { if (!productids.Contains(command.ProductId[i]))
                    throw new Exception("Product Id " + command.ProductId[i] + " Does not Exist");
              else
                {
                    var productinstance = product.Where(x => x.Id == command.ProductId[i]).FirstOrDefault();
                    if (productinstance.Quantity < command.QuantityApproved[i])
                    {
                        throw new Exception("Quantity of " + productinstance.Name + " is Only " + productinstance.Quantity +
                            " Whilst You request " + command.QuantityApproved[i]);
                    }
                    else
                    {
                        productinstance.Quantity = productinstance.Quantity - command.QuantityApproved[i];
                    }


                }
            
            }
            await _db.SaveChangesAsync();
  
        }
    }
}
