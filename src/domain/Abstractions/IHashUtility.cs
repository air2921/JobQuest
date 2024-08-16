namespace domain.Abstractions;

public interface IHashUtility
{
    string Hash(string password);
    bool Verify(string input, string src);
}
