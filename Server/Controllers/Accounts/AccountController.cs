using System;
using System.Security.Authentication;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Server.Domain;

namespace Server.Controllers {

	[RoutePrefix("account")]
	public class AccountController : BaseController {

		private readonly IUserRepository _userRepository;

		public AccountController(IUserRepository userRepository) {
			this._userRepository = userRepository;
		}

		[Route("login")]
		[HttpPost]
		public string Login(CredentialsSpecification credentials) {
			User user = this._userRepository.FindByName(credentials.UserName);
			if (user == null) {
				throw new AuthenticationException("User does not exist"); // do not do this in a real application
			}
			if (user.PasswordMatches(credentials.Password)) {
				string json = JsonConvert.SerializeObject(user);
				string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
				return token;
			}
			throw new AuthenticationException("Bad password"); // do not do this in a real application
		}

	}

}