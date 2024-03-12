namespace Tests
{
    [TestClass]
    public class NounsDictionaryTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            NounDictionary nouns = new NounDictionary();
            string firstWord = nouns.Nouns.Keys.First();
            string firstWordDefinition = nouns.Nouns.Values.First();
            string lastWord = nouns.Nouns.Keys.Last();
            string lastWordDefinition = nouns.Nouns.Values.Last();

            Assert.AreEqual(firstWord, "абажур");
            Assert.AreEqual(firstWordDefinition, "м. 1) Часть светильника, обычно в виде колпака, предназначенная для сосредоточения и отражения света и защиты глаз от его воздействия. 2) устар. Козырек, надеваемый на лоб для защиты глаз от воздействия света.");
            Assert.AreEqual(lastWord, "ящурка");
            Assert.AreEqual(lastWordDefinition, "ж. Род небольших ящериц.");
        }
    }
}