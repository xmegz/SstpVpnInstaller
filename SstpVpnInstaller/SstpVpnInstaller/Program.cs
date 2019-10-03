using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace SstpVpnInstaller
{
    internal static class Program
    {
        /// <summary>
        /// Without argmument try to load cert from resource
        /// </summary>
        private const string DEFAULT_CERTIFICATE_RESOURCE_NAME = "default.crt";

        /// <summary>
        /// Create Sstp Vpn PhoneBook entry from certificate to hostname in User Phonebook
        /// </summary>
        /// <param name="hostName">Name of the connection</param>
        /// <returns>Entry creation is success?</returns>
        private static bool CreateSstpVpn(string hostName)
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
        /// Display content of the certificate
        /// </summary>
        /// <param name="certificate">Certificate</param>
        /// <returns>Display is ok?</returns>
        private static bool DisplayCertificate(X509Certificate2 certificate)
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
        /// Get DNS name from Executable name
        /// </summary>
        /// <returns>Reolved name </returns>
        private static string GetDnsNameFromExecutableFileName()
        {
            try
            {
                string fileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                fileName = fileName.Split('(')[0];
                fileName = Path.GetFileNameWithoutExtension(fileName);

                var hostEntry = Dns.GetHostEntry(fileName);

                return fileName;
                
            }
            catch{}

            return null;
        }


        /// <summary>
        /// Get source agrument if not present
        /// </summary>
        /// <param name="args">Command line argments</param>
        /// <returns>Arguments</returns>
        private static bool GetSourceIfNotPresent(ref string[] args)
        {
            try
            {
                if (CertificateHelper.LoadFromResource(DEFAULT_CERTIFICATE_RESOURCE_NAME) != null)
                    return true;

                if (args.Length == 1)
                {
                    return true;
                }
                else if (args.Length < 1)
                {
                    var name = GetDnsNameFromExecutableFileName();

                    if (name!= null)
                    {
                        args = new string[1];
                        args[0] = name;
                        return true;
                    }

                    var inputHelperForm = new InputHelperForm();
                    if (inputHelperForm.ShowDialog() == DialogResult.OK)
                    {
                        args = new string[1];
                        args[0] = inputHelperForm.toolStripTextBox1.Text;
                        return true;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(args) + ".Length", args.Length, "> 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), nameof(GetSourceIfNotPresent) + " Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        /// <summary>
        /// Install certificate to Root Store, Local Machine
        /// </summary>
        /// <param name="certificate">Certificate</param>
        /// <param name="canonicalName">Canonical name of the certificate</param>
        /// <returns>Install is success?</returns>
        private static bool InstallCertificate(X509Certificate2 certificate, out string canonicalName)
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
        private static bool LoadCertificate(string[] args, out X509Certificate2 certificate)
        {
            certificate = null;

            try
            {
                certificate = CertificateHelper.LoadFromResource(DEFAULT_CERTIFICATE_RESOURCE_NAME);
                if (certificate != null) return true;

             
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
        private static void Main(string[] args)
        {
            bool isSuccess = false;

            isSuccess = GetSourceIfNotPresent(ref args);
            if (!isSuccess) return;

            isSuccess = LoadCertificate(args, out X509Certificate2 certificate);
            if (!isSuccess) return;

            //isSuccess = DisplayCertificate(certificate);
            //if (!isSuccess) return;

            isSuccess = InstallCertificate(certificate, out string hostName);
            if (!isSuccess) return;

            isSuccess = CreateSstpVpn(hostName);
            if (!isSuccess) return;

            MessageBox.Show(certificate.Subject, nameof(SstpVpnInstaller) + " OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}