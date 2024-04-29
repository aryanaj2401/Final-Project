//using BackendUsers.Data;
//using BackendUsers.DTO.OrderDTOs;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace BackendUsers.Handlers.OrderHandler
//{
//    public class DeleteFromCartQuery : IRequest
//    {
//        public DeleteFromCartInputDTO deleteFromCartDTO { get; set; }

//        public DeleteFromCartQuery(DeleteFromCartInputDTO data)
//        {
//            deleteFromCartDTO = data;
            
//        }
//    }
//    public class DeleteFromCartHandler : IRequestHandler<DeleteFromCartQuery, DeleteFromCartInputDTO>
//    {
//        private readonly DataContext _db;
//        public DeleteFromCartHandler(DataContext db)
//        {
//            _db = db;
            
//        }
//        public async Task <Unit> Handle (DeleteFromCartQuery command , CancellationToken cancellationToken)
//        {
//            var x = await _db.Orders.Where(x => x.Id == command.deleteFromCartDTO.OrderId).FirstOrDefaultAsync();

//            if(x==null)
//                throw new Exception("Orderid "+ command.deleteFromCartDTO.OrderId+" Not found");

//           var u =  x.Products_of_this_order.ToList();
            


//        }
//    }
//}
