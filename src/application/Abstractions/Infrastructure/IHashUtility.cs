namespace application.Abstractions.Infrastructure
{
    public interface IHashUtility
    {
        string Hash(string password);
        bool Verify(string input, string src);
    }
}
