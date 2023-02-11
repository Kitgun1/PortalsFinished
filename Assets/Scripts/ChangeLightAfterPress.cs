using UnityEngine;

public class ChangeLightAfterPress : AnimationAfterPressAbstract
{
    [SerializeField] private GameObject _light1;
    [SerializeField] private GameObject _light2;
    [SerializeField] private int _countToActive;
    private int _pressCount;

    private Turret _turret;

    private void Start()
    {
        _turret = GetComponent<Turret>();
    }

    public override void Enter()
    {
        _pressCount++;
        if (_pressCount >= _countToActive)
        {
            if (_turret != null)
            {
                _turret.Off();
            }
            _light1.SetActive(false);
            _light2.SetActive(true);
        }
    }

    public override void Exit()
    {
        _pressCount--;
        if (_pressCount < _countToActive)
        {
            if (_turret != null)
            {
                _turret.On();
            }
            _light1.SetActive(true);
            _light2.SetActive(false);
        }
    }
}
