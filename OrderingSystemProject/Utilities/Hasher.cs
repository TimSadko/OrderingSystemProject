using System.Security.Cryptography;
using System.Text;

namespace OrderingSystemProject.Utilities;

public class Hasher
{
    // stores the salt value used for password hashing
    private static string saltString = "";
    
    // set the salt value (should be called during application startup)
    public static void SetSalt(string salt)
    {
        saltString = salt;
    }
    
    // get the hash bytes using PBKDF2
    public static byte[] GetHash(string input)
    {
        // convert the salt string to bytes
        byte[] salt = ASCIIEncoding.ASCII.GetBytes(saltString);
            
        // use PBKDF2 (Rfc2898DeriveBytes) with 50000 iterations and SHA256
        using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, 50000, HashAlgorithmName.SHA256))
        {
            // return 32 bytes hash
            return pbkdf2.GetBytes(32);
        }
    }
    
    // convert hash bytes to a hex string
    public static string GetHashString(string input)
    {
        // get the hash bytes
        byte[] hash = GetHash(input);
    
        // convert the bytes to a hex string
        StringBuilder hexStringBuilder = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            hexStringBuilder.Append(hash[i].ToString("X2")); // convert each byte to a 2-digit hex value
        }
        return hexStringBuilder.ToString();
    }
}