using System;
using UnityEngine;

public class AudioData
{
    private const string MasterVolumeVariableName = nameof(MasterVolumeValue);
    private const string EffectsVolumeVariableName = nameof(EffectVolumeValue);
    private const string MusicVolumeVariableName = nameof(MusicVolumeValue);
    private const float MinimumVolume = 0f;
    private const float MaximumVolume = 1f;

    public float MasterVolumeValue
    {
        get
        {
            if (PlayerPrefs.HasKey(MasterVolumeVariableName))
                return PlayerPrefs.GetFloat(MasterVolumeVariableName);
            else
                return MaximumVolume;
        }

        set
        {
            if (value < MinimumVolume || value > MaximumVolume)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetFloat(MasterVolumeVariableName, value);
            PlayerPrefs.Save();
        }
    }

    public float EffectVolumeValue
    {
        get
        {
            if (PlayerPrefs.HasKey(EffectsVolumeVariableName))
                return PlayerPrefs.GetFloat(EffectsVolumeVariableName);
            else
                return MaximumVolume;
        }

        set
        {
            if (value < MinimumVolume || value > MaximumVolume)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetFloat(EffectsVolumeVariableName, value);
            PlayerPrefs.Save();
        }
    }

    public float MusicVolumeValue
    {
        get
        {
            if (PlayerPrefs.HasKey(MusicVolumeVariableName))
                return PlayerPrefs.GetFloat(MusicVolumeVariableName);
            else
                return MaximumVolume;
        }

        set
        {
            if (value < MinimumVolume || value > MaximumVolume)
                throw new ArgumentOutOfRangeException(nameof(value));

            PlayerPrefs.SetFloat(MusicVolumeVariableName, value);
            PlayerPrefs.Save();
        }
    }
}
