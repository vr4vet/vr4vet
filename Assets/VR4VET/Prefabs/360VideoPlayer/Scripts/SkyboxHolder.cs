/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using UnityEngine;

[CreateAssetMenu]
public class SkyboxHolder: ScriptableObject
{

	[SerializeField] Material defaultSkyboxMaterial;
	[SerializeField] Material videoPlayerSkyboxMaterial;


	/// <summary>
	/// adds video renderer to active sceene skybox
	/// </summary>
	public void applyVideoTextureToSkybox()
	{
		RenderSettings.skybox = videoPlayerSkyboxMaterial;
	}

	/// <summary>
	/// removes video renderer to active sceene skybox
	/// </summary>
	public void applyDefaultSkybox()
	{
		RenderSettings.skybox = defaultSkyboxMaterial;
	}
}
