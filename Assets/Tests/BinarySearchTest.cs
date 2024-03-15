using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class BinarySearchTest
{
    [Test]
    public void Test()
    {
        NounDictionary nounDictionary = new NounDictionary();
        List<string> collection = new List<string>(nounDictionary.Nouns.Keys);
        string firstWord = "АААА";
        string secondWord = "А";
        string thirdWord = "ААААААААААААААААААААААААААААААААААААААААААААААААА";
        string fourthWord = "АУЛ";
        string fifthWord = "ПЕРЕД";
        string sixthWord = "ПЕРЕДАЧА";
        string aaa = "ПИАЛ";

        Assert.False(BinarySeacrh.Contains(aaa, collection));
        Assert.False(BinarySeacrh.Contains(firstWord, collection));
        Assert.False(BinarySeacrh.Contains(secondWord, collection));
        Assert.False(BinarySeacrh.Contains(thirdWord, collection));
        Assert.True(BinarySeacrh.Contains(fourthWord, collection));
        Assert.True(BinarySeacrh.Contains(fifthWord, collection));
        Assert.True(BinarySeacrh.Contains(sixthWord, collection));        
    }

    [Test]
    public void Test2()
    {
        NounDictionary nounDictionary = new NounDictionary();
        List<string> collection = new List<string>(nounDictionary.Nouns.Keys);
        string firstWord = "АААА";
        string secondWord = "А";
        string thirdWord = "ААААААААААААААААААААААААААААААААААААААААААААААААА";
        string fourthWord = "АУЛ";
        string fifthWord = "ПЕРЕД";
        string sixthWord = "ПЕРЕДАЧА";

        Assert.False(collection.Contains(firstWord));
        Assert.False(collection.Contains(secondWord));
        Assert.False(collection.Contains(thirdWord));
        Assert.True(collection.Contains(fourthWord));
        Assert.True(collection.Contains(fifthWord));
        Assert.True(collection.Contains(sixthWord));
    }
}
