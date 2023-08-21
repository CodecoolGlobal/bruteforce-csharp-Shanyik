using System.Text;
using Codecool.BruteForce.Passwords.Model;

namespace Codecool.BruteForce.Passwords.Generator;

public class PasswordGenerator : IPasswordGenerator
{
    private static readonly Random Random = new();
    private readonly AsciiTableRange[] _characterSets;

    public PasswordGenerator(params AsciiTableRange[] characterSets)
    {
        _characterSets = characterSets;
    }

    public string Generate(int length)
    {
        if (_characterSets.Length == 0)
        {
            throw new InvalidOperationException("No character sets are defined.");
        }

        var password = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            AsciiTableRange randomCharacterSet = GetRandomCharacterSet();
            char randomChar = GetRandomCharacter(randomCharacterSet);
            password.Append(randomChar);
        }

        return password.ToString();
    }

    private AsciiTableRange GetRandomCharacterSet()
    {
        int randomIndex = Random.Next(0, _characterSets.Length);
        return _characterSets[randomIndex];
    }

    private static char GetRandomCharacter(AsciiTableRange characterSet)
    {
        int randomCharCode = Random.Next(characterSet.Start, characterSet.End + 1);
        return (char)randomCharCode;
    }
}
