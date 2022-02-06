# Art Of Rally Championship Mod

[![](https://img.shields.io/github/v/release/Theaninova/aorc-client?label=Download)](https://github.com/Theaninova/aorc-client/releases/latest)
![](https://img.shields.io/badge/Game%20Version-v1.3.3a-blue)
[![GitHub license](https://img.shields.io/github/license/theaninova/aorc-client.svg)](https://github.com/wulkanat/aorc-client/blob/master/LICENSE)

[![](https://img.shields.io/badge/Controller%20Reference%20Implementation-GitHub-23292F)](https://github.com/Theaninova/aorc-reference-observer)
[![](https://img.shields.io/badge/Server-GitHub-23292F)](https://github.com/Theaninova/aorc-server)

A mod that allows for real-time streaming of data via websockets to a
server which forwards them to one or more observers.

#### Discord
[![Art Or Rally Discord](https://badgen.net/discord/members/Sx3e7qGTh9)](https://discord.gg/Sx3e7qGTh9)

#### Launcher Support
![](https://img.shields.io/badge/GOG-Supprted-green)
![](https://img.shields.io/badge/Steam-Supprted-green)
![](https://img.shields.io/badge/Epic-Untested-yellow)

#### Platform Support
![](https://img.shields.io/badge/Windows-Supprted-green)
![](https://img.shields.io/badge/Linux-Untested-yellow)
![](https://img.shields.io/badge/OS%2FX-Untested-yellow)
![](https://img.shields.io/badge/PlayStation-Not%20Supprted-red)
![](https://img.shields.io/badge/XBox-Not%20Supprted-red)
![](https://img.shields.io/badge/Switch-Not%20Supprted-red)

## Usage

[![](https://img.shields.io/github/v/release/Theaninova/aorc-client?label=Download%20Client%20Mod)](https://github.com/Theaninova/aorc-client/releases/latest)
[![](https://img.shields.io/github/v/release/Theaninova/aorc-server?label=Download%20Server)](https://github.com/Theaninova/aorc-server/releases/latest)
[![Website](https://img.shields.io/website-up-down-green-red/https/theaninova.github.io/aorc-reference-observer/.svg?label=Controller%20Reference%20Site)](https://theaninova.github.io/aorc-reference-observer/)

1. Download the client and install it using the [Unity Mod Manager](https://www.nexusmods.com/site/mods/21/)
2. Download the server
3. Open the reference controller site to see the data

## Features

Multiple clients and observers can connect to a single server

### Streaming of real-time data

* Inputs like throttle, clutch, breaks etc.
* Position, Rotation, Velocity
* Car state & hidden stats like RPM, current gear, wheel tire velocity, etc.
* Timer state

### Game State Updates

* Level Loading
* Event Starting *this is under construction*
  * Level data
  * Car data
* Event Over *this is under construction*
  * Final time

### Remote Control

*This is under construction*

# Controller Development

Every controller has to connect to the server

[![](https://img.shields.io/github/v/release/Theaninova/aorc-server?label=Download%20Server)](https://github.com/Theaninova/aorc-server/releases/latest)
[![](https://img.shields.io/badge/Server-GitHub-23292F)](https://github.com/Theaninova/aorc-server)

This mod uses `socket.io`

[![](https://img.shields.io/badge/npm-socket.io--client-C8001A)](https://www.npmjs.com/package/socket.io-client)
[![NuGet](https://img.shields.io/badge/NuGet-SocketIOClient-%23004880)](https://www.nuget.org/packages/SocketIOClient)

*This gives you an overview of the protocol you can use from a controller*

Route fragment: `/controllers`

***All events have the following format:***
```ts
interface ClientData<T> {
  user: string;
  data: T;
}

type Vector3 = [number, number, number]; // float
```

## Events

### `userJoined`
Emitted when a user joined

**data**: `undefined`

### `userLeft`

Emitted when a user left

**data**: `undefined`

### `loadLevel`

Emitted when a level is loaded

**data**: `number` (level ID)

### `stageUpdate`

Emitted when the in-game timer changes

**data**:

```ts
export interface PositionData {
  position: Vector3
  rotation: Vector3
  velocity: Vector3
}

export interface AssistanceData {
  absTriggered: number // float
  tcsTriggered: number // float
  espTriggered: number // float
}

export interface InputData {
  throttleInput: number // float
  steeringInput: number // float
  brakeInput: number // float
  handbrakeInput: number // float
  clutchInput: number // float
}

export interface CarData {
  positionData?: PositionData
  inputData?: InputData
  brakeData?: BrakeData
  drivetrain?: DrivetrainData
  assistance?: AssistanceData
}

export interface BrakeData {
  temperatureBack: number // float
  temperatureFront: number // float
}

export interface DrivetrainData {
  clutch: number // float
  rpm: number // float
  torque: number // float
  gear: number // int
  wheelTireVelocity: number // float
  canStall: boolean
  throttle: number // float
  shiftTriggered: boolean
  canShiftAgain: boolean
  currentPower: number // float
  currentGearRatio: number // float
  isChangingGear: boolean
  velocity: number // float
  isStalli
```

### `waypointsGathered`

When the level has loaded, a list of all waypoints in that level will be sent

```ts
type waypoints = Vector3[]; // float
```

### `eventStart`

### `eventOver`

```ts
interface EventOverData {
  terminalDamage: boolean;
  finalTime: number; // seconds fraction
  penalties: number; // int
}
```

### `carsInfo`

**data**: takes whatever raw data there is in memory and serializes it to JSON.

### `stagesInfo`

**data**: takes whatever raw data there is in memory and serializes it to JSON.

## Actions

