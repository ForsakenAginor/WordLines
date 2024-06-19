using System;
using UnityEngine;

public class TutorialData
{
    private const string TutorialVariableName = nameof(IsTutorialCompleted);

    public bool IsTutorialCompleted
    {
        get
        {
            if (PlayerPrefs.HasKey(TutorialVariableName))
                return Convert.ToBoolean(PlayerPrefs.GetInt(TutorialVariableName));
            else
                return false;
        }

        set
        {
            PlayerPrefs.SetInt(TutorialVariableName, Convert.ToInt32(value));
            PlayerPrefs.Save();
        }
    }
}
