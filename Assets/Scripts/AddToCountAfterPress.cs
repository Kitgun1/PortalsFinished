using UnityEngine;
using DG.Tweening;

public class AddToCountAfterPress : AnimationAfterPressAbstract
{
    private OffPortalOnMove _offPortal;
    private int _counter;
    [SerializeField] private int _countToDo;

    private void Start()
    {
        _offPortal = GetComponent<OffPortalOnMove>();
    }

    public override void Enter()
    {
        _counter++;

        if (_counter >= _countToDo)
        {
            transform.DOMove(_firstObject.position, 1f);
        }
    }

    public override void Exit()
    {
        _counter--;

        if (_counter < _countToDo)
        {
            transform.DOMove(_secondObject.position, 1f);

            if (_offPortal != null && _offPortal.portal != null)
            {
                _offPortal.portal.GoToStartPos();
            }
        }
    }
}
