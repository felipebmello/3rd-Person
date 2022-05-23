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
- NavMeshAgents and patrolling routes
- FOV that detects the player (visible on the inspector)
- Enemy follows player to last seen position
- Added materials to diferentiate when enemy's chasing
- returns to patrolling set waypoints or stationary position (upgraded to a stack of waypoints)
#### UI
- Added Main Menu screen
- Added Game Menu screen overlay with Flashlight UI icon
- Added Pause Menu screen
- Added Victory Menu screen
- State machine implemented for menu system


## IN PROGRESS

#### Enemy
- I've started implementing Behaviour Trees to separate the two types of enemies, 
but felt the light detection needed to be sorted out before I started working on individual nodes

## TODO:

#### UI
- Revamp visuals
#### Flashlight
- Add flickering effect
#### Player
- Add model
- Add animations
#### Enemy
- Add model
- Add two types of enemies (follows light, runs from light)
#### Maze
- Block out a bigger level
- Change goal platform to stairs
- Add textures
#### SFX
#### Post-processing effects
