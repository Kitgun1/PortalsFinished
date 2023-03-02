using UnityEngine;

public class ControlsSwitcher : MonoBehaviour
{
    private CheckWebGLPlatform _platform;
    [SerializeField] private GameObject _desktop;
    [SerializeField] private GameObject _mobile;

    private void Start()
    {
        _platform = GetComponent<CheckWebGLPlatform>();

        if (_platform.CheckIfMobile())
        {
            _desktop.SetActive(false);
            _mobile.SetActive(true);
        }
        else
        {
            _desktop.SetActive(true);
            _mobile.SetActive(false);
        }
    }
}
