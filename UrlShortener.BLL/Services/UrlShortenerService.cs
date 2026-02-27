using UrlShortener.BLL.Interfaces;

namespace UrlShortener.BLL.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Base = 62;

    public string GenerateShortCode(int id)
    {
        var result = new Stack<char>();

        while (id > 0)
        {
            result.Push(Alphabet[id % Base]);
            id /= Base;
        }

        return new string(result.ToArray());
    }
}
