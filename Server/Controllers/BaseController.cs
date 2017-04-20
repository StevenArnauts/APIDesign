using System.Web.Http;
using Autofac.Integration.WebApi;

namespace Server.Controllers {

	[AutofacControllerConfiguration]
	public class BaseController : ApiController { }

}