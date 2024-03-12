using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.TestTools;

public class MatchFinderTest
{
    /*
    [Test]
    public void ConstructorTest()
    {
        NounDictionary nouns = new NounDictionary();

        Assert.DoesNotThrow(() => { MatchFinder finder = new MatchFinder(nouns.Nouns.Keys); });
    }

    [Test]
    public void TryFindTest()
    {
        NounDictionary nouns = new NounDictionary();
        MatchFinder finder = new MatchFinder(nouns.Nouns.Keys);
        string letters = "рокот";
        int index = 1;
        IEnumerable<string> results;
        bool isFound = finder.TryFind(letters, index, out results);


        Assert.IsTrue(isFound);
        Assert.AreEqual(4, results.Count());
    }

    [Test]
    public void TryFindFailTest()
    {
        NounDictionary nouns = new NounDictionary();
        MatchFinder finder = new MatchFinder(nouns.Nouns.Keys);
        string letters = "щъяюа";
        int index = 3;
        bool isFound = finder.TryFind(letters, index, out _);

        Assert.IsFalse(isFound);
    }

    [Test]
    public void TryFindTest2()
    {
        Assert.AreEqual(2, Setup(0).Count());
        Assert.AreEqual(3, Setup(4).Count());
        Assert.AreEqual(5, Setup(2).Count());
        Assert.Throws<ArgumentOutOfRangeException>(() => Setup(5));
    }

    [Test]
    public void TryFindTest3()
    {
        Assert.AreEqual(0, Setup2(0).Count());
        Assert.AreEqual(1, Setup2(1).Count());
        Assert.AreEqual(1, Setup2(2).Count());
        Assert.AreEqual(1, Setup2(3).Count());
        Assert.AreEqual(0, Setup2(4).Count());
    }

    private IEnumerable<string> Setup(int index)
    {
        NounDictionary nouns = new NounDictionary();
        MatchFinder finder = new MatchFinder(nouns.Nouns.Keys);
        string letters = "рокот";
        IEnumerable<string> results;
        finder.TryFind(letters, index, out results);
        return results;
    }

    private IEnumerable<string> Setup2(int index)
    {
        NounDictionary nouns = new NounDictionary();
        MatchFinder finder = new MatchFinder(nouns.Nouns.Keys);
        string letters = "ысано";
        IEnumerable<string> results;
        finder.TryFind(letters, index, out results);
        return results;
    }
    */
}

