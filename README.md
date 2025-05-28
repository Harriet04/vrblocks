# vrblocks (Add splash)

[About](#about) | [Documentation](#documentation) | [Installation](#installation) | [Screenshots](#screenshots) | [Development](#development) | [Contact Us](#contact-us)

## About

This project is intended as an educational tool to teach fundamental programming skills through the medium of Virtual Reality.

It draws inspiration from tools like Blockly and Scratch, while extending the metaphor of block-based programming to a fully 3D environment.

The project began as our [development groups's](#development) senior capstone project at UNT. Since then, we've continued development at a slower but steady pace.

### Principals

The project should be progressive, introducing simple constructs and functionality, and using them to build grandually more complex algorithms to achieve specific goals.

The project should be relevant to computer science. It may introduce concepts using toy examples, but ultimately it should
teach practical and realistic examples that can be applied to real programs.

The project should be fun. Programming is a complex, and often extremely dry subject. Introducing the concept to children and
adult beginners in a way that is engaging, is perhaps a never-ending endeavor for computer science educators. This project
should employ the techniques of game design to create real motivation to solve the given problems.
Progression systems, achievements, unlockables, and easter eggs are all viable mechanisms to engage players.

## Documentation

Documentation for the scripts and assets in this project are available in the [Wiki](https://github.com/reckoncrafter/vrblocks/wiki).

## Installation

This project is currently built for Oculus/Meta Quest headsets, and tested on Oculus/Meta Quest 2 headsets. The APK be downloaded from the [Releases](https://github.com/reckoncrafter/vrblocks/releases) page.

The project is not available on the Meta Quest store, and must be sideloaded.

To allow sideloading on Meta headsets, Developer Mode must be enabled. Instructions for enabling developer mode can be found [here](https://developers.meta.com/horizon/documentation/native/android/mobile-device-setup/)

### Android Debug Bridge

If you don't already have Android SDK Platform Tools installed, they can be downloaded [here](https://developer.android.com/tools/releases/platform-tools)

First connect your device to `adb`. Make sure the ADB daemon is started by running.
```
$ adb devices
```

When plugging in your headset over USB, your device should prompt you to allow USB debugging.
Once USB debugging is allowed, running `$ adb devices` should now show your device's serial number and name.

To install the project, run:
```
$ adb install vrblocks_android_oculus.apk
```
### SideQuest

SideQuest is a tool for installing applications for the Oculus/Meta Quest that aren't available in the Meta Quest Store.

It can be installed [here](https://sidequestvr.com/setup-howto).

After downloading the APK for the project, open SideQuest, and plug in your headset. You may be prompted to allow USB debugging.

Click on the icon labeled "Install APK from file on computer" and select the APK.

## Screenshots

ADD ME

## Development

### The Garbanzo Boys Development Team 

- [Tyler Braun](https://github.com/TyTheRockstar)
- [Malcolm Case](https://github.com/malcolmcase97)
- [Dawson Finklea](https://github.com/reckoncrafter)
- [Johnathon Lawton](https://github.com/J1aw)
- [Kevin Pham](https://github.com/KevinFham)

### Other Contributors

- None Yet!!

## Contact Us

If you have any questions about the project, or if you're interested in contributing, feel free to reach out!

📧 Email: [malcolm.case.97@gmail.com](mailto:malcolm.case.97@gmail.com)