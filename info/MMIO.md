> Disclaimer: The information provided here is a collection of research notes and personal observations. Accuracy is not guaranteed.
### **MMIO (Memory-Mapped I/O)** — это метод доступа к регистрам устройства через адресное пространство памяти. Вместо использования специальных портов ввода-вывода (как в случае с MSR), устройство маппирует свои регистры на конкретные адреса оперативной памяти, к которым можно обращаться как к обычной памяти.
---
Для работы нужно:
1. **BAR (Base Address Register)** — физический адрес MMIO региона (обычно BAR0)
2. **Смещения (offsets)** — адреса конкретных регистров внутри региона (берутся из даташита устройства)

---

## Пример: xHCI IMOD (Interrupt Moderation)
Согласно официальной спецификации xHCI от Intel, MMIO область BAR0 разделена на три региона:

| Регион | Адрес | Описание |
|--------|-------|---------|
| **Capability Registers** | BAR + 0x00 − 0x1F | Информация о возможностях контроллера |
| **Operational Registers** | BAR + CAPLENGTH | Управление контроллером |
| **Runtime Registers** | BAR + RTSOFF | Регистры прерываний и событий |

где:
- **CAPLENGTH** — смещение, указанное в Capability Registers на offset 0x00
- **RTSOFF** — смещение, указанное в Capability Registers на offset 0x18

---

### Пошаговая инструкция

#### Шаг 1: Получить BAR адрес
Прочитай BAR регистр из PCI конфигурации, например:
```
BAR raw = 0x53400004
```
Младшие 4 бита содержат флаги и не являются частью адреса, нужно их замаскировать.
```
BAR адрес = 0x53400004 & 0xFFFFFFF0 = 0x53400000
```

#### Шаг 2: Навигация к Runtime Registers
1. Прочитай смещение **RTSOFF** из Capability Registers на offset 0x18:
```
   RTSOFF адрес = BAR + 0x18 = 0x53400000 + 0x18 = 0x53400018
   RTSOFF значение = 2000 (например)
```

2. Вычисли адрес Runtime Registers:
```
   Runtime Registers = BAR + RTSOFF = 0x53400000 + 2000 = 0x53402000
```

#### Шаг 3: Найти регистр IMOD
Регистр IMOD находится в Runtime Registers по формуле:
```
IMOD адрес = Runtime Registers + 0x20 + (0x20 * N) + 0x04
```

где **N** — номер interrupter'а (от 0 до MaxInterrupters).

**Примеры конечного адреса:**
- Interrupter 0: 0x53401000 + 0x20 + 0x04 = **0x53402024**
- Interrupter 1: 0x53401000 + 0x20 + 0x24 + 0x04 = **0x53402044**
- Interrupter 2: 0x53401000 + 0x20 + 0x44 + 0x04 = **0x53402064**

<p align="left">
  <img src="https://github.com/NZK95/RegMaster/blob/master/info/images/imod/1.png?raw=true">
</p>

#### Шаг 4: Структура регистра IMOD
Регистр IMOD — это 32-битное значение со следующей структурой:

| Поле | Биты | Описание |
|------|------|---------|
| **IMODI** | 0-15 | Интервал модерации (в единицах по 250 нс) |
| **IMODC** | 16-31 | Счётчик событий для модерации |

**Стандартное значение:** 4000 (IMODI = 4000 × 250 нс = 1 мс)

<p align="left">
  <img src="https://github.com/NZK95/RegMaster/blob/master/info/images/imod/2.png?raw=true">
</p>

#### Шаг 5: Установить желаемое значение IMOD
0x0 для полного отключения

## Ссылки
- [xHCI Specification](https://www.intel.com/content/dam/www/public/us/en/documents/technical-specifications/extensible-host-controler-interface-usb-xhci.pdf)
- [PCI Configuration Space](https://en.wikipedia.org/wiki/PCI_configuration_space)
