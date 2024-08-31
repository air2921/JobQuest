namespace common;

public static class App
{
    public const string MAIN_DB = "Postgres";
    public const string ELASTIC_SEARCH = "Elasticsearch";

    public const string REDIS_SECTION = "Redis";
    public const string REDIS_CONNECTION = "Connection";
    public const string REDIS_NAME = "Name";

    public const string REDIS_PRIMARY = "Primary";
    public const string REDIS_SECONDARY = "Secondary";
    public const string REDIS_CHAT = "Chat";

    public const string EMAIL_SECTION = "Email";
    public const string EMAIL_PROVIDER = "Provider";
    public const string EMAIL = "Address";
    public const string EMAIL_PASSWORD = "Password";

    public const string JWT_SECTION = "JsonWebToken";
    public const string SECRET_KEY = "Key";
    public const string ISSUER = "Issuer";
    public const string AUDIENCE = "Audience";

    public const string S3_SECTION = "S3";
    public const string S3_KEY_ID = "KeyId";
    public const string S3_ACCESS_KEY = "AccessKey";
    public const string S3_BUCKET = "Bucket";
    public const string S3_URL = "Provider";
}
