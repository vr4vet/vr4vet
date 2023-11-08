# Tablet

This repository contains the tablet prefab project which is used in all Virtual Internship apps.

# Importing the prefab
1. In Unity, right click on the project panel and then Import Packages -> Custom Package
2. Select the Tablet prefab you downloaded.
3. Now you have a new folder “Tablet” with some subfolders.
4. Drag “Tablet” prefab from Tablet -> Prefabs folder to your scene.

![Adding custom package in Unity](/ReadMePictures/360video-readme-01.png)

# Skill manager "FerdighetManager"

Expand the tablet object in the hierarchy window and under Managers choose FerdighetManager (NOR Ferdighet = ENG Skill). 
Now you can define the number of skills, add new skills, and their descriptions in the inspector. 
For each skill, you need to define the name of the skill and provide a short description.

![Skill manager screenshot](/ReadMePictures/Tablet-readme-02.png)

# Navigation manager "NavigationManager"

In order to use the navigation system, add the player to NavigationManager. You can do this either with assign the player in the inspector of the NavigationManager (which is under Tablet -> Manager) or you can add the “Player” tag to your player.
If you use XR plugin, you can add XR Rig to the player field.

Remember to bake the navigation areas. 
Go to Window -> AI -> Navigation and then press the bake button. Remember to check the areas where the player can walk or teleport on. The static option can be found in the inspector window of each object. Do the same for the obstacles in your scene.

![Navigation manager screenshot](/ReadMePictures/Tablet-readme-03.png)

# Tasks

## Creating a new task "Oppgave"

For creating a new task, you need a new “Task” object which is in the prefabs folder and it should be added to the scene.
If you have two tasks, then you need two “Task” objects in your scene.

![Task objects in a Unity scene screenshot](/ReadMePictures/Tablet-readme-04.png)

## Activities of tasks: connecting to navigation and points

Every task can have several activities (Aktiviteter) that can be defined in the “Oppgave” component. Activities are components of tasks. These components allow, for example, to track if the user if performing the Task correctly (completing all activities or completing the activities in the right order). Activities can also be used to create complex tasks that may contain repeated activities.

In addition, you need to assign the target object of this task. The navigation system will use this target to navigate to this task.
This target should be destroyed or inactive only when all activities of this task are completed.

![Task object Unity inspector screenshot](/ReadMePictures/Tablet-readme-05.png)

Prerequisite will be assigned when there is a prerequisite task for this task, and it should be done before this one. 
*Note:* The "prerequisite task" feature is in beta and it may happen some small bugs! If you find bugs, please, create issues.

# Points

## Generating the point file

The point file (FerdighetPoenger.txt) contains the maximum point that every activity can give when completed. The points for each activity are aggregared from one or multiple activities. There is a tool to generate this file automatically. You can find the “Ferdighet Poeng Generator” under “IMTEL Tools” in Unity menu.

1. Go to the IMTEL Tools in the Unity menu
2. Create a backup of the existing file before generating a new  file. The backup file will be created in same folder and can be recovered again.
3. After the file is created you need open it in a text editor and enter the points after “:” and before “>” of each activity.

*Note:* The structure of this file is important, so just fill it like the picture bellow. Also, you need to generate it again each time you add a new skill, task or activity.
*Note:* Generating a new point file will erase all numbers in the existing file

![Generating the point file in Unity screenshot](/ReadMePictures/Tablet-readme-06.png)

## Example
In the example below, you can see two skills: "Accuracy" and "Speed". You can also see two tasks "HMS" and "Find Hammer".
Task "HMS" contains four activities. The are listed under both skills.
The "Find the helmet" activity can give a max of 20 points for the Accuracy of the player and 15 points for the Speed.

![Point file example screenshot](/ReadMePictures/Tablet-readme-07.png)

## How can we give point to an activity?

Now that we have the maximum points, it is time to give some points to the player in the play time. You can give point to the player by using this code:

`FerdighetManager.ferdighetManager.AddPoeng("Find  the  helmet",  "Accuracy",  20);`

This code line will give 20 points to the activity “Find the helmet” for the players Accuracy.

## How can we give feedback for a skill?

You can give feedback to the performance of the player. The feedback can be given for each skill. This feedback will be shown as the skill description. The feedback can and should be different based on how well the player has performed on the particular skill. For eaxmple, if in all tasks the player can collect max 50 points in accuracy, the feedback can be the following:
- 0-15 points: "Accuracy is an important skill in this workspace, because ... Your accuracy has been below average. You can try to improve your accuracy in the next play."
- 15-35 poitns: "Your accuracy has been average in the tasks you performed. ... "
- 36-50 points: "Your accuracy has been above average in the tasks you performed! ... "

Use this code:

`FerdighetManager.ferdighetManager.GiveFeedback("Accuracy",  "Your accuracy has been above average in the tasks you performed!");`

This code will change the description of accuracy to “Your accuracy has been above average in the tasks you performed!”.

# Hints

Remember you need add the tablet namespace to your .cs file.

`using  Tablet;`

Make sure you have an Audio Listener in your scene, otherwise you can hear any sound including the tablet sounds. Usually it will be added to the camera.
