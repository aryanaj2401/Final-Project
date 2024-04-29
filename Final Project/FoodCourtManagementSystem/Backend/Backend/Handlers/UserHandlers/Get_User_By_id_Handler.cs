using AutoMapper;
using BackendUsers.Data;
using BackendUsers.DTO;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackendUsers.Handlers.UserHandlers
{


    public class Get_User_By_id_Query : IRequest<UserOutputDTO>
    {
        public string _id;
        public Get_User_By_id_Query(string id)
        {
            _id = id;
        }
    }
    public class Get_User_By_id_Handler: IRequestHandler<Get_User_By_id_Query, UserOutputDTO> 
    {

        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public Get_User_By_id_Handler(DataContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<UserOutputDTO> Handle(Get_User_By_id_Query request, CancellationToken cancellationToken)
        {

            var user = await _db.Users.Where(x => x.Email == request._id.ToUpper()).FirstOrDefaultAsync();
            if(user == null)
            {
                var ex = new Exception("No Such Id Exists");
                throw ex;

            }
            var user_to_be_given = _mapper.Map<UserOutputDTO>(user);
            return user_to_be_given;

        }


        


    }




}
