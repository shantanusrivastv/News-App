using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pressford.News.Data;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;
using entity = Pressford.News.Entities;

namespace Pressford.News.Services.Services
{
	public class UserService : IUserService
	{
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;
		private readonly IRepository<entity.UserLogin> _repository;

		public UserService(IConfiguration config, IMapper mapper, IRepository<entity.UserLogin> repository)
		{
			_config = config;
			_mapper = mapper;
			_repository = repository;
		}

		public UserInfo Authenticate(Credentials credentials)
		{
			var userLogin = VerifyAndGetUserDetails(credentials);

			// return null if user not found
			if (userLogin == null)
				return null;

			userLogin.Token = GenerateToken(userLogin);
			UserInfo userInfo = _mapper.Map<UserInfo>(userLogin);

			return userInfo;
		}

		private entity.UserLogin VerifyAndGetUserDetails(Credentials credentials)
		{
			Expression<Func<entity.UserLogin, bool>> predicate = (x)
							 => x.Username == credentials.Username && x.Password == credentials.Password;

			return _repository.FindBy(predicate, x => x.User).SingleOrDefault();
		}

		private string GenerateToken(entity.UserLogin userLogin)
		{
			// authentication successful hence generating JWT token
			var tokenHandler = new JwtSecurityTokenHandler();
			var appSecret = _config.GetValue<string>("AppSettings:Secret");
			var key = Encoding.ASCII.GetBytes(appSecret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, $"{userLogin.User.FirstName} {userLogin.User.LastName}"),
					new Claim(ClaimTypes.Role, userLogin.Role.ToString()),
					new Claim(ClaimTypes.NameIdentifier, userLogin.Username)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}