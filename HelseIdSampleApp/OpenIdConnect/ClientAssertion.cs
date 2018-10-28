using HelseIdSampleApp.OpenIdConnect.Helpers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace HelseIdSampleApp.OpenIdConnect
{
    public class ClientAssertion
    {
        [JsonProperty("client_assertion")]
        public string client_assertion { get; set; }

        [JsonProperty("client_assertion_type")]
        public string client_assertion_type { get; set; } = IdentityModel.OidcConstants.ClientAssertionTypes.JwtBearer;

        public static ClientAssertion CreateWithRsaKeys(string clientId, string tokenEndpointUrl, string organizationEnhId)
        {
            var rsaKeyGenerator = new RSAKeyGenerator();
            var rsa = rsaKeyGenerator.GetRsaParameters(organizationEnhId);
            var securityKey = new RsaSecurityKey(rsa);
            
            var assertion = JwtGenerator.Generate(clientId, tokenEndpointUrl, JwtGenerator.SigningMethod.RsaSecurityKey, securityKey, SecurityAlgorithms.RsaSha512);

            return new ClientAssertion { client_assertion = assertion };
        }

        public static ClientAssertion CreateWithEnterpriseCertificate(string clientId, string tokenEndpointUrl, string thumbprint)
        {
            var certificate = GetCertificateByThumbprint(thumbprint);
            var securityKey = new X509SecurityKey(certificate);
            var assertion = JwtGenerator.Generate(clientId, tokenEndpointUrl, JwtGenerator.SigningMethod.X509EnterpriseSecurityKey, securityKey, SecurityAlgorithms.RsaSha512);

            return new ClientAssertion { client_assertion = assertion };
        }

        public static X509Certificate2 GetCertificateByThumbprint(string thumbprint, StoreName store = StoreName.My, StoreLocation location = StoreLocation.LocalMachine)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
            {
                throw new ArgumentOutOfRangeException(nameof(thumbprint));
            }

            using (var x509Store = new X509Store(store, location))
            {
                x509Store.Open(OpenFlags.ReadOnly);

                var certificatesInStore = x509Store.Certificates;
                var certificates = certificatesInStore.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count < 1)
                {
                    throw new ArgumentException($"Did not find any Certificates with the thumbprint: {thumbprint}");
                }

                if (certificates.Count > 1)
                {
                    throw new Exception($"Found {certificates.Count} certificates with thumbprint: {thumbprint}");
                }

                return certificates[0];
            }
        }
    }
}
