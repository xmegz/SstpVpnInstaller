using System;
using DotRas;
using System.Linq;
using System.Windows.Forms;

namespace SstpVpnInstaller
{
    internal static class RasHelper
    {
        public static void CreateSstpVpn(string entryName, string serverName, RasPhoneBookType rasPhoneBookType = RasPhoneBookType.User, RasVpnStrategy rasVpnStrategy = RasVpnStrategy.SstpOnly)
        {
            var path = RasPhoneBook.GetPhoneBookPath(rasPhoneBookType);

            using (var phoneBook = new RasPhoneBook())
            {
                phoneBook.Open(path);

                if (phoneBook.Entries.Contains(entryName))
                {
                    MessageBox.Show(entryName + " recreated!", "Phonebook", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    phoneBook.Entries.Remove(entryName);
                }

                var device = RasDevice.GetDevices().FirstOrDefault(d => d.Name.Contains("SSTP"));
                var entry = RasEntry.CreateVpnEntry(entryName, serverName, rasVpnStrategy, device);

                //entry.Options.RemoteDefaultGateway = true;

                phoneBook.Entries.Add(entry);                
            }
        }

    }
}
