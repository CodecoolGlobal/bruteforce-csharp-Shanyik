namespace Codecool.BruteForce.Passwords.Breaker;

public class PasswordBreaker : IPasswordBreaker
{
    
    private readonly char[] _asciiCharacters = Enumerable.Range(48, 122).Select(i => (char)i).ToArray();
    public IEnumerable<string> GetCombinations(int passwordLength)
    {
        
        IEnumerable<IEnumerable<string>> stringLists = Enumerable.Repeat(_asciiCharacters.Select(c => c.ToString()), passwordLength);
        IEnumerable<string> combinations = GetAllPossibleCombos(stringLists);

        return combinations;
    }

    private static IEnumerable<string> GetAllPossibleCombos(IEnumerable<IEnumerable<string>> strings)
    {
        IEnumerable<string> combos = new[] { "" };

        combos = strings
            .Aggregate(combos, (current, inner) => current.SelectMany(c => inner, (c, i) => c + i));

        return combos;
    }
}
