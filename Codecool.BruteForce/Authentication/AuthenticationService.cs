using Codecool.BruteForce.Users.Model;
using Codecool.BruteForce.Users.Repository;

namespace Codecool.BruteForce.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool Authenticate(string userName, string password)
    {
        User user = _userRepository.GetAll().FirstOrDefault(u => u.UserName == userName);
        
        if (user != null)
        {
            return user.Password == password;
        }

        return false;
    }
}