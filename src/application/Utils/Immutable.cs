using System;

namespace application.Utils;

public static class Immutable
{
    #region Tokens Expires

    public static readonly TimeSpan JwtExpires = TimeSpan.FromMinutes(90);
    public static readonly TimeSpan RefreshExpires = TimeSpan.FromDays(90);

    #endregion

    #region Tokens keys in cookies

    /// <summary>
    /// This cookie name does not reflect its direct purpose. Such a non-obvious name may complicate possible attacks to steal authentication data
    /// </summary>

    public const string JWT_COOKIE_KEY = "session_preference";
    public const string REFRESH_COOKIE_KEY = "long_time_preference";
    public const string XSRF_COOKIE_KEY = ".AspNetCore.Xsrf";

    #endregion

    #region AuthCookie

    public const string USER_ID_COOKIE_KEY = "auth_user_id";
    public const string ROLE_COOKIE_KEY = "auth_role";
    public const string IS_AUTHORIZED = "auth_success";

    #endregion

    public const string XSRF_HEADER_NAME = "X-XSRF-TOKEN";
    public const string REFRESH_TOKEN_HEADER_NAME = "X-REFRESH";
    public const string NONE_BEARER = "X-BEARER";
}
