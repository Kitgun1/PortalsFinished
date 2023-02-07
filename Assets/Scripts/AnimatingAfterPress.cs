using DG.Tweening;
using UnityEngine;

public class AnimatingAfterPress : AnimationAfterPressAbstract
{
    private OffPortalOnMove _offPortal;

    private void Start()
    {
        _offPortal = GetComponent<OffPortalOnMove>();
    }

    public override void Enter()
    {
        transform.DOMove(_firstObject.position, 1f);
    }

    public override void Exit()
    {
        transform.DOMove(_secondObject.position, 1f);

        if (_offPortal != null && _offPortal.portal != null)
        {
            _offPortal.portal.GoToStartPos();
        }
    }
}
