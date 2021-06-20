using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using My_web_app.common;

namespace My_web_app.helper
{
    public class CustomBaseRepository
    {
        private DbContext _dbContext;
        public IConfiguration _configuration;

        public CustomBaseRepository(DbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }


        public string GenerateJwtToken(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["jwtConfig:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", userId),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtConfig:Secret"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetMessage(string key)
        {
            return _configuration["messages:" + key];
        }


        public string generateUUID()
        {
            return Guid.NewGuid().ToString();
        }


        public static ResponseHandler Ok(ResponseStatus status)
        {
            return new ResponseHandler {_responseStatus = status};
        }

        public object OK(ResponseStatus status, object data)
        {
            return new
            {
                status,
                data,
            };
        }

        public ResponseHandler Ok(ResponseStatus status, string msg)
        {
            return new ResponseHandler
                {_Msg = msg, _responseStatus = status};
        }


        public ResponseHandler Ok(string msg, ResponseStatus status, object data)
        {
            return new ResponseHandler {_responseStatus = status, _Msg = msg, _Data = data};
        }

        public ResponseHandler Ok(string msg)
        {
            return new ResponseHandler {_Msg = msg};
        }

        public ResponseHandler Ok(object data)
        {
            return new ResponseHandler {_Data = data};
        }
    }


    public class ResponseHandler
    {
        public ResponseStatus? _responseStatus { get; set; } = null;
        public string _Msg { get; set; } = null;
        public object _Data { get; set; } = null;
    }
}