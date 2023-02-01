using DG.Tweening;
using UnityEngine;

public class AnimatingAfterPress : MonoBehaviour
{
    [SerializeField] private Transform _firstObject;
    [SerializeField] private Transform _secondObject;

    public void Enter()
    {
        transform.DOMove(_firstObject.position, 1f);
    }

    public void Exit()
    {
        transform.DOMove(_secondObject.position, 1f);
    }
}
