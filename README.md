# Elite Joystick

Highly custom code for joining together multiple joysticks to output the movement to virtual joystick devices.  I use this project for my [simpit](https://en.wikipedia.org/wiki/Simulation_cockpit) with [Elite Dangerous](https://www.elitedangerous.com/).  The examples in this project can be used for other devices and games.

### Design

The project can be broken down into three major parts.

##### Service

The service (Windows Service or command line) is the part that reads the input from the attached devices.  I currently have joysticks, game controllers, midi devices, keyboards, and mice all attached to the simpit.  The service connects to these devices and reads their inputs.  If there is an input that I would like to change, I react to that input using [Reactive Extensions: Rx](https://github.com/dotnet/reactive) and output a new value on a virtual joystick using [vJoy](http://vjoystick.sourceforge.net/joomla/).  I can also react to input by sending keystrokes, executing other scripts, or changing state.

##### Client

The client can communicate to the service using [IPC](https://en.wikipedia.org/wiki/Inter-process_communication) and send basic messages.  These messages can start or stop the device listeners and change state in the service.  The client can be a command line executable, a [VoiceAttack plugin](https://voiceattack.com/), or the [EDDI Voice Attack Plugin](https://github.com/EDCD/EDDI).  I am currently just using the command line utility.

##### Game & Service State

The game and service states are part of the main service.  They can read the game state by utilizing the [EliteAPI](https://github.com/EliteAPI/EliteAPI) project.  Game state changes are translated into reactive (Rx) actions and processed the same way the service handles device inputs.  The service state is a container for common values to define the current state of the service and allows the reactive methods to perform different actions on the same inputs.

### Prerequisites

[vJoy](http://vjoystick.sourceforge.net/joomla/) Installed and 4 virtual joysticks configured.

### Running The Code

After building you will run the binary output from the EliteJoystickService project.  Executing this file will bring up a command line output window.  You could also register this executable as a Windows Service and start it from there.  After the service is running another program needs to attach to the IPC connection and send the "Connect" message.  This can be done from the EliteJoystickConsole executable.  You will receive a bunch of errors from the service executable as the service attempts to connect to the devices I have installed on my specific simpit.

### Customization

The connections to the specific devices can be found in the EliteJoystick\EliteJoystickService\JoystickService.cs file.  By following the examples in the vJoyMapping project you can create classes to define the connection and reactive methods for your specific device.

#### Other Notes:

- I use an [Arduino Due](https://store.arduino.cc/usa/due) to send the keyboard output.  I had problems with DirectInput picking up keystrokes from SendKey().
- Midi devices are supported and I am working on the library to read in from other devices besides the [Pioneer DDJSB2](https://www.pioneerdj.com/en-us/product/controller/archive/ddj-sb2/black/overview/)
- The repository contains a lot of projects for other functionality (like Force Feedback for the MSFFB2).  Some of these are deprecated but I didn't want to lose the knowledge of how to perform the actions they contain.  Sorry for the mess.
- 


