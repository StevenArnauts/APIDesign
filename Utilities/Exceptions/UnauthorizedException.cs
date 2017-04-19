namespace Utilities {

	/// <summary>
	/// Because the user hasn't logged in
	/// </summary>
	public class UnauthorizedException : BusinessException { }

	/// <summary>
	/// Because the user does not have the required permissions
	/// </summary>
	public class NotAllowedException : BusinessException { }

}
