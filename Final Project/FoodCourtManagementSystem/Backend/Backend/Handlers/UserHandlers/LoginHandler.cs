using AutoMapper;
using BackendUsers.Data;
using BackendUsers.DTO;
using BackendUsers.DTO.LoginDTOs;
using BackendUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendUsers.Handlers.UserHandlers
{

    public class LoginCommand : IRequest<LoginOutputDTO>
    {
        public string username;
        public string password;

        public LoginCommand(string user, string pass)
        {
            username = user;
            password = pass;
        }


    }
    public class LoginHandler : IRequestHandler<LoginCommand, LoginOutputDTO>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        public LoginHandler(DataContext db, IMapper mapper, IConfiguration _configuration)
        {
            _mapper = mapper;
            _db = db;
           configuration = _configuration;


        }
        public async Task<LoginOutputDTO> Handle (LoginCommand command, CancellationToken cancellationtoken)
        {
            command.username = command.username.ToUpper();
            var user = await _db.Users.Where(x => x.Email == command.username).FirstOrDefaultAsync();
            if(user == null)
            {
                var ex = new Exception("Email Not Found");
                throw ex;

            }
            if(user.Password != command.password)
            {
                var ex = new Exception ("Password Does Not Match");
                throw ex;
            }
            #region tokengeneration


            //    var issuer = configuration["Jwt:Issuer"];
            //    var audience = configuration["Jwt:Audience"];
            //    var role = user.IsAdmin ? "Admin" : "User";
            //    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            //    var signingCredentials = new SigningCredentials(
            //                            new SymmetricSecurityKey(key),
            //                            SecurityAlgorithms.HmacSha512Signature
            //                        );
            //    var subject = new ClaimsIdentity(new[]
            //    {
            //            new Claim(JwtRegisteredClaimNames.Email,command.username),
            //       //new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            //        new Claim(ClaimTypes.Role, role)
            //        }
            //);
            //    var expires = DateTime.UtcNow.AddMinutes(100);
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = subject,
            //        Expires = DateTime.UtcNow.AddMinutes(100),
            //        Issuer = issuer,
            //        Audience = audience,
            //        SigningCredentials = signingCredentials
            //    };


            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    var jwtToken = tokenHandler.WriteToken(token);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.IsAdmin?"admin":"user")
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken =  tokenHandler.WriteToken(token);

            #endregion
            return new LoginOutputDTO
            {  token = jwtToken,
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                isAdmin = user.IsAdmin,

            };
        }

    }
}
