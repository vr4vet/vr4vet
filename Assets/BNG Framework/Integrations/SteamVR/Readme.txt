If you require Steam device support (such as Valve Index or HTC Cosmos) then the SteamVR SDK may be a good option for you. The SteamVR SDK uses it's own Action system for inputs. This integration package will help you map actions to inputs that VRIF can read.

Some devices such as the original HTC Vive and WMR devices may be able to utilize the OpenVR Desktop package in Unity 2019.4 (it is no longer compatible with Unity 2020). 

If you are using a Steam device, but do not wish to use the SteamVR SDK, you may also want to consider using the OpenXR (beta) plugin : https://docs.unity3d.com/Packages/com.unity.xr.openxr@0.1/manual/index.html

If you want to give SteamVR a go, this integration package can get you started. It includes SteamVR bindings you can use to map SteamVR actions to raw inputs that the InputBridge can read. 

1. First install the SteamVR asset from the asset store : https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647

2. Extract the included SteamVR integration package (SteamVR.unityPackage)

3. Click "Yes" to import the provided Input Actions / Bindings into SteamVR. You can integrate them with your own bindings, or overwrite them completely.

4. Add "STEAM_VR_SDK" (no quotes) to your Scripting Define Symbols (Edit -> Project Settings -> Other -> Scripting Define Symbols).
  - Alternatively, you can go to Window -> VRIF and check "SteamVR Integration"

5. On Player's InputBridge object, make sure "SteamVR" is selected as the input source.

6. On the Player object, add the "SteamVR_ActivateActionSetOnLoad" and make sure the Action Set is set to the VRIF actions that were imported

- If you aren't getting any inputs, make sure yours keys are all mapped by going to Window -> SteamVR Input -> "Open Binding UI" and verify all inputs have been mapped.

Note : This essentially maps SteamVR Actions such as "Grip", "Trigger", etc. so that the InputBridge can convert it to be used as raw input. It's not how SteamVR's input system is intended to be used, but is one way to get raw input from certain devices until Unity gets full OpenVR or OpenXR support. If you are looking to publish on Steam with the SteamVR SDK, you should consider renaming your actions to "Move", "Fire", etc.

- Check out InputBridge.cs to see how Steam Actions are bound to inputs
