using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace HelseIdSampleApp.OpenIdConnect.Helpers
{
    /// <summary>
    /// For identificators used to set JWS algorithm see: http://self-issued.info/docs/draft-ietf-jose-json-web-algorithms.html#rfc.appendix.A.1
    /// </summary>
    public class RSAKeyGenerator
    {
        private const string KeyNamePrefix = "HelseID_DCR_";
        public const int Size = 4096;
        public const string JwsAlgorithmName = Microsoft.IdentityModel.Tokens.SecurityAlgorithms.RsaSha512;

        /// <summary>
        /// Creates a new RSA key pair, and returns the key as Xml formatted string
        /// If a key allready exists it will be deleted
        /// </summary>
        /// <param name="includePrivateParameters">If true the private parameters will be included in the xml formatted key</param>
        /// <param name="organizationEnhId">The organization ENH-Id</param>
        /// <returns></returns>
        public static string CreateNewKey(bool includePrivateParameters, string organizationEnhId)
        {
            CngKey cngKey;
            var keyName = KeyNamePrefix + organizationEnhId;
            try
            {
                cngKey = CngKey.Open(keyName);
                cngKey.Dispose();
                DeleteKey(organizationEnhId);
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Unable to open CngKey - assuming that the key does not exist");
                Debug.WriteLine($"{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

            try
            {
                var creationParameters = new CngKeyCreationParameters()
                {
                    ExportPolicy = CngExportPolicies.AllowPlaintextExport,
                    Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
                    KeyCreationOptions = CngKeyCreationOptions.OverwriteExistingKey,
                    KeyUsage = CngKeyUsages.Signing,

                    Parameters =
                    {
                        new CngProperty("Length", BitConverter.GetBytes(Size), CngPropertyOptions.None),
                    }
                };

                Debug.WriteLine("Creating new CngKey");
                cngKey = CngKey.Create(CngAlgorithm.Rsa, keyName, creationParameters);

                using (cngKey)
                {
                    using (RSA rsa = new RSACng(cngKey))
                    {
                        Debug.WriteLine("Creating new CngKey");
                        return rsa.ToXmlString(includePrivateParameters);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception($"An error occurred: {e.ToString()}");
            }
        }

        public RSAParameters GetRsaParameters(string organizationEnhId)
        {
            try
            {
                var keyName = KeyNamePrefix + organizationEnhId;

                Debug.WriteLine("Trying to open existing CngKey");

                CreateKeyIfNeeded(organizationEnhId);
                var cngKey = CngKey.Open(keyName);

                using (cngKey)
                {
                    using (RSA rsa = new RSACng(cngKey))
                    {
                        return rsa.ExportParameters(true);
                    }
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception($"An exception occurred while opening a CngKey: {e.ToString()}");
            }
        }

        public static string GetPublicKeyAsXml(string organizationEnhId)
        {
            try
            {
                var keyName = KeyNamePrefix + organizationEnhId;
                Debug.WriteLine("Trying to open existing CngKey");
                var cngKey = CngKey.Open(keyName);

                using (cngKey)
                {
                    using (RSA rsa = new RSACng(cngKey))
                    {
                        return rsa.ToXmlString(false);
                    }
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception($"An exception occurred while opening a CngKey: {e.ToString()}");
            }
        }

        public static bool KeyExists(string organizationEnhId)
        {
            try
            {
                var keyName = KeyNamePrefix + organizationEnhId;
                var key = CngKey.Open(keyName);
                key.Dispose();
                return true;
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                return false;
            }
        }

        public static void DeleteKey(string organizationEnhId)
        {
            try
            {
                var keyName = KeyNamePrefix + organizationEnhId;
                var key = CngKey.Open(keyName);
                key.Delete();
                //Delete closes the handle to the key - no need to dispose
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Unable to delete CngKey.");
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw;
            }
        }

        private static void CreateKeyIfNeeded(string organizationEnhId)
        {
            
            if (!CngKey.Exists(KeyNamePrefix + organizationEnhId))
            {
                CreateNewKey(false, organizationEnhId);
            }
        }
    }
}
