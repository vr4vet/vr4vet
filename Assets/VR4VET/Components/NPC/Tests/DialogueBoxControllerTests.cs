using NUnit.Framework;

namespace Tests 
{
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
