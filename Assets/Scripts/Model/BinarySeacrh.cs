using System.Collections.Generic;
using System.Linq;

public class BinarySeacrh
{
    public static bool Contains(string word, IEnumerable<string> collection)
    {
        char[] alfabet = Letters.GetAlfabet().ToArray();
        List<string> tempCollection = new List<string>(collection);
        bool isFound = false;
        int halfIndex;
        int half = 2;
        string comparedWord;
        int[] wordIndexes = new int[word.Length];
        bool isWorking = true;

        for (int i = 0; i < wordIndexes.Length; i++)
            wordIndexes[i] = alfabet.ToList().IndexOf(word[i]);

        while (isWorking)
        {
            halfIndex = tempCollection.Count() / half;
            comparedWord = tempCollection[halfIndex];

            if (word == comparedWord)
            {
                isWorking = false;
                isFound = true;
            }
            else if(halfIndex == 0)
            {
                isWorking = false;
                isFound = false;
            }
            else
            {
                int[] comparedWordIndexes = new int[comparedWord.Length];

                for (int i = 0; i < comparedWord.Length; i++)
                    comparedWordIndexes[i] = alfabet.ToList().IndexOf(comparedWord[i]);

                int iterations = word.Length < comparedWord.Length ? word.Length - 1 : comparedWord.Length - 1;

                for (int i = 0; i <= iterations; i++)
                {
                    if (wordIndexes[i] < comparedWordIndexes[i])
                    {
                        tempCollection = tempCollection.SkipLast(halfIndex).ToList();
                        break;
                    }
                    else if (wordIndexes[i] > comparedWordIndexes[i])
                    {
                        tempCollection = tempCollection.Skip(halfIndex).ToList();
                        break;
                    }
                    else if( i == iterations)
                    {
                        if(word.Length < comparedWord.Length)
                            tempCollection = tempCollection.SkipLast(halfIndex).ToList();
                        else
                            tempCollection = tempCollection.Skip(halfIndex).ToList();
                    }
                }
            }
        }

        return isFound;
    }
}
