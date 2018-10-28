using System;
using System.Text;
using System.Security.Cryptography;
using IdentityModel;

namespace HelseIdSampleApp.OpenIdConnect.Helpers
{
    public class CryptoHelper
    {
        public HashAlgorithm GetMatchingHashAlgorithm(string signatureAlgorithm)
        {
            var signingAlgorithmBits = int.Parse(signatureAlgorithm.Substring(signatureAlgorithm.Length - 3));

            switch (signingAlgorithmBits)
            {
                case 256:
                    {
                        return SHA256.Create();
                    }
                case 384:
                    {
                        return SHA384.Create();
                    }
                case 512:
                    {
                        return SHA512.Create();
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public bool ValidateHash(string data, string hashedData, string signatureAlgorithm)
        {
            var hashAlgorithm = GetMatchingHashAlgorithm(signatureAlgorithm);

            using (hashAlgorithm)
            {
                var hash = hashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(data));

                byte[] leftPart = new byte[hashAlgorithm.HashSize / 16];
                Array.Copy(hash, leftPart, hashAlgorithm.HashSize / 16);

                var leftPartB64 = Base64Url.Encode(leftPart);
                var match = leftPartB64.Equals(hashedData);

                return match;
            }
        }

        public string CreateState()
        {
            return CryptoRandom.CreateUniqueId(16);
        }

        public string CreateNonce()
        {
            return CryptoRandom.CreateUniqueId(16);
        }

        public Pkce CreatePkceData()
        {
            var pkce = new Pkce
            {
                CodeVerifier = CryptoRandom.CreateUniqueId()
            };

            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pkce.CodeVerifier));
                pkce.CodeChallenge = Base64Url.Encode(challengeBytes);
            }

            return pkce;
        }

        public class Pkce
        {
            public string CodeVerifier { get; set; }
            public string CodeChallenge { get; set; }
        }
    }
}
