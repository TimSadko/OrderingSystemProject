using System.Security.Cryptography;
using System.Text;

namespace OrderingSystemProject.Other
{
    public class Hasher
    {
        public static byte[] GetHash(string input)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            }           
        }

        public static string GetHashString(string input)
        {
            StringBuilder sb = new StringBuilder();
            byte[] hash = GetHash(input);

            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }           

            return sb.ToString();
        }
    }
}
