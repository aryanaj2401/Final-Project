using BackendUsers.Data;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BackendUsers.Handlers.OrderHandler
{   public class PutNewStatusCommand : IRequest<OrderOutputDTO>
    {
       public string NewStatus { get; set; }   
       public int OrderId { get; set; }
        public PutNewStatusCommand(string newstatus, int orderId)
        {
            NewStatus = newstatus;

            OrderId = orderId;

        }
    }
    public class PutNewStatusHandler : IRequestHandler<PutNewStatusCommand,OrderOutputDTO>
    {
        private readonly DataContext _db;
        public PutNewStatusHandler(DataContext db)
        {
            _db = db;
            
        }

        public async Task<OrderOutputDTO> Handle(PutNewStatusCommand command,CancellationToken cancellationToken)
        {
            command.NewStatus = command.NewStatus.ToUpper();
            var check = await _db.Orders.Where(x => x.Id == command.OrderId).FirstOrDefaultAsync();

            if( check == null )
            {
                throw new Exception("Order Id "+ command.OrderId + " Not Found");
            }

            if (command.NewStatus == "REJECTED")
            {




                check.Status_Of_Request = command.NewStatus;

                await _db.SaveChangesAsync();

                var output = new OrderOutputDTO()
                {
                    Id = command.OrderId,
                    email = check.email,
                    //  ProductIdandQuantity = check.Products_of_this_order;
                    Status_Of_Request = check.Status_Of_Request,
                    CreatedTime = check.CreatedTime,
                    AcceptedTime = check.AcceptedTime,
                };
                return output;
            }
            else if (command.NewStatus == "APPROVED")
            {
                var products = check.Products_of_this_order.ToList();
                int[] productId = new int[products.Count];
                int[] quantityApproced = new int[products.Count];



                for (var i = 0; i < products.Count; i++)
                {
                    productId[i] = products[i].ProductId;
                    quantityApproced[i] = products[i].Quantity;

                }

                PutProductRequestDto body = new PutProductRequestDto()
                {
                    ProductId = productId,
                    QuantityApproced = quantityApproced,
                };

                HttpClient client = new HttpClient();

                JsonContent putBody = JsonContent.Create(body);

                var responseFromProductService = await client.PutAsync($"https://localhost:7199/api/Products", putBody);

                if (!responseFromProductService.IsSuccessStatusCode)
                {
                    string errorMessage = await responseFromProductService.Content.ReadAsStringAsync();
                    throw new Exception(errorMessage);

                    

                }


                using (var httpclient = new HttpClient())
                {


                    var response = await httpclient.GetAsync("https://localhost:7199/api/Products");
                    var responsestring = await response.Content.ReadAsStringAsync();
                    var productlist = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responsestring,
                        new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    var productarr = productlist.Select(e => e.Id).ToList();





                    var disabledproducts = productlist.Where(x => x.Deleted == true).Select(e => e.Id).ToList();

                    foreach (var productid in products)
                    {

                        if (disabledproducts.Contains(productid.ProductId))
                            throw new Exception($"Product Id {productid.ProductId} is Deleted");

                    }
                }
                    check.Status_Of_Request = command.NewStatus;

                await _db.SaveChangesAsync();

                var output = new OrderOutputDTO()
                {
                    Id = command.OrderId,
                    email = check.email,
                    //  ProductIdandQuantity = check.Products_of_this_order;
                    Status_Of_Request = check.Status_Of_Request,
                    CreatedTime = check.CreatedTime,
                    AcceptedTime = check.AcceptedTime,
                };
                return output;


            }
            else
                throw new Exception("Unrecognised Status Of Request");

        }
    }
}
