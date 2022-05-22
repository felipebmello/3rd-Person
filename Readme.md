## Packages used:

#### Cinemachine
#### ProBuilder
#### TextMeshPro

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
- follows mouse position up to a certain degree
#### Enemy 
- NavMeshAgents and patrolling routes
- FOV that detects the player (visible on the inspector)
- Enemy follows player to last seen position
- Added materials to diferentiate when enemy's chasing
- returns to patrolling set waypoints or stationary position (upgraded to a stack of waypoints)
#### UI
- Added Main Menu screen
- Added Game Menu screen overlay with Flashlight UI
- Added Pause Menu screen
- Added Victory Menu screen
- State machine implemented for menu system


## IN PROGRESS

#### Flashlight
- reused logic from Enemy FOV to create a sphere/raycast at the center of the spotlight 
to detect enemies in range, still in the process of creating gizmos to properly test the 
correct positions
- I'll probably rework the way the flashlight's direction angle is calculated in relation 
to the mouse position, reusing the logic used to create the field of view cone on enemies
- Add a cone of light to the spotlights
#### Enemy
- I've started implementing Behaviour Trees to separate the two types of enemies, 
but felt the light detection needed to be sorted out before I started working on individual nodes

## TODO:

#### UI
- Revamp visuals
- New flashlight UI icon
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
