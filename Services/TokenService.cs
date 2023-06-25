using BookManagementSystem.Interfaces;
using BookManagementSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace BookManagementSystem.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            // Initializing the symmetric security key using the token key from the configuration.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            // Creating a list of claims for the token.
            var claims = new List<Claim>
            {
                // Adding a claim representing the NameId with the value of the user's UserName.
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // Creating signing credentials using the symmetric security key.
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Creating a token descriptor to define the properties of the JWT token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Setting the subject (claims identity) of the token.
                Subject = new ClaimsIdentity(claims),

                // Setting the expiration date of the token (7 days from the current date).
                Expires = DateTime.Now.AddDays(7),

                // Setting the signing credentials for the token.
                SigningCredentials = creds,
            };

            // Creating an instance of JwtSecurityTokenHandler to generate and write JWT tokens.
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generating the JWT token using the token descriptor.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Returning the string representation of the JWT token.
            return tokenHandler.WriteToken(token);
        }
    }
}
