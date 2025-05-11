using System.Security.Cryptography;
using System.Text;

namespace OrderingSystemProject.Other
{
    public class Hasher
    {
        private static string _salt_string = "";

        public static void SetSalt(string salt) {  _salt_string = salt; }

        public static byte[] GetHash(string input)
        {
            byte[] salt = ASCIIEncoding.ASCII.GetBytes(_salt_string);

			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, 50000, HashAlgorithmName.SHA256);
			return pbkdf2.GetBytes(32);
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
