# UC: DC - communication example

This repository contains a console application that demonstrates how to communicate with a game called Unity-Chan: Desktop Companion via port-based messaging using JSON. This app is specifically designed to interact with Unity-chan using `UnityChanMessage` objects, showcasing how to send and receive messages between applications.

The project is intended to be copied and reimplemented in games/mods/tools/anything you want to communicate with Unity-chan. So be creative and good luck! 🧡🤎

---

## Table of Contents

- [Overview](#overview)
- [Structure](#structure)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

---

## Overview

This application provides a simple example of port communication between a C# console app and Unity-Chan: Desktop Companion game. The app sends `UnityChanMessage` objects in JSON format over a specified port to control Unity-chan behaviour, or let her know about anything going on in another app.
One example use could be creating a mod for another game and notifying Unity-chan whenever player wins inside it, or creating another AI mascot and letting them both talk to each other.

---

## Structure

All messaging classes sent from app are contained inside `Models/UnityChanMessage.cs`. The base class `UnityChanMessage` contains three fields:
- `EMessageTask Task` - what type of task we are sending, i.e. sending chat message, rewarding points, making Unity-chan play animation
- `bool? AnswerBack` - *true* if we want the main game to confirm reveiving proper message, null or false if we don't need it
- `UnityChanContent Content` - the details of the request. Its content changes depending on what type of task we are sending

Example json messages:
  1. Chat Task
  ```json
  {
  "Task": "Chat",
  "AnswerBack": true,
  "Content": {
    "Message": {
      "role": "system",
      "content": "Player just lost playing Pinball.",
      "imageBase64": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA..."
    }
  }
}
```

2. Reaction Task
```json
{
  "Task": "Reaction",
  "AnswerBack": false,
  "Content": {
    "Emotion": "Happy"
  }
}
```

3. GameRegister Task
```json
{
  "Task": "GameRegister",
  "AnswerBack": true,
  "Content": {
    "IsPlay": true,
    "IsRegister": true,
    "GameName": "Unity-chan Pinball Adventure",
    "ExecutablePath": "C:/Games/Unity-chan Pinball Adventure/Unity-chan Pinball Adventure.exe",
    "ImagePath": "C:/Games/Unity-chan Pinball Adventure/icon.png",
    "Description": "A classic Pinball game with Unity-chan. Beat through sci-fi themed levels and gain highscores!",
    "IsUnityChanBasicComments": true
  }
}
```

If `AnswerBack` is set to *true*, then the game will answer back after completing the task or receiving wrong data, example messages would return the `ApiResponse` object:
- `int StatusCode` - [Http status code](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes), 200 if everything is OK, 400 is the request had wrong structure
- `string Body` - return message, in case of positive chat message it contains Unity-chan's response, otherwise it contains return code explanation


1. Positive answer for chat, code 200 and Body contains Kohaku's answer
```json
{
  "StatusCode": 200,
  "Body": "Oh no, you lost the game! Don't worry, you will beat it next time!"
}
```

2. Negative answer for request, code 400, Body contains code explanation. i.e. if we send wrong json as a request:
```json
{
  "StatusCode": 400,
  "Body": "Bad Request"
}
```

---

## Features

This console app supports sending various messages, such as:
- Chat - write chat message to Unity-chan, and await for her response. 
- Reward - grant special in-game points to the player, designed to reward for completed games, achievements. Or cheating, it's a single-player game so feel free to have fun your own way.
- Relation - change Unity-chan's mood and relation. Maybe you could make her angier if you beat her in PvP game, or reward if you give gifts etc.
- Reaction - make Unity-chan play animation
- Prop - set Unity-chan prop object, turn on and off headphones, gamepad etc.
- GameRegister - add new Game to Kohaku's Game Station. Registered games can be opened there and Unity-chan will react by commenting current screen from time to time while playing them. Command allows to choose if we want to just register, just play, or do both at once.

---

## Installation

### Prerequisites

- Any IDE supporting building .NET app (i.e. Visual Studio)
- .NET Framework 4.8
  

### Steps

1. Clone or download the repository:
   ```git clone https://github.com/JacopoDev/UC-DC---communication-example.git```
   or Click on `<> Code` and `Download ZIP` button
2. Open the project solution `PortTest.sln` in your IDE
3. Build the project.
4. You can run project directly in IDE or open the built file in `PortTest\bin\Debug\PortTest,exe`
   
---

## Usage

First you need to open Unity-Chan: Desktop Companion. Go to its Settings and Port Connection option and set the port value through which you're going to communicate. Then Enable the communication.

After that open `PortTest.exe`. If app is built correctly, it will prompt you to enter the port, and then ask you to select the task you want to send to the application.
Depending on selected task it will ask for more details and send the request. It will do it in a loop until you decide to leave the console application by selecting Exit option or just closing the app.

---

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

---
