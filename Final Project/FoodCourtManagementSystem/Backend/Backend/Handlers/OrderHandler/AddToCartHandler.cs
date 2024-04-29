using AutoMapper;
using BackendUsers.Data;
using BackendUsers.DTO.OrderDTOs;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http.Headers;

namespace BackendUsers.Handlers.OrderHandler
{

    public class AddToCartCommand : IRequest<OrderOutputDTO>
    {
        public OrderInputDTO OrderInputDTO { get; set; }
        public string _token {  get; set; }
        public AddToCartCommand(OrderInputDTO orderInput,string token)
        {
            OrderInputDTO = orderInput;
            _token = token;

        }
    }
    public class AddToCartHandler : IRequestHandler<AddToCartCommand, OrderOutputDTO>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public AddToCartHandler(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }

        public async Task<OrderOutputDTO> Handle(AddToCartCommand command, CancellationToken cancellationToken)
        {
            if (command.OrderInputDTO.ProductIds[0].quantity<1)

            {
                throw new Exception("Can't have less than 1 in cart");
            }
            #region gettingtheproductsfromproductsmicroserviceandcheckingifproductsexistornot
            using (var httpclient = new HttpClient())
            {

                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", command._token.Split(" ")[1]);
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                command.OrderInputDTO.email = command.OrderInputDTO.email.ToUpper();
                command.OrderInputDTO.Status_Of_Request = command.OrderInputDTO.Status_Of_Request.ToUpper();
                var response = await httpclient.GetAsync("https://localhost:7199/api/Products");
                var responsestring = await response.Content.ReadAsStringAsync();
                var productlist = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(responsestring,
                    new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var productarr = productlist.Where(x=> x.Deleted==false).Select(e => e.Id).ToList();

                foreach (var productid in command.OrderInputDTO.ProductIds)
                {
                    if (!productarr.Contains(productid.productId))
                        throw new Exception($"Product Id {productid} Does Not Exist");
                }

                #endregion

                #region checkIfThereAlreadyIsACartOrNot
                var ordersalreadyfromtheuser = await _db.Orders.Where(x => x.email == command.OrderInputDTO.email).ToListAsync();
                var orderidsoftheuser = ordersalreadyfromtheuser.Select(x => x.Id).ToList();
                var cartemptyornot = ordersalreadyfromtheuser.Where(x => x.Status_Of_Request == "CART").FirstOrDefault();
                #endregion
                if (cartemptyornot == null) // if cart is empty then normal order is created

                {






                    var order = new Order
                    {
                        email = command.OrderInputDTO.email,
                        Status_Of_Request = command.OrderInputDTO.Status_Of_Request,
                        CreatedTime = DateTime.Now,
                        AcceptedTime = DateTime.Now
                    };
                    _db.Orders.Add(order);
                    await _db.SaveChangesAsync();

                    foreach (var productId in command.OrderInputDTO.ProductIds)
                    {
                        var orderProduct = new OrderProduct
                        {
                            OrderId = order.Id,

                            ProductId = productId.productId,
                            Quantity = productId.quantity
                        };

                        _db.OrderProducts.Add(orderProduct);

                    }

                    await _db.SaveChangesAsync();

                    var output = new OrderOutputDTO()
                    {
                        Id = order.Id,
                        email = order.email,
                        ProductIdandQuantity = command.OrderInputDTO.ProductIds,
                        Status_Of_Request = order.Status_Of_Request,
                        CreatedTime = DateTime.Now,
                        AcceptedTime = DateTime.Now


                    };
                    return output;
                }

                else // if cart is not empty there are 2 cases
                {

                    var productsAlreadyInTheCartOfTheUser = await _db.OrderProducts.Where(x=> x.OrderId == cartemptyornot.Id).ToListAsync();
                    // Case 1 if the product i have added now does not exist in the cart i should add
                    //just the product to the orderproduct

                    foreach (var productId in command.OrderInputDTO.ProductIds)
                    { if (productsAlreadyInTheCartOfTheUser.Select(x => x.ProductId).Contains(productId.productId))
                        {
                            var x = await _db.OrderProducts.FirstOrDefaultAsync(x=> x.OrderId == cartemptyornot.Id && x.ProductId == productId.productId);
                            x.Quantity = productId.quantity +
                                           productsAlreadyInTheCartOfTheUser.Where(x => x.ProductId == productId.productId).
                                           Select(x => x.Quantity).FirstOrDefault();

                            //var orderProduct = new OrderProduct
                            //{
                            //    OrderId = cartemptyornot.Id,

                            //    ProductId = productId.productId,
                            //    Quantity = productId.quantity + 
                            //               productsAlreadyInTheCartOfTheUser.Where(x => x.ProductId == productId.productId).Select(x => x.Quantity).FirstOrDefault()
                            //};

                            

                           

                        }
                    else
                        {
                            var orderProduct = new OrderProduct
                            {
                                OrderId = cartemptyornot.Id,

                                ProductId = productId.productId,
                                Quantity = productId.quantity
                            };

                            _db.OrderProducts.Add(orderProduct);

                        }

                    }

                    await _db.SaveChangesAsync();
                    var updatedproducts = await _db.OrderProducts.Where(x => x.OrderId == cartemptyornot.Id).ToListAsync();
                      List< productDetails > updatedProductIdandQuantity = new();
                    foreach (var product in updatedproducts)
                    {
                        productDetails pro = new productDetails()
                        {
                            productId = product.ProductId,
                            quantity = product.Quantity
                        };
                        updatedProductIdandQuantity.Add(pro);
                    }
                    var output = new OrderOutputDTO()
                    {
                        Id = cartemptyornot.Id,
                        email = command.OrderInputDTO.email,
                        ProductIdandQuantity = updatedProductIdandQuantity,
                        Status_Of_Request = command.OrderInputDTO.Status_Of_Request ,
                        CreatedTime = DateTime.Now,
                        AcceptedTime = DateTime.Now


                    };
                    return output;
                }
            }


        }
    }
}
