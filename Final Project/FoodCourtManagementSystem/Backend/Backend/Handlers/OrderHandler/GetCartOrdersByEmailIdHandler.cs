using BackendUsers.Data;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace BackendUsers.Handlers.OrderHandler
{
    public class GetCartOrdersByEmailIdQuery : IRequest<List<Product>>
    {
        public string _email {  get; set; }
        public string _token { get; set; }
        public GetCartOrdersByEmailIdQuery(string email, string token)
        {
            _email = email;
            _token=token;
        }
    }
    public class GetCartOrdersByEmailIdHandler : IRequestHandler<GetCartOrdersByEmailIdQuery, List<Product>>

    {
        public DataContext _db;
        public GetCartOrdersByEmailIdHandler(DataContext db)
        {
            _db = db;
        }

        public async Task <List<Product>> Handle(GetCartOrdersByEmailIdQuery query, CancellationToken cancellationToken)
        {
            using (var httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",query._token.Split(" ")[1]);
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpclient.GetAsync("https://localhost:7199/api/Products");
                var responsestring = await response.Content.ReadAsStringAsync();
                var productlist = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responsestring,
                    new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                var orderarrcheckemail = await _db.Orders.Where(x => x.email == query._email).ToListAsync();


                if(orderarrcheckemail == null) {
                    throw new Exception("No  Orders with this email");
                }

                var ordersincart = orderarrcheckemail.Where(x => x.Status_Of_Request == "CART").FirstOrDefault();
                if(ordersincart == null)
                {
                    throw new Exception("Nothing In Cart");
                }

                var productarr = await _db.OrderProducts.ToListAsync();

                var productswithorderid = productarr.Where(x=> x.OrderId == ordersincart.Id).ToList();

                var result = (from o in productswithorderid.AsQueryable()
                              join p in productlist.AsQueryable()
                              on o.ProductId equals p.Id
                              select new Product
                              {   Id = p.Id,
                                  Name = p.Name,
                                  Quantity = o.Quantity,
                                  Price = p.Price,
                                  ImageURl = p.ImageURl,
                                  Carbohydrate = p.Carbohydrate,
                                  StartTime = p.StartTime,
                                  EndTime = p.EndTime,
                                  Sugar = p.Sugar,
                                  Fiber = p.Fiber,
                                  Energy = p.Energy,
                                  Fact = p.Fact,
                                  Description = p.Description,
                                  Protein = p.Protein,
                                  CategoryId = p.CategoryId,


                              }
                              ).ToList();

                return result;






            }
        }


    }
}
