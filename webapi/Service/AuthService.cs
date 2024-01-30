namespace webapi.Service;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Data;
using ViewModel.Model;
using Data.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService
{
    public const string USER_ID_CLAIM_ID = "useridclaimid";
    private const KeyDerivationPrf PRF = KeyDerivationPrf.HMACSHA256;
    private const int ITERATIONS = 10000;
    private const int SALT_SIZE = 16;
    private const int SUBKEY_SIZE = 32;

    public IConfiguration _configuration;
    private readonly WapContext _dbContext;

    public AuthService(IConfiguration config, WapContext context)
    {
        _configuration = config;
        _dbContext = context;
    }

    public async Task SignInAsync(UserVM userVM)
    {
        if (userVM.Password is null)
            throw new Exception();
        var newUser = await _dbContext.Users.AddAsync(new User()
        {
            Name = userVM.Name,
            Password = HashPassword(userVM.Password)
        });
        await _dbContext.SaveChangesAsync();
        if (newUser is null) 
            throw new Exception();
    }

    public static string HashPassword(string password)
    {
        var salt = new byte[SALT_SIZE];
        new Random(DateTime.Now.Millisecond).NextBytes(salt);
        var subkey = KeyDerivation.Pbkdf2(password, salt, PRF, ITERATIONS, SUBKEY_SIZE);

        var outputBytes = new byte[salt.Length + subkey.Length];
        Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, SALT_SIZE, subkey.Length);
        return Convert.ToBase64String(outputBytes);
    }

    public static bool VerifyHashedPassword(string hashedPassword, string? providedPassword)
    {
        if (providedPassword is null)
            return false;
        var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SALT_SIZE];
        Buffer.BlockCopy(decodedHashedPassword, 0, salt, 0, salt.Length);
        var expectedSubkey = new byte[SUBKEY_SIZE];
        Buffer.BlockCopy(decodedHashedPassword, salt.Length, expectedSubkey, 0, expectedSubkey.Length);

        var actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, PRF, ITERATIONS, SUBKEY_SIZE);
        return actualSubkey.SequenceEqual(expectedSubkey);
    }

    private User GetUser(UserVM userVM)
    {
        if (userVM.Name == "" || string.IsNullOrEmpty(userVM.Password))
            throw new Exception();
        var user = _dbContext.Users.First(x => x.Name == userVM.Name);
        return VerifyHashedPassword(user.Password, userVM.Password) ? user : throw new Exception();
    }

    private User GetUser(int userId)
    {
        return _dbContext.Users.First(x => x.Id == userId);
    }

    private Claim[] CreateClaims(User user)
    {
        return new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? throw new Exception()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(USER_ID_CLAIM_ID, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                    };
    }

    private string Token(User user)
    {
        var claims = CreateClaims(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception()));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(40),
            signingCredentials: signIn);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    internal string Token(UserVM userVM)
    {
        return Token(GetUser(userVM));
    }

    internal string Token(int userId)
    {
        return Token(GetUser(userId));
    }
}
