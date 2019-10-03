# SstpVpnInstaller
Easy Windows SSTP VPN install from certificate

## Function:

* Download certificate from host

* Install certificate Root Store, Local Machine

* Create SSTP VPN type PhoneBook entry with certificate canonical name

* Don't set default gw to vpn server

## Usage:

* Just rename file eg: vpn.example.com

* SstpVpnInstaller (without arguments) 

* SstpVpnInstaller [IpAddress]

* SstpVpnInstaller [HostName]


## Dependencies:
RasDial (https://www.nuget.org/packages/DotRas.for.Win7/)

IlMerge (https://www.nuget.org/packages/ilmerge/)
