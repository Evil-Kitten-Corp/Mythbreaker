<div align="center">
  
# Mythbreaker

  ![Unity Version](https://img.shields.io/badge/Unity-2022.3.10f1-blue)
[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)
[![CC BY-NC-SA 4.0](https://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-lightgrey.svg)](http://creativecommons.org/licenses/by-nc-sa/4.0/)

</div>

[![Jira](https://img.shields.io/badge/Jira-0052CC?style=for-the-badge&logo=Jira&logoColor=white)](https://ateliergroupname.atlassian.net/jira/software/projects/SCRUM/boards/1/timeline?shared=&atlOrigin=eyJpIjoiMWMzYjUwN2NjNzE5NGJhMDhkMzRjNzQ3NGYzM2VjM2YiLCJwIjoiaiJ9)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![Rider](https://img.shields.io/badge/Rider-000000.svg?style=for-the-badge&logo=Rider&logoColor=white&color=black&labelColor=crimson)

## Game Overview

*Mythbreaker* is a Third-Person Hack ‘n Slash RPG where you fight through waves of enemies using a variety of divine powers. The game features a combat system that lets you chain combos and use up to 10 unique abilities to defeat robotic enemies. As you progress, you unlock new abilities and face stronger enemies.

- **Genre**: Hack n’ Slash
- **Perspective**: Third Person
- **Number of Players**: Single Player
- **Target Platform**: Windows PC

---

## Table of Contents
1. [Overview](#overview)
2. [Game Design](#game-design)
   - [Setting](#setting)
   - [Gameplay](#gameplay)
   - [Controls](#controls)
   - [Progression](#progression)
   - [Movement](#movement)
   - [Combat](#combat)
   - [Abilities](#abilities)
   - [Enemies](#enemies)
3. [Installation](#installation)
6. [License](#license)

## Overview
**Mythbreaker** is a Third-Person Hack 'n Slash RPG where the player must fight through waves of enemy robots. The game loop revolves around the player gaining new abilities after defeating each wave and using them to overcome increasingly difficult enemies.

- **Genre**: Hack n’ Slash
- **Perspective**: Third Person
- **Number of Players**: Single Player
- **Target Platform**: Windows PC

## Game Design

### Setting
The game is set in an abandoned warehouse disguised as a sprawling city, used by a company as a holding cell and test facility. The player is one of the test subjects, unknowingly helping to refine the combat skills of robots designed for war. With the help of divine powers, the company believes the player is the perfect candidate for the tests.

### Gameplay
- Fight through 12 waves of robots, gaining divine abilities after each wave to help combat increasingly tougher enemies.
- **Progression**: Progress is saved at the start of each round. If the player dies, they will respawn at the beginning of the round.

### Controls

|Action |	Key|
| -------------------- | ------------------ |
Move	|WASD
Jump	|Space
Dash/Sidestep	|Shift
Sprint	|Hold Shift
Hook	|Q
Light Attack	|Left Mouse Button
Heavy Attack	|Right Mouse Button
Ability Slot 1	|1
Ability Slot 2	|2
Ability Slot 3	|3
Ability Slot 4	|4
Warp Strike	|E

### Progression
At the beginning of each round, the player's progress is saved, allowing them to continue from where they left off if they die.

### Movement
Movement focuses on agility and combat utility, including features like:
- **Double Jump**: Jump twice while in the air.
- **Sprint**: Move faster with continuous running.
- **Hook**: A grapple hook that attaches to surfaces.
- **Dash**: A quick burst of movement to a target location.
- **Run**: Base movement function.

### Combat
Combat involves a combo system with light and heavy attacks, plus 4 ability slots that add depth to combos. Abilities can be selected from a list of 10 options, giving players the chance to build diverse playstyles across different sessions.

#### Basic Combat Keys
| Key                  | Function           |
| -------------------- | ------------------ |
| Left Mouse Button     | Light Attack       |
| Right Mouse Button    | Heavy Attack       |
| E                     | Warp Strike        |

#### Abilities
The player can assign abilities to any of the 4 available slots. Here’s an example of some abilities:

| Ability               | Description                                                           | Damage | Type            | Cooldown |
| --------------------- | --------------------------------------------------------------------- | ------ | --------------- | -------- |
| **Thorn Launch**       | Launch thorns at marked enemies after attacking them.                 | 25     | Targeted        | 3s       |
| **Earth Rumble**       | Tremble the ground, knocking up and damaging enemies in the blast.    | 40     | Area of Effect  | 4s       |
| **Lightning Slasher**  | Slash through enemies while becoming immune to damage.                | 15/slash | Area of Effect | 8s       |
| **Take Down**          | Leap to a target location and stun enemies in an area.                | 30     | Area of Effect  | 6s       |

More abilities are detailed in the game design document and in-game.

### Enemies
The game features different classes of enemies, such as:
- **Ranged Unit**: High attack damage, long-range.
- **Melee Unit**: Close-range, strong attacks.
- **Feline Unit**: Fast and agile enemies.

## Installation
1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/yourusername/mythbreaker.git
   ```
2. Navigate to the project directory:
   ```bash
   cd mythbreaker
   ```
3. Install dependencies (if using Unity):
   - Open the project in Unity.
   - Ensure all required assets are imported.
4. Build the project and run the executable for Windows PC.

## Future Plans
We are not planning on expanding this project further. It remains as a vertical slice prototype made for a class to test our competences on building this type of game in such a short amount of time.

## License
This project is licensed under the Attribution-Non Commercial-Share Alike 4.0 International License - see the [LICENSE](LICENSE.md) file for details.
