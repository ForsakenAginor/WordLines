using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffectPool : MonoBehaviour
{
    private readonly Stack<ScoreEffect> _pool = new();
    private ScoreEffect _prefab;
    private LocalizationLanguages _language;

    public void Init(ScoreEffect prefab, LocalizationLanguages language)
    {
        _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
        _language = language;
    }

    public void SpawnEffect(Vector3 position, int score, int combo)
    {
        ScoreEffect spawned;

        if (_pool.Count <= 0)
        {
            spawned = Instantiate(_prefab, transform);
            spawned.Init(score, position, combo, _language);
        }
        else
        {
            spawned = _pool.Pop();
            spawned.Init(score, position, combo, _language);
            spawned.gameObject.SetActive(true);
            //spawned.enabled = true;
        }

        StartCoroutine(ReturnToPool(spawned));
    }

    private IEnumerator ReturnToPool(ScoreEffect effect)
    {
        WaitForSeconds delay = new(effect.Duration);
        yield return delay;
        effect.gameObject.SetActive(false);
        _pool.Push(effect);
    }
}
