using Microsoft.Owin.Security;

namespace Server.Infrastructure.Authentication {

	internal class BasicAuthenticationOptions : AuthenticationOptions {

		public BasicAuthenticationOptions(string authenticationType) : base(authenticationType) {}

	}

}