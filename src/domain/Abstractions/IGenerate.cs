namespace domain.Abstractions;

public interface IGenerate
{
    string GuidCombine(int count, bool useNoHyphensFormat = false);
    int GenerateCode(int length);
}
