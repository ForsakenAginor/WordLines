using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DictionaryTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Test()
    {/*
        YaHeZe loader = new YaHeZe();
        NounDictionary nouns = new NounDictionary(loader.Dictionary);

        Assert.True(nouns.Nouns.ContainsKey("АР"));
        Assert.True(nouns.Nouns.ContainsKey("АД"));
        Assert.True(nouns.Nouns.ContainsKey("КОТ"));
        Assert.True(nouns.Nouns.ContainsKey("АБАЖУР"));
        Assert.True(nouns.Nouns.ContainsKey("ЯК"));*/
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DictionaryTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
