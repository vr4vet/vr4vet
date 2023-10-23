using NUnit.Framework;

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
        public DialogueBoxController dialogueBoxControllerUnderTest;

        [Test]
        public void InstanceNotNullTest()
        {
            Assert.NotNull(dialogueBoxControllerUnderTest);
        }

    }
}
