using System.Security.Cryptography;
using System.Text;

namespace Backend.Services;

public class BaseHadler
{
    private readonly string _passwd_salt = Environment.GetEnvironmentVariable("PASSWORD_SALT_KEY")
           ?? throw new Exception("PASSWORD_SALT_KEY not set.");
    public async Task<(string,string)> HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var combined = password + _passwd_salt;
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = sha.ComputeHash(bytes);

        // แปลงเป็น Base64 เพื่อเก็บใน DB
        return (Convert.ToBase64String(hash), _passwd_salt);
    }
}
