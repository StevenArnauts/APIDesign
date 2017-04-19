using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Server.Controllers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json;
using Server.Domain;
using Claim = System.Security.Claims.Claim;

namespace Server.Infrastructure.Authentication {

	internal class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions> {

		protected override Task<AuthenticationTicket> AuthenticateCoreAsync() {
			AuthenticationTicket ticket = null;
			try {
				string header = this.Request.Headers.Get("Authorization");
				if (!string.IsNullOrEmpty(header)) {
					if (header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)) {
						string token = header.Substring("Basic ".Length).Trim();
						string json = Encoding.UTF8.GetString(Convert.FromBase64String(token));
						User user = JsonConvert.DeserializeObject<User>(json);
						Console.WriteLine("Authenticated user = " + user.Name);
						List<Claim> claims = new List<Claim> {
							new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name)
						};	
						ClaimsIdentity identity = new ClaimsIdentity(claims, this.Options.AuthenticationType);
						ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
						return (Task.FromResult(ticket));
					}
				}
			} catch (Exception ex) {
				Console.WriteLine("Basic authentication middleware failed: " + ex.Message);
			}
			return (Task.FromResult(ticket));
		}

	}

}
