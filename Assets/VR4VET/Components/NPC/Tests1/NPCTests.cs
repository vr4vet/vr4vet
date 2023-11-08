using NUnit.Framework;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NPCTests
{
	public class DialogueTreeTests
	{
		private DialogueTree dialogueTreeUnderTest;

		[SetUp]
		public void Initialize()
		{
			try
			{
				dialogueTreeUnderTest = JsonConvert.DeserializeObject<DialogueTree>(File.ReadAllText("Assets/VR4VET/Components/NPC/Resources/data.json"));
			}
			catch (Exception ex)
			{
				Assert.Fail("Expected no exception, but got: " + ex.Message);
			}
		}

		public void InstanceNotNullTest()
		{
			Assert.NotNull(dialogueTreeUnderTest);
		}

		[Test]
		public void TreeStructureTest()
		{
			Assert.NotNull(dialogueTreeUnderTest.sections);
			Assert.IsInstanceOf<DialogueSection[]>(dialogueTreeUnderTest.sections);

			Debug.Log(dialogueTreeUnderTest.sections.Length);

			foreach (DialogueSection section in dialogueTreeUnderTest.sections)
			{
				Assert.IsInstanceOf<bool>(section.endAfterDialogue);
				Assert.IsInstanceOf<string[]>(section.dialogue);





				Assert.IsInstanceOf<BranchPoint>(section.branchPoint);

				Assert.IsInstanceOf<string>(section.branchPoint.question);


				Assert.IsInstanceOf<Answer[]>(section.branchPoint.answers);


				foreach (Answer answer in section.branchPoint.answers)
				{

					Assert.IsInstanceOf<string>(answer.answerLabel);


					Assert.IsInstanceOf<int>(answer.nextElement);
				}
			}
		}
	}
	public class DialogueBoxControllerTests
	{
		private DialogueBoxController dialogueBoxControllerUnderTest;

		[SetUp]
		public void Initialize()
		{
			dialogueBoxControllerUnderTest = new();
		}

		[Test]
		public void InstanceNotNullTest()
		{
			Assert.NotNull(dialogueBoxControllerUnderTest);
		}

		[Test]
		public void StartDialogueTest()
		{
			DialogueTree dialogueTree = JsonConvert.DeserializeObject<DialogueTree>(File.ReadAllText("Assets/VR4VET/Components/NPC/Resources/data.json"));
			dialogueBoxControllerUnderTest.StartDialogue(dialogueTree, 0, "Test");

			Assert.AreEqual(dialogueBoxControllerUnderTest.GetNameText(), "Test");
		}
	}

	public class NPCManagerTests
	{
		private NPCManager npcManagerUnderTest;
		[SetUp]
		public void Initialize()
		{
			npcManagerUnderTest = new();
		}


		public void SetDialogueTreeListTest()
		{
			List<DialogueTree> dialogueTrees = new();
			npcManagerUnderTest.SetDialogueTreeList(dialogueTrees);

			Assert.AreEqual(dialogueTrees, npcManagerUnderTest.GetDialogueTrees());
		}

	}
}
