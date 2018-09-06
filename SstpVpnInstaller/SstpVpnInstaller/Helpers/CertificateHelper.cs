using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SstpVpnInstaller
{
    internal static class CertificateHelper
    {
        public static void Install(X509Certificate2 certificate, StoreName storeName = StoreName.Root, StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            var store = new X509Store(storeName, storeLocation);

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
        }

        public static X509Certificate2 LoadFromFile(string fileName)
        {
            return new X509Certificate2(fileName);
        }

        public static X509Certificate2 LoadFromResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (!assembly.GetManifestResourceNames().Contains(resourceName))
                return null;

            var stream = assembly.GetManifestResourceStream(resourceName);
            var rawData = new byte[stream.Length];

            stream.Read(rawData, 0, rawData.Length);

            var certificate = new X509Certificate2(rawData);

            return certificate;
        }

        public static X509Certificate2 LoadFromTcpHost(string hostName, int tcpPort = 443)
        {
            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(hostName, tcpPort);

                using (var sslStream = new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
                {
                    sslStream.AuthenticateAsClient(hostName);
                    var cert = new X509Certificate2(sslStream.RemoteCertificate);

                    return cert;
                }
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {           
            return true;
        }
    }
}
