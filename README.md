# RegMaster
[![Downloads](https://img.shields.io/github/downloads/NZK95/RegMaster/total.svg)](https://github.com/NZK95/RegMaster/releases)

> ### Disclaimer
> The author is not responsible for any possible damage caused to hardware as a result of using this project. <br>
> This software does not guarantee any increase in performance and is intended for enthusiasts only. <br>
> You use this program at your own risk. <br>

> `WinRing0` and `inpoutx64` drivers may be blocked on Windows 11 22h2 and later, set `VulnerableDriverBlocklistEnable` to  0 in `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config` to remove this limitation.

## Features
R/W MSR registers on different cores
R/W MMIO data for 8, 16 or 32 bit operations
R/W PCI Configuration Space registers for byte, word or dword
Scan and enumerate all PCI devices

## Known Issues
- `Devices` tab may not work properly, especially device/vendor ids, so look at BDF address

## Requirements
- Windows 10 or higher
- Administrator privileges
- Last version of **RegMaster** from [`releases`](https://github.com/NZK95/RegMaster/releases) <br>

## Usage


## Resources & Credits
Inspired by [chiptool](https://github.com/LuSlower/chiptool/tree/main)

## Troubleshooting
If you encounter errors or bugs, please report them via the [issue tracker](https://github.com/NZK95/RegMaster/issues).<br>
Sometimes, after reading/writing to some registries you can have BSOD screen. Just reboot your system. <br>

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.
