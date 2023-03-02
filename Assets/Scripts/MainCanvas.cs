using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public PlayerControls _player;
    public GameObject MobileControl;
    public Joystick joystick;
    public GameObject SkipLvlPC;
    public Button BlueButton;
    public Button RedButton;
    public Button JumpButton;
    public Button PauseButton;
    public Button SkipLvl;
    public Button TakeButton;
    public Joystick CameraJoystick;
    public BoxCollider2D _collider;
    [SerializeField] private GameObject _desktopMenuBtn;

    private bool _isMobile;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        if (GetComponent<CheckWebGLPlatform>().CheckIfMobile())
        {
            MobileControl.SetActive(true);
            SkipLvlPC.SetActive(false);
            SetMobileControls();
            _desktopMenuBtn.SetActive(false);
        }
    }

    private void SetMobileControls()
    {
        BlueButton.onClick.AddListener(_player.GetComponent<PortalGun>().ShootBlue);
        RedButton.onClick.AddListener(_player.GetComponent<PortalGun>().ShootRed);
        JumpButton.onClick.AddListener(_player.Jump);
        PauseButton.onClick.AddListener(_player.Pause);
        SkipLvl.onClick.AddListener(_player.SkipLvl);
        TakeButton.onClick.AddListener(_player.Carring);
    }
}
