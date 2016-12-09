![CCManager header](https://raw.github.com/jglim/CCManager/master/other/images/header-small.png)


CCManager is a control panel for a sub-1GHz radio frequency transmitter, with hardware design based on the Texas Instruments CC1101 IC.

I've been looking for a simple, strong sub-1GHz transmitter to complement my receive-only RTL2832U SDR. 

Importantly, it has to be affordable especially since I'm on a student budget. The really impressive, hobbyist-friendly [HackRF](http://www.kickstarter.com/projects/mossmann/hackrf-an-open-source-sdr-platform) costs $275 which is still quite pricey for students. My entire TX/RX setup, although rather hacky, costs about ~$30.

I designed this with the intention of making RF penetration testing more accessible and affordable for hackers and pentesters. As RF is invisible and typically not audited, security is surprisingly lax and thus this opens up more opportunities for a greater attack surface.

About CCManager
---
The goal of CCManager is to have a complete RF (transmit) pentesting hardware and software suite. The CC1101 chip interfaces with the computer via a USB-SPI bridge such as a MSP430 Launchpad, Arduino etc. 

The hardware design is hacker-friendly where you can mix-and-match parts using whatever you have.

CCManager comes in 2 parts - the software here, as well as the hardware consisting of a CC1101 module and USB-SPI bridge.

For receiving RF data, look into the RTLSDR/RTL2832U projects. 

![CCManager software](https://raw.github.com/jglim/CCManager/master/other/images/ccmanager-view.png)
_The software portion_

Features
---
Low hardware BOM Cost - To assemble a basic, functional transmitting device, you'll only need about $20 in parts. (to receive, get a ~$12 RTLSDR device)
 - 1 CC1101 module - Below $10
 - 1 Texas Instruments Launchpad - $10
 - 6 Female to Female jumper wires (~$3 for a pack of 40. Consider asking a friend nicely if they have some) 

Uses the very popular, affordable and widely available CC1101 module 
These boards are typically below US$10 on ebay, Aliexpress, dx.com, with free worldwide shipping. 

Easy to assemble - no soldering required.

Transmit across most of the sub-1GHz band (specifically 300-348, 387-464, 779-928 MHz)
The CC1101 boards usually have a RF frontend designed for a certain frequency, although they can transmit on other frequencies (albeit with some attenuation)

Configure all registers on the CC1101 device. You can configure most of the core features using registers, such as data whitening, channel spacing, data rate, carrier frequency etc.

Import configuration from SmartRF Studio 7 - useful when configuring alternate modes such as GFSK

Written and works on Windows, runs well on Linux via mono.

Things you can do
---
RF replay attack - attack fixed-code communication devices such as some gates, doorbells and wireless switches. ~~I'll be publishing a guide shortly~~ (shortly: 3 years). A guide is now available [here](https://github.com/jglim/CCManager/files/612566/CCManager.pdf).


![CCManager software](https://raw.github.com/jglim/CCManager/master/other/images/doorbell.gif)

_fixed code doorbell being triggered wirelessly by CCManager_

Build your own
---
You can try this guide to build a basic CCManager hardware 

This is the easiest to build, tinker with, reuse/upgrade and most available and affordable. However the onboard UART restricts the communication speed to 9600 bps. _If you know what you are doing, you can use an external USB-UART to run at 115200 bps_.

1. Get the required hardware: 
    - CC1101 module
    - TI Launchpad (MSP430G2)
    - 6 F-F jumper wires.
1. Set up the hardware - connect
    - CC1101 VDD - Launchpad VCC
    - CC1101 GND - Launchpad GND
    - CC1101 CSn - Launchpad P1.4
    - CC1101 SCK - Launchpad P1.5
    - CC1101 MISO - Launchpad P1.6
    - CC1101 MOSI - Launchpad P1.7
1. Enable hardware UART on the Launchpad by rotating the TXD and RXD jumpers (remove the 2 jumpers and reinsert them horizontally). The jumpers are found on the right side of the text "EMULATION" near the dotted line.
1. Install [Energia](http://energia.nu/), as well as the Launchpad [serial port drivers](https://github.com/noccy80/mspdev/tree/master/reference/launchpad-captouch/LaunchPad_Driver).
1. Run Energia and.. 
    - Paste the contents of firmware/cc1101-launchpad-passthrough.c into the textarea
    - Under Tools>Board, select "LaunchPad w/ msp430g2553 (16MHz)"
    - Under Tools>Serial Port, pick the (usually) only option. If you have more than 1 entry, the largest number is typically the correct one.
    - Select File>Upload
    - Once this completes, you can close Energia
1. Run CCManager.exe - when prompted for a serial port, pick the same one. If you have an SDR, try transmitting something like `0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00` on a known frequency. If you see your transmission, then your setup works!
    
_Any hardware that implements the same (simple) serial protocol can work with CCManager. I've had success with an Arduino Due and Pro Mini 3.3v as well._

__BEAR IN MIND THAT THE CC1101 MODULE IS 3.3V ONLY__ Most popular Arduinos run on 5V, and connecting a 3.3V module will likely brick it!

Limitations
---
Restricted to 61 bytes of data per transmission. The USB-SPI bridge appears to be incapable of communicating fast enough, and also I do not know if it is possible to disable the entire RX FIFO so that the TX FIFO can be doubled.

~~I haven't figured a way to disable the CC1101's preamble and sync transmission. However most transmissions still work fine since only the first portion (may) be discarded.~~ This has been fixed. Thanks [@AzInstall](https://github.com/AzInstall) !

Other Notes
---
I'd also like to thank the guys at Panstamp, where they have released their code at http://code.google.com/p/panstamp/ . Their work has really helped me to understand the CC1101 module better.

The hardware they sell ([Panstamp](http://www.panstamp.com/products/wirelessarduino)) consists of an Arduino and a CC1101 module, making it a great fit. That means that you can likely modify the firmware code and use the Panstamp as a really small CCManager hardware. Unfortunately they don't ship to where I am so I am unable to verify if that works. (Do let me know if it does!)

There are also "UART" versions of the CC1101 modules from China. From what I observe, they use an Atmega168 to process AT commands from the UART. If you are feeling adventurous, it should be possible to reflash the firmware on the Atmega168 and use it as a CCManager hardware.
