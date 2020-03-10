using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CasaDeShowAPI.Model;
using CasaDeShowAPI.Model.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace CasaDeShowAPI.Token
{
    public static class TokenService
    {
        public static LoginResponse GenerateToken(Usuario user)
        {
            if(user.Role == null) user.Role = "User";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString =  tokenHandler.WriteToken(token);

            return new LoginResponse(user.Nome, user.Email, tokenString);
        }
    }
}