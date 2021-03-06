## Controls:

#### Press 'WASD' to move
#### Press 'Shift' to run
#### Press 'Space to jump
#### Press 'Left Mouse Button' to turn flashlight on/off
#### Press 'Right Mouse Button' to turn rotate the camera around
#### Use the mouse to direct your flashlight input
#### Press 'Esc' to pause the game

## Executable:

https://felipemello.itch.io/3rd-person-prototype?password=testeunityrisestudio

## Packages used:

#### Cinemachine
#### ProBuilder
#### TextMeshPro

## Assets used
Flashlight Icon by Daniel Bruce on IconScout

## DONE:

#### 3rd person movement controller
- Running action with left shift
- Jumping action with spacebar
- State machine implemented for player movement system
#### 3rd person camera
- free movement with right mouse click
- Camera blends closer when running
#### Flashlight
- turns on and off with left mouse click
- follows mouse position up to a maximum angle determined by the player's field of view component
- casts sphere on the spotlight to detect enemies within range / hit by the light
#### Enemy 
- NavMeshAgents
- FOV that detects the player (visible on the inspector)
- Enemy follows player to last seen position
- Added materials to diferentiate when enemy's chasing
- Added Behaviour Trees for Enemies Type #1 and #2
    - Has FOV, chases players and is afraid of light
    - Doesn't have FOV, chases light
- Removed patrolling behaviour due to poor performance and weird movements
#### UI
- Added Main Menu screen
- Added Game Menu screen overlay with Flashlight UI icon
- Added Pause Menu screen
- Added Victory Menu screen
- State machine implemented for menu system
#### Level Design
- Blocked out a maze
- Removed other

## BACKLOG:

#### UI
- Revamp visuals
- Add an Instructions Menu
- Use multiple scenes and MenuManager.cs singleton
#### Flashlight
- Add flickering effect
#### Player
- Add model
- Add animations
#### Enemy
- Add model
- Rework patrol behaviour
#### Maze
- Add textures
#### SFX
#### Post-processing effects
