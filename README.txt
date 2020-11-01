<<<<<<< HEAD:README.txt
<<<<<<< HEAD:README.txt
Introduction
Brief
This area will be added to as new knowledge as each section is acquired. I will also try a pair each introduction section with the GitHub commits. A good starting note is the Unity IDE. It is made for non-programmers since even some of the most advanced tutorials are very simple in terms of programing knowledge.
Unity Hub
This is a tool used to develop the unity game. The most obvious change compared to other IDE is its focus on a scene and not any form of code. Most IDE have some drop and drag feature but it normally isnft the primary way you implement things in the IDE and only works for some languages.
In unity itfs in reverse the drop and drag UI if the primary feature and the scripts or code are the addition.
With this knowledge I will need to sift through the UI elements and turn them into code. The UI elements are inherently hard coded and not dynamic but if I can generate UI element with code, I can break this inbuilt cycle.
Working with Unity
The Scene and code division adds a very interesting problem especially for a puzzle game that needs dynamic generation. That problem is I donft need the scene at all, the only potential us is for something like backgrounds or UI.
The ability to create a dynamic puzzle requires the creation of the scenes assets that can be scaled. For future note a 2d game assets would instead be duplicated many times.
Another way to look at this is each scene asset is a separate view interface. They can instance once created and manipulated; this is how normal Unity development works but on a much smaller scale.
Pushed to the extreme one asset could serve the entire programfs needs, like letfs say like moving grass.
(Unity, 2019)

MoSCoW
A good idea of what I need to priorities in each prototype.
Must
Instanced grid asset
Instantiate one image into multiple grid tiles
UI for menu and end screen with reuse
Should
Update grid with changed on from a save variable
Saving variable parsed into a file
File can be edited and is parsed into the save variable
More than one level with some sort of selection menu
Could
Have image save file
Load save file into separate program to edit grid
Auto generated game logic based of save variable
Instantiate the UI assets.
Would
Add some navigation
Hints to help discover the image
=======
# Puzzle Game
 
>>>>>>> parent of 70d9278... Update Readme:README.md
=======
# Puzzle Game
 
>>>>>>> parent of 70d9278... Update Readme:README.md
