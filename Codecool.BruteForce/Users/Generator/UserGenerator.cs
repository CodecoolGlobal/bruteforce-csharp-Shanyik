using Codecool.BruteForce.Passwords.Generator;

namespace Codecool.BruteForce.Users.Generator;

public class UserGenerator : IUserGenerator
{
    private readonly List<IPasswordGenerator> _passwordGenerators;

    private int _userCount;
    private readonly Random _random = new Random();
    
    public UserGenerator(IEnumerable<IPasswordGenerator> passwordGenerators)
    {
        _passwordGenerators = passwordGenerators.ToList();
    }

    public IEnumerable<(string userName, string password)> Generate(int count, int maxPasswordLength)
    {
        var generatedUsers = new List<(string userName, string password)>();

        for (int i = 0; i < count; i++)
        {
            string userName = "user" + (++_userCount);
            IPasswordGenerator passwordGenerator = GetRandomPasswordGenerator();
            int passwordLength = GetRandomPasswordLength(maxPasswordLength);
            string password = passwordGenerator.Generate(passwordLength);
            generatedUsers.Add((userName, password));
        }

        return generatedUsers;
    }

    private IPasswordGenerator GetRandomPasswordGenerator()
    {
        int randomIndex = _random.Next(0, _passwordGenerators.Count);
        return _passwordGenerators[randomIndex];
    }

    private int GetRandomPasswordLength(int maxPasswordLength)
    {
        return _random.Next(1, maxPasswordLength + 1);
    }
}
