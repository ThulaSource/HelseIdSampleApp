using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HelseIdSampleApp.OpenIdConnect.Helpers
{
    public class JwtGenerator
    {
        public enum SigningMethod
        {
            None, X509SecurityKey, RsaSecurityKey, X509EnterpriseSecurityKey
        };

        public static List<string> ValidAudiences = new List<string> { "https://localhost:44366/connect/token", "https://helseid-sts.utvikling.nhn.no", "https://helseid-sts.test.nhn.no", "https://helseid-sts.utvikling.nhn.no" };
        private const double DefaultExpiryInHours = 10;

        /// <summary>
        /// Generates a new JWT
        /// </summary>
        /// <param name="clientId">The application (OAuth/OID) client id</param>
        /// <param name="tokenEndpoint">The endpoint url to request the access token</param>
        /// <param name="signingMethod">Indicate which method to use when signing the Jwt Token</param>
        /// <param name="securityKey">The <see cref="RsaSecurityKey"/></param>
        /// <param name="securityAlgorithm">The <see cref="SecurityAlgorithms"/></param>
        public static string Generate(string clientId, string tokenEndpoint, SigningMethod signingMethod, SecurityKey securityKey, string securityAlgorithm)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId can not be empty or null");
            }

            if (string.IsNullOrEmpty(tokenEndpoint))
            {
                throw new ArgumentException("The token endpoint address can not be empty or null");
            }

            if (securityKey == null)
            {
                throw new ArgumentException("The security key can not be null");
            }

            if (string.IsNullOrEmpty(securityAlgorithm))
            {
                throw new ArgumentException("The security algorithm can not be empty or null");
            }

            return GenerateJwt(clientId, tokenEndpoint, null, signingMethod, securityKey, securityAlgorithm);
        }

        /// <summary>
        /// Generates a new JWT
        /// </summary>
        /// <param name="clientId">The OAuth/OIDC client ID</param>
        /// <param name="audience">The Authorization Server (STS)</param>
        /// <param name="expiryDate">If value is null, the default expiry date is used (10 hrs)</param>
        /// <param name="signingMethod">Indicate which method to use when signing the Jwt Token</param>
        /// <param name="securityKey">The <see cref="RsaSecurityKey"/></param>
        /// <param name="securityAlgorithm">The <see cref="SecurityAlgorithms"/></param>
        /// <returns></returns>
        private static string GenerateJwt(string clientId, string audience, DateTime? expiryDate, SigningMethod signingMethod, SecurityKey securityKey, string securityAlgorithm)
        {
            var signingCredentials = new SigningCredentials(securityKey, securityAlgorithm);

            var jwt = CreateJwtSecurityToken(clientId, audience + "", expiryDate, signingCredentials);

            if (signingMethod == SigningMethod.X509EnterpriseSecurityKey)
            {
                UpdateJwtHeader(securityKey, jwt);
            }

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static void UpdateJwtHeader(SecurityKey key, JwtSecurityToken token)
        {
            if (key is X509SecurityKey)
            {
                var x509Key = key as X509SecurityKey;
                var thumbprint = Base64Url.Encode(x509Key.Certificate.GetCertHash());
                var x5c = GenerateX5c(x509Key.Certificate);
                var pubKey = x509Key.PublicKey as RSA;
                var parameters = pubKey.ExportParameters(false);
                var exponent = Base64Url.Encode(parameters.Exponent);
                var modulus = Base64Url.Encode(parameters.Modulus);

                token.Header.Add("x5c", x5c);
                token.Header.Add("kty", pubKey.SignatureAlgorithm);
                token.Header.Add("use", "sig");
                token.Header.Add("x5t", thumbprint);
                token.Header.Add("e", exponent);
                token.Header.Add("n", modulus);
            }

            if (key is RsaSecurityKey)
            {
                var rsaKey = key as RsaSecurityKey;
                var parameters = rsaKey.Rsa?.ExportParameters(false) ?? rsaKey.Parameters;
                var exponent = Base64Url.Encode(parameters.Exponent);
                var modulus = Base64Url.Encode(parameters.Modulus);

                token.Header.Add("kty", "RSA");
                token.Header.Add("use", "sig");
                token.Header.Add("e", exponent);
                token.Header.Add("n", modulus);
            }
        }

        private static List<string> GenerateX5c(X509Certificate2 certificate)
        {
            var x5c = new List<string>();
            var chain = GetCertificateChain(certificate);
            if (chain != null)
            {
                foreach (var cert in chain.ChainElements)
                {
                    var x509base64 = Convert.ToBase64String(cert.Certificate.RawData);
                    x5c.Add(x509base64);
                }
            }
            return x5c;
        }

        private static X509Chain GetCertificateChain(X509Certificate2 cert)
        {
            var certificateChain = X509Chain.Create();
            certificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            certificateChain.Build(cert);
            return certificateChain;
        }

        private static JwtSecurityToken CreateJwtSecurityToken(string clientId, string audience, DateTime? expiryDate, SigningCredentials signingCredentials)
        {
            var exp = new DateTimeOffset(expiryDate ?? DateTime.Now.AddHours(DefaultExpiryInHours));

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, exp.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            return new JwtSecurityToken(clientId, audience, claims, DateTime.Now, DateTime.Now.AddHours(10), signingCredentials);
        }

        public static SecurityToken ValidateToken(string token, string validIssuer, string validAudience, string organizationEnhId)
        {
            var publicKey = RSAKeyGenerator.GetPublicKeyAsXml(organizationEnhId);

            var test = RSA.Create();
            test.FromXmlString(publicKey);

            var securityKey = new RsaSecurityKey(test.ExportParameters(false));

            var handler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                IssuerSigningKey = securityKey,
                ValidAudience = validAudience,
                ValidIssuer = validIssuer
            };

            SecurityToken validatedToken;
            var claimsPrincipal = handler.ValidateToken(token, validationParams, out validatedToken);

            return validatedToken;
        }

        public static string GenerateApiToken(string key, string issuer, string audience)
        {
            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token = tokenHandler.CreateJwtSecurityToken(issuer: issuer, audience: audience, subject: null, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddMinutes(15), signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
