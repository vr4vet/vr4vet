# State Saver Read Me

## Adding the State Saver to your project

1. Add the stateSaver directory into your project

2. Drag the stateSaver prefab from the directory into your scene folder

3. Drag the SavedStates prefab from the directory and add it as a child of Pause Menu

4. Drag the SavedStates menu that you just added into the scene into the Load Menu property of the stateSaver (See figure 1)

5. Drag the Pause Menu into the Main Menu property of the state saver (Used to resume the game when a state is loaded) (See figure 1)

![](C:\Users\ottev\AppData\Roaming\marktext\images\2022-12-01-15-37-58-image.png)Figure 1

## Adding objects to the state saver for rotation and position

Not all objects have to be saved. A wall that stays in the same place does not have to be saved. To save the position and rotation of the object do the following:

1. Click the plus sign under Unity Game Objects (Figure 2) and drag the wanted object into the slot that has just been added

![](C:\Users\ottev\AppData\Roaming\marktext\images\2022-12-01-15-45-17-image.png)

Figure 2

2. Add the saveObject script to the object that has been put into the list. Also give the object an unique ID. (ID's can't be randomly generated during runtime since all changes that are made during runtime will be reset...)

## Saving other data

If other data such as a name or level progression needs to be saved, it can be added by expanding on the gameData.

1. In the gameData.cs file, add the data as a property and don't forget to also add this in the constructor.

2. If the constructor is updated, update the creation of the gameData in the saveObjects function in the state saver

3. If the newly added data has to be used upon retrieving the data, it can be done in the loadObjects function after gameDataLoaded has been initialized.
