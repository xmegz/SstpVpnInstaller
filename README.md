# SstpVpnInstaller

Easy Windows SSTP VPN install from host certificate

## Function:

* Download certificate from host

* Install certificate to Root Store, Local Machine category (It doesn't matter, valid or not)

* Create SSTP VPN type PhoneBook entry with certificate canonical name

* Don't set default gateway to host server

## Download: [Latest](https://github.com/xmegz/SstpVpnInstaller/releases/download/v1.4.0/SstpVpnInstaller.Full.exe)

## Usage:
0, Download file & Just rename it to target hostname

1, Start program

![alt text](https://raw.githubusercontent.com/xmegz/SstpVpnInstaller/master/Images/Step1.png)

2, Ack UAC warning

![alt text](https://raw.githubusercontent.com/xmegz/SstpVpnInstaller/master/Images/Step2.png)

3, Show install details

![alt text](https://raw.githubusercontent.com/xmegz/SstpVpnInstaller/master/Images/Step3.png)

4, Start connect

![alt text](https://raw.githubusercontent.com/xmegz/SstpVpnInstaller/master/Images/Step4.png)

## Alterantive Usage
* SstpVpnInstaller (without arguments) 

* SstpVpnInstaller [IpAddress]

* SstpVpnInstaller [HostName]

## Dependencies:
RasDial (https://www.nuget.org/packages/DotRas.for.Win7/)

IlMerge (https://www.nuget.org/packages/ilmerge/)
