using UnityEngine;
using DG.Tweening;

public class TutorialLetterMover : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _duration;

    private void Start()
    {
        int infinityLoops = -1;
        transform.DOLocalMove(_targetPosition, _duration).SetEase(Ease.Linear).SetLoops(infinityLoops);
    }
}
