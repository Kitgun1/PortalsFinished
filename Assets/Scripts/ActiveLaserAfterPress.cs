using UnityEngine;

[RequireComponent(typeof(Laser))]
public class ActiveLaserAfterPress : AnimationAfterPressAbstract
{
    private Laser _laser;

    private void Start()
    {
        _laser = GetComponent<Laser>();
    }

    public override void Enter()
    {
        _laser.IsActive = true;
    }

    public override void Exit()
    {
        _laser.IsActive = false;
    }
}
