namespace X509CertificateProject
{
    using System;
    using System.IdentityModel.Selectors;
    using System.Security.Cryptography.X509Certificates;

    internal class Program
    {
        private static void Main(string[] args)
        {
            // load certificate from cert store (user/computer store = MY = Personal)
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            // ... do work
            foreach (var cert in store.Certificates)
            {
                // validate certificates
                var chain = new X509Chain();
                var policy = new X509ChainPolicy
                {
                    RevocationFlag = X509RevocationFlag.EntireChain,
                    RevocationMode = X509RevocationMode.Online,
                    UrlRetrievalTimeout = TimeSpan.FromMilliseconds(10000)
                };

                chain.ChainPolicy = policy;
                if (!chain.Build(cert))
                {
                    // do some work
                }

                // validation - special class to validate cert
                var validator = X509CertificateValidator.ChainTrust;
                validator.Validate(cert);

                Console.WriteLine(cert.FriendlyName);
            }

            store.Close();
        }
    }
}