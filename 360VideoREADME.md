# 360VideoPlayer

This repository contains the 360 degree video player prefabs project which is used in all virtual practice apps. This can be used in all VR project that is developed with XR Plugin. It can be used in OpenVR too with some small code changing.

# Importing the prefab
1. In Unity, right click on the project panel and then Import Packages -> Custom Package
2. Select the 360VideoPlayer prefab you downloaded.
3. Now you have a new folder “360VideoPlayer” with some subfolders.
4. Drag “VideoManager” prefab from 360VideoPlayer -> Prefabs folder to your scene.

![Adding custom package in Unity](/uploads/ddafa2585af85466f9b8ab7fa1fa518d/360video-readme-01.png)

# Assigning the player
1. For adding a new “VideoObject” that will play a 360-degree video, you need to add a new “VideoObject” prefab to your scene. 
2. Then select it and under the “Video Object” component, assign your video in the “Video Clip” field.

![Assigning a player in Unity](/uploads/9365527bf3224e9cdf01124f1579012a/360video-readme-02.png)

# 3D Layout setting for matching your video layout
Matching the 3D layout setting to the video layout settings will allow to display your video correctly. 

1. Select the correct “3D Layout” under the Skybox/PanoramicBeta shader. This shader is already assigned to the material “VideoSkyBox” which is located on 360VideoPlayer -> Materials.
2. Select VideoSkyBox on the project window and then choose the 3D layout from the rollover menu in the inspector window. (See the picture)

*Note:* The size of the render texture (in this prefab, it is named 4kRenderTexture) should be the same as your video size. As default, it is set to 3840 x 1920.

![3D layout and render texture in Unity](/uploads/8edbbef6100c1919afbce24618b64b31/360video-readme-03.png)

# 3D Layout alternatives
Choose the correct render texture in the way that is displayed on the figures bellow.

### None

![3D layout none](/uploads/b6e9da771380c4d2464f1e49903ffb30/360video-readme-04.png)

### Over Under

![3D layout over under](/uploads/72c9207a0390a004fa3152743c199d84/360video-readme-05.png)

### Side by Side

![3D layout side by side](/uploads/695c973e08f516187967207469cea4c3/360video-readme-06.png)
