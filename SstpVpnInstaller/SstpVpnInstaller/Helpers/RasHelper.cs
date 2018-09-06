using System;
using DotRas;
using System.Linq;

namespace SstpVpnInstaller
{
    internal static class RasHelper
    {
        public static void CreateSstpVpn(string entryName, string serverName, RasPhoneBookType rasPhoneBookType = RasPhoneBookType.User, RasVpnStrategy rasVpnStrategy = RasVpnStrategy.SstpOnly)
        {
            var path = RasPhoneBook.GetPhoneBookPath(rasPhoneBookType);            
            var phoneBook = new RasPhoneBook();
            phoneBook.Open(path);

            if (phoneBook.Entries.Contains(entryName))
                throw new ArgumentException("PhoneBook entry already exist!", entryName);

            var device = RasDevice.GetDevices().FirstOrDefault(d => d.Name.Contains("SSTP"));
            var entry = RasEntry.CreateVpnEntry(entryName, serverName, rasVpnStrategy, device);

            entry.Options.RemoteDefaultGateway = false;

            phoneBook.Entries.Add(entry);                 
        }

    }
}
