using AutoMapper;
using BookStore.API.ApplicationSettings;
using BookStore.API.DTOs;
using BookStore.API.Helpers;
using BookStore.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.API.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(UserForLoginDto userForLoginDto)
        {
            var authenticationResponse = new AuthenticationResponse();

            var user = await _userManager.FindByEmailAsync(userForLoginDto.Email);

            if (user == null)
            {
                authenticationResponse.IsAuthentication = false;
                authenticationResponse.Message = $"No Accounts Registered with this email";

                return authenticationResponse;
            }

            if (await _userManager.CheckPasswordAsync(user, userForLoginDto.Password))
            {
                authenticationResponse.IsAuthentication = true;

                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

                authenticationResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationResponse.Email = user.Email;
                authenticationResponse.UserName = user.UserName;

                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                authenticationResponse.Roles = roles.ToList();

                return authenticationResponse;
            }

            authenticationResponse.IsAuthentication = false;
            authenticationResponse.Message = $"Incorrect Credentials for user {user.Email}";

            return authenticationResponse;
        }

        public async Task<string> RegisterAsync(UserForRegisterDto userForRegisterDto)
        {
            var user = _mapper.Map<ApplicationUser>(userForRegisterDto);

            var userFromDb = await _userManager.FindByEmailAsync(userForRegisterDto.Email);

            if (userFromDb == null)
            {
                var result = await _userManager.CreateAsync(user, userForRegisterDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Authorization.DEFAULT_ROLE.ToString());
                }

                return $"User registered successfully";
            }
            else
            {
                return $"Email {user.Email} is already registered";
            }
        }

        #region Private Methods
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        #endregion
    }
}
