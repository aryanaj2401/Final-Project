using AutoMapper;
using BackendUsers.Data;
using BackendUsers.DTO;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendUsers.Handlers.UserHandlers
{

    public class All_Users_Query : IRequest<List<UserOutputDTO>>
    {

    }
    public class All_Users_Handler : IRequestHandler<All_Users_Query, List<UserOutputDTO> >
    {
        private readonly DataContext _data_context;
        private readonly IMapper _mapper;
        public All_Users_Handler(DataContext data,IMapper mapper)
        {
            _data_context = data;
            _mapper = mapper;
        }

        public async Task <List <UserOutputDTO>> Handle (All_Users_Query request, CancellationToken cancellationToken)
        {
            var all_users = await _data_context.Users.ToListAsync();
           var all_users_output_without_password =  _mapper.Map<List<UserOutputDTO>>(all_users);
            return all_users_output_without_password ;
        }
    }
}
