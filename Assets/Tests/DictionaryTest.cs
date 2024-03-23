using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DictionaryTest
{
    public void Test()
    {
        FileLoader loader = new FileLoader();
        NounDictionary nouns = new NounDictionary(loader.Dictionary);

        Assert.True(nouns.Nouns.ContainsKey("???"));
        Assert.True(nouns.Nouns.ContainsKey("??"));
        Assert.True(nouns.Nouns.ContainsKey("??????"));
        Assert.True(nouns.Nouns.ContainsKey("??"));
    }
}
