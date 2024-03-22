using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffectPool : MonoBehaviour
{
    [SerializeField] private ScoreEffect _prefab;

    private readonly Stack<ScoreEffect> _pool = new();

    public void SpawnEffect(Vector3 position, int score, int combo)
    {
        ScoreEffect spawned;

        if (_pool.Count <= 0)
        {
            spawned = Instantiate(_prefab, transform);
            spawned.Init(score, position, combo);
        }
        else
        {
            spawned = _pool.Pop();
            spawned.Init(score, position, combo);
            spawned.gameObject.SetActive(true);
            spawned.enabled = true;
        }

        StartCoroutine(ReturnToPool(spawned));
    }

    private IEnumerator ReturnToPool(ScoreEffect effect)
    {
        WaitForSeconds delay = new(effect.Duration);
        yield return delay;
        effect.gameObject.SetActive(false);
        //effect.enabled = false;
        _pool.Push(effect);
    }
}
