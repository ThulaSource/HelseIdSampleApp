namespace HelseIdSampleApp.OpenIdConnect.DCR
{
    public static class SecretTypes
    {
        //This is a special case of the client authentication method private_key_jwt - where an Enterprise certificate is used, and the public key is 
        //saved as a XML formatted string in IdentityServer4 client config
        public const string EnterpriseCertificate = "EnterpriseX509CertificateBase64";

        //This is a special case of the client authentication method private_key_jwt - where a Rsa key pair is generated, and the public key is 
        //saved as a XML formatted string in IdentityServer4 client config
        public const string RsaPrivateKey = "private_key_jwt:RsaPrivateKeyJwtSecret";
    }
}
