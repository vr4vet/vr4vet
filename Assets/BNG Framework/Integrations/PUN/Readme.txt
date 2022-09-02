This integration package will get you started with PhotonPUN. The included scripts will connect you to a room, sync the player's head, hands, hand animations. The NetworkedGrabbable script will allow you to sync a Grabbables position / rotation.

Check out the online documentation here : https://wiki.beardedninjagames.com/en/Integrations/PhotonPUN2

1. Download and import the PhotonPUN package : https://assetstore.unity.com/packages/tools/network/pun-2-free-119922

2. Make sure to create and enter your AppID in the PhotonServer Settings. (Window -> Photon Unity Networking -> Highlight Server Settings).
  - If you don't already have an app id, you can click on the button next to "App Id Realtime" to create a new one. It is free for up to 20 concurrent users.

3. Extract the PUN.unityPackage file in this directory.

4. Run the included demo scene and connect additional clients.

How it works :

1. The NetworkManager.cs script attempts to connect to the Photon service, and once successful will load the specified Room.

2. Once connected to the room, the "RemotePlayer" object is Network Instantiated. This acts as our representation to other players in the world. This is what holds our NetworkView instead of the VR character. 

3. If the RemotePlayer object belongs to us, we update it with information to send along to the other players, such as our Head Position / Rotation, Animation status, etc. 
If the RemotePlayer is not ours, then we update the representation with the values that were sent over. To achieve a smooth result, we Lerp between our current position and the latest update position.