using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace SstpVpnInstaller
{

    static class Program
    {
        /// <summary>
        /// Without argmument try to load cert from resource
        /// </summary>
        public const string DEFAULT_CERTIFICATE_RESOURCE_NAME = "default.crt";

       
        /// <summary>
        /// Display content of the certificate
        /// </summary>
        /// <param name="certificate">Certificate</param>
        /// <returns>Display is ok?</returns>
        public static bool DisplayCertificate(X509Certificate2 certificate)
        {
            try
            {
                X509Certificate2UI.DisplayCertificate(new X509Certificate2(certificate));

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), nameof(DisplayCertificate) + " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }


        /// <summary>
        /// Create Sstp Vpn PhoneBook entry from certificate to hostname in User Phonebook
        /// </summary>
        /// <param name="hostName">Name of the connection</param>
        /// <returns>Entry creation is success?</returns>
        public static bool CreateSstpVpn(string hostName)
        {
            try
            {
                RasHelper.CreateSstpVpn(hostName, hostName);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), nameof(CreateSstpVpn) + " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        /// <summary>
        /// Install certificate to Root Store, Local Machine
        /// </summary>
        /// <param name="certificate">Certificate</param>
        /// <param name="canonicalName">Canonical name of the certificate</param>
        /// <returns>Install is success?</returns>
        public static bool InstallCertificate(X509Certificate2 certificate, out string canonicalName)
        {
            canonicalName = null;
            try
            {
                canonicalName = certificate.GetNameInfo(X509NameType.SimpleName, false);

                CertificateHelper.Install(certificate);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), nameof(InstallCertificate) + " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        /// <summary>
        /// Load certificate from file, http server or resource
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="certificate">Certificate</param>
        /// <returns>Load is success?</returns>
        public static bool LoadCertificate(string[] args, out X509Certificate2 certificate)
        {
            certificate = null;

            try
            {
                certificate = CertificateHelper.LoadFromResource(DEFAULT_CERTIFICATE_RESOURCE_NAME);
                if (certificate != null) return true;

                if (args.Length > 1)
                    throw new ArgumentOutOfRangeException(nameof(args) + ".Length", args.Length, "> 1");

                if (args.Length == 1)
                {
                    string name = args[0];

                    if (File.Exists(name))
                    {
                        certificate = CertificateHelper.LoadFromFile(name);
                        return true;
                    }
                    else
                    {
                        certificate = CertificateHelper.LoadFromTcpHost(name);
                        return true;
                    }
                }
                else
                {
                    var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                    var files = new List<string>();

                    files.AddRange(Directory.GetFiles(path, "*.crt"));
                    files.AddRange(Directory.GetFiles(path, "*.pfx"));
                    files.AddRange(Directory.GetFiles(path, "*.pem"));
                    files.AddRange(Directory.GetFiles(path, "*.cer"));

                    if (files.Count > 1)
                        throw new AmbiguousMatchException("Multiple certificate file found!");
                    else if (files.Count < 1)
                    {                       
                        throw new FileNotFoundException("Certificate file not found!");
                    }

                    certificate = CertificateHelper.LoadFromFile(files[0]);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), nameof(LoadCertificate) + " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }
        /// <summary>
        /// Enrty point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        public static void Main(string[] args)
        {            
            bool isSuccess = false;

            isSuccess = LoadCertificate(args, out X509Certificate2 certificate);
            if (!isSuccess) return;

            isSuccess = DisplayCertificate(certificate);
            if (!isSuccess) return;

            isSuccess = InstallCertificate(certificate, out string hostName);
            if (!isSuccess) return;

            isSuccess = CreateSstpVpn(hostName);
            if (!isSuccess) return;

            MessageBox.Show(certificate.Subject, nameof(SstpVpnInstaller) + " OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
       
    }
}
