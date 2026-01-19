# RegMaster
[![Downloads](https://img.shields.io/github/downloads/NZK95/RegMaster/total.svg)](https://github.com/NZK95/RegMaster/releases)

> ### Disclaimer
> The author is not responsible for any possible damage caused to hardware as a result of using this project. <br>
> This software does not guarantee any increase in performance and is intended for enthusiasts only. <br>
> You use this program at your own risk. <br>

> `WinRing0` and `inpoutx64` drivers may be blocked on Windows 11 22h2 and later, set `VulnerableDriverBlocklistEnable` to  0 in `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config` to remove this limitation.

## Features
- R/W MSR registers on different cores
- R/W MMIO data for 8, 16 or 32 bit operations
- Read PCI Configuration Space registers for byte, word or dword
- Scan and enumerate all PCI devices

## Known Issues
- `Devices` tab may not work properly, especially device/vendor ids, so look at BDF address

## Requirements
- Windows 10 or higher
- Administrator privileges
- Last version of **RegMaster** from [`releases`](https://github.com/NZK95/RegMaster/releases) <br>

## Usage

### MSR
1. Select target core
> For read operation, if `All cores` checkbox is selected, no core is selected or there are selected more than 1 core, it will be read on core 0.
2. Enter `MS`R index in Address field (e.g., `0x1a0`)
3. Enter bit value (`Bitfield` or `single bit`) (if needed) — by default, the full bitmask is displayed
4. Click `Read` button to display current value
5. Modify value in `EAX/EDX` registers
6. Click `Apply` button to write changes back
7. Use `Reset` button to restore text boxes to default values

### MMIO
1. Enter memory address in the `Address field`
2. Enter bit value (`Bitfield` or `single bit`) (if needed) — by default, the full bitmask is displayed
3. For write operations, after the `0x` prefix, enter 2 digits for `Byte` values (e.g., `0x11`), 4 for `Word` (`0x10FA`), or 8 for `Dword` (`0x00000001`)
4. Click `Read` button to display the current value as a full bitmask in Byte, Word, and Dword format
5. Use the registry section at the bottom to modify individual bits and click `Apply` button to save changes
6. Click `Reset` button to restore text boxes to default values
   
### PCI Configuration
1. Enter `Bus:Device:Function` values
2. Enter `offset` value
3. Click `Read` button to display register value
4. Use `Reset` button to restore text boxes to default values

## Resources & Credits
Inspired by [chiptool](https://github.com/LuSlower/chiptool/tree/main)

## Troubleshooting
If you encounter errors or bugs, please report them via the [issue tracker](https://github.com/NZK95/RegMaster/issues).<br>
Sometimes, after reading/writing to some registries you can have BSOD screen. Just reboot your system. <br>

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.
