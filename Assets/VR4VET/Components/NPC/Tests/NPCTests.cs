using NUnit.Framework;
using Newtonsoft.Json;
using System.IO;

namespace NPCTests
{

    public class DialogueTreeTests
    {
        private DialogueTree dialogueTreeUnderTest = new();

        public void InstanceNotNullTest()
        {
            Assert.NotNull(dialogueTreeUnderTest);
        }
    }
    public class DialogueBoxControllerTests
    {
        private DialogueBoxController dialogueBoxControllerUnderTest;

        [SetUp]
        void Initialize()
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
            DialogueTree dialogueTree = JsonConvert.DeserializeObject<DialogueTree>(File.ReadAllText("../Resources/data.json"));
            dialogueBoxControllerUnderTest.StartDialogue(dialogueTree, 0, "Test");

            Assert.AreEqual(dialogueBoxControllerUnderTest.GetNameText(), "Test");
        }

    }
}
