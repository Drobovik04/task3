using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    internal class FairRandomGenerator
    {
        public byte[] GenerateKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var key = new byte[32];
                rng.GetBytes(key);
                return key;
            }
        }

        public int GenerateSecureRoll(int range)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                int result;
                do
                {
                    var bytes = new byte[4];
                    rng.GetBytes(bytes);
                    result = BitConverter.ToInt32(bytes, 0) % range;
                } while (result < 0 || result >= range);

                return result;
            }
        }

        public string GenerateHMAC(byte[] key, int message)
        {
            using (var hmac = new HMACSHA256(key))
            {
                byte[] messageBytes = BitConverter.GetBytes(message);
                var hash = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}
