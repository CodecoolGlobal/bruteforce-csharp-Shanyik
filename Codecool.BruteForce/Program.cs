using System.Diagnostics;
using Codecool.BruteForce.Authentication;
using Codecool.BruteForce.Passwords.Breaker;
using Codecool.BruteForce.Passwords.Generator;
using Codecool.BruteForce.Passwords.Model;
using Codecool.BruteForce.Users.Generator;
using Codecool.BruteForce.Users.Repository;

namespace Codecool.BruteForce;

internal static class Program
{
    private static readonly AsciiTableRange LowercaseChars = new(97, 122);
    private static readonly AsciiTableRange UppercaseChars = new(65, 90);
    private static readonly AsciiTableRange Numbers = new(48, 57);

    public static void Main(string[] args)
    {
        string workDir = AppDomain.CurrentDomain.BaseDirectory;
        var dbFile = $"{workDir}\\Resources\\Users.db";

        IUserRepository userRepository = new UserRepository(dbFile);
        userRepository.DeleteAll();

        var passwordGenerators = CreatePasswordGenerators();
        IUserGenerator userGenerator = new UserGenerator(passwordGenerators);
        int userCount = 10;
        int maxPwLength = 4;

        AddUsersToDb(userCount, maxPwLength, userGenerator, userRepository);

        Console.WriteLine($"Database initialized with {userCount} users; maximum password length: {maxPwLength}");

        IAuthenticationService authenticationService = new AuthenticationService(userRepository);
        BreakUsers(userCount, maxPwLength, authenticationService);

        Console.WriteLine($"Press any key to exit.");

        Console.ReadKey();
    }

    private static void AddUsersToDb(int count, int maxPwLength, IUserGenerator userGenerator,
        IUserRepository userRepository)
    {
        var users = userGenerator.Generate(count, maxPwLength);

        foreach (var (userName, password) in users)
        {
            userRepository.Add(userName, password);
            Console.WriteLine($"Added user: {userName} with password: {password}");
        }
    }

    private static IEnumerable<IPasswordGenerator> CreatePasswordGenerators()
    {
        var lowercasePwGen = new PasswordGenerator(LowercaseChars);
        var uppercasePwGen = new PasswordGenerator(LowercaseChars, UppercaseChars);
        var numbersPwGen = new PasswordGenerator(LowercaseChars, UppercaseChars, Numbers);

        return new List<IPasswordGenerator>
        {
            lowercasePwGen, uppercasePwGen, numbersPwGen
        };
    }

    private static void BreakUsers(int userCount, int maxPwLength, IAuthenticationService authenticationService)
    {
        var passwordBreaker = new PasswordBreaker();
        Console.WriteLine("Initiating password breaker...\n");

        for (int i = 1; i <= userCount; i++)
        {
            var user = $"user{i}";
            for (int j = 1; j <= maxPwLength; j++)
            {
                Console.WriteLine($"Trying to break {user} with all possible password combinations with length = {j}... ");

                var pwCombinations = passwordBreaker.GetCombinations(j);
                bool broken = false;
                
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                foreach (var pw in pwCombinations)
                {
                    if (authenticationService.Authenticate(user, pw))
                    {
                        stopwatch.Stop();
                        Console.WriteLine($"Password for {user} cracked: {pw}");
                        Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalMilliseconds} ms");
                        broken = true;
                        break;
                    }
                }

                if (broken)
                {
                    break;
                }
            }
        }
    }
}
