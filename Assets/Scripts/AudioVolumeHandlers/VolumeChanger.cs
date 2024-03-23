using System;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
    private VolumeHolder[] _audioSources;

    private void Awake()
    {
        _audioSources = FindObjectsOfType<VolumeHolder>();

        if (_audioSources == null)
            throw new Exception("Can't find any VolumeHolder");
    }

    public void ChangeGlobalVolume(float targetPercent)
    {
        targetPercent = Mathf.Clamp01(targetPercent);

        for (int i = 0; i < _audioSources.Length; i++)        
            _audioSources[i].ChangeVolume(targetPercent);        
    }
}
