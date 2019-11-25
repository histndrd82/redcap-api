using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VCU.Redcap.Utilities
{
    /// <summary>
    /// Class provides a policy that is insecure.
    /// This should only be used for testing.
    /// </summary>
    internal class BrokenCertificate
    {
        public static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> DangerousAcceptAnyServerCertificateValidator { get; } = delegate { return true; };
    }
}
