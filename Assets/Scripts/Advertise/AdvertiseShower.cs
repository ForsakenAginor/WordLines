using Agava.YandexGames;
using System;
using UnityEngine;

public class AdvertiseShower
{
    private readonly Silencer _silencer;

    public AdvertiseShower(Silencer silencer)
    {
        _silencer = silencer != null ? silencer : throw new ArgumentNullException(nameof(silencer));
    }

    public void ShowAdvertise()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        AudioListener.volume = 0f;
        InterstitialAd.Show(null, OnCloseAdvertise);
    }

    private void OnCloseAdvertise(bool nonmatterValue)
    {
        AudioListener.pause = false;
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        _silencer.SetGameState(Time.timeScale, AudioListener.volume, AudioListener.pause);
    }
}
