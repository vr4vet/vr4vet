using NUnit.Framework;
using System.IO;
using UnityEngine;

// Tests for the presence of required character models and animations
public class AssetsTests
{
	private readonly string basePath = Path.Combine(Application.dataPath, "VR4VET", "Components", "NPC", "Animations");
	private readonly string gesturesPath = Path.Combine(Application.dataPath, "VR4VET", "Components", "NPC", "Animations", "Gestures");
	private readonly string locomotionPath = Path.Combine(Application.dataPath, "VR4VET", "Components", "NPC", "Animations", "Locomotion");
	private readonly string modelsPath = Path.Combine(Application.dataPath, "VR4VET", "Components", "NPC", "characterModels");

	// Checks that the animation controller is present
	[Test]
	public void GetHumanoidAnimationController()
	{
		string humanoidControllerFile = "NPCHumanoidAnimationController.controller";
		string fullPath = Path.Combine(basePath, humanoidControllerFile);
		Assert.IsTrue(File.Exists(fullPath), $"File '{humanoidControllerFile}' is missing in the Animations folder.");
	}

	// Checks that the gestures are present
	[Test]
	public void GetGestures()
	{
		string[] expectedFiles = new string[]
		{
			"Talking - Conversation.anim",
			"Thoughtful Head Nod.anim",
			"Waving - Both hands in the air.anim",
			"X Bot Talking.fbx",
			"X Bot@Thoughtful Head Nod 1.fbx",
			"Y Bot@Talking.fbx",
			"Y Bot@Waving.fbx"
		};

		foreach (var fileName in expectedFiles)
		{
			string fullPath = Path.Combine(gesturesPath, fileName);
			Assert.IsTrue(File.Exists(fullPath), $"File '{fileName}' is missing in the Gestures folder.");
		}
	}

	// Tests that the locomotion files exist
	[Test]
	public void GetLocomotion()
	{
		string[] expectedFiles = new string[]
		{
			"Idle (stand still) Y.anim",
			"Idle Y.anim",
			"Left Strafe Walk Y.anim",
			"Left Strafe Walking Y.anim",
			"Neutral Idle Y.anim",
			"Right Strafe Walk Y.anim",
			"Right Strafe Walking Y.anim",
			"Running Backward_Y.anim",
			"Running_Y.anim",
			"Walk Strafe Left Y.anim",
			"Walk Strafe Right Y.anim",
			"Walking Backwards Y.anim",
			"Walking_Y.anim"
		};

		foreach (var fileName in expectedFiles)
		{
			string fullPath = Path.Combine(locomotionPath, fileName);
			Assert.IsTrue(File.Exists(fullPath), $"File '{fileName}' is missing in the Locomotion folder.");
		}
	}

	// Checks that the character models exist
	[Test]
	public void GetCharacterModels()
	{
		string[] expectedFiles = new string[]
		{
			"Ch01_nonPBR.fbx",
			"Ch11_nonPBR.fbx",
			"Ch12_nonPBR.fbx",
			"Ch16_nonPBR.fbx",
			"Ch17_nonPBR.fbx",
			"Ch21_nonPBR.fbx",
			"Ch22_nonPBR.fbx",
			"Ch27_nonPBR.fbx"
		};

		foreach (var fileName in expectedFiles)
		{
			string fullPath = Path.Combine(modelsPath, fileName);
			Assert.IsTrue(File.Exists(fullPath), $"File '{fileName}' is missing in the characterModels folder.");
		}
	}
}