using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NounsDictionaryTests
{
    // A Test behaves as an ordinary method
    [Test]
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

    [Test]
    public void FilterBySizeTest()
    {
        NounDictionary nouns = new NounDictionary();
        int maxSize = BoardConfig.Width > BoardConfig.Height ? BoardConfig.Width : BoardConfig.Height;
        string longNoun = nouns.Nouns.Keys.Where(o => o.Length > maxSize).First();
        string normaLengthNoun = nouns.Nouns.Keys.Where(o => o.Length == maxSize).First();
        nouns.FilterBySize(maxSize);

        Assert.False(nouns.Nouns.ContainsKey(longNoun));
        Assert.True(nouns.Nouns.ContainsKey(normaLengthNoun));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NounsDictionaryTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
