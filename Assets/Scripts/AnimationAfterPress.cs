using UnityEngine;

public abstract class AnimationAfterPressAbstract : MonoBehaviour
{
    [SerializeField] protected Transform _firstObject;
    [SerializeField] protected Transform _secondObject;

    public abstract void Enter();
    public abstract void Exit();
}
