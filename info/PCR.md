### PCR  — это специальные приватные регистры чипсета, которые находятся в приватной памяти чипсета, предназначены для конфигурации внутренних подсистем (GPIO, PMC, USB, SATA, DCI и др.) и доступны через **SBI** (Sideband Interface) или **P2SB** (Primary to Sideband Bridge). <br> Эти регистры адресуются через **PID** (Port ID) и оффсет смещения. 

---

## **Основные компоненты**

### **SBI (Sideband Interface)**
SBI — это старинный протокол внутри чипсета, предназначенный для доступа к регистрам компонентов, которые не имеют собственного MMIO адреса.

### **P2SB (Primary-to-Sideband Bridge)**
P2SB — это PCI устройство (с адресом шины 0, устройства 31, функции 1 (D31:F1)). У P2SB есть регистр BAR0, который указывает на базу PCR MMIO (часто называемой SBREG – sideband registers). Главная его функция заключается в том что, он переводит обычный MMIO доступ в SBI доступ, отправляя команду в **SBI** интерфейс (PID + оффсет). Таким образом, можно обращаться к компонентам чипсета как к обычной памяти.
> По умолчанию firmware скрывает P2SB устройство, чтобы ОС не переназначала его BAR, а также чтения из него возвращают 0xFFFFFFFF (скрыт) – но при этом записи разрешены.

### **PID (Port ID)**
PID - В чипсете находится более 50 компонентов, и для каждого компонента существует свой уникальный идентификатор **PID**, который используется в интерфейсе **SBI** для обращения к регистрам.

---

### **Пример PID для компонентов чипсета**

| **Компонент**                | **PID** |
|------------------------------|---------|
| Power Management Controller  | 0xCC    |
| PMC Broadcast                | 0xCD    |
| USB 3.0 хост                 | 0x70    |
| USB 2.0 PHY                  | 0xCA    |
| SATA контроллер              | 0xD9    |
| Integrated Sensor Hub        | 0xBE    |
| Digital Signal Processor     | 0xD7    |
| Gigabit Ethernet             | 0xDB    |
| Time-Sensitive Networking    | 0xDA    |
| PCIe Sideband Fabric         | 0xBA    |
| LPC/eSPI контроллер          | 0xC7    |
| SMBus контроллер             | 0xC6    |
| CNVi (WiFi/Bluetooth)        | 0x73    |

---

## Использование
Для изменения значений в регистрах **PCR**, необходимо вычислить адрес в памяти: ``` SBREG_BAR + (PID << 16) + Offset  ```. Дальше работать с ним как с обычным регистром в MMIO, следуя документацию.

## Ссылки
- [Intel Leak](https://sizeof.cat/post/intel-alder-lake-bios-sourcecode-leak/)
- [1](https://lab.whitequark.org/notes/2017-11-08/accessing-intel-ich-pch-gpios/#:~:text=%2Asbreg_addr%20%3D%20d31f1,08lx%22%2C%20%2Asbreg_addr)
- [2](https://xairy.io/articles/thinkpad-xdci#:~:text)
