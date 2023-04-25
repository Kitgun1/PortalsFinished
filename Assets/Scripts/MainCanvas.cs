using System;
using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Text _timerText;
    private DateTime dt_TimeStarted;
    private double dt_CurTime;
    private FinishLvl _finishLvl;
    [SerializeField] private GameObject _clueBtnPC;
    [SerializeField] private Button _clutBtnMobile;
    [SerializeField] private GameObject indicator;
    private bool _isAdShowen;

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
            _clueBtnPC.SetActive(false);
        }
        dt_TimeStarted = DateTime.Now;
        _finishLvl = FindObjectOfType<FinishLvl>();

        YandexSDK.instance.onRewardedAdOpened += ActivateAd;
        YandexSDK.instance.onRewardedAdClosed += DiactivateAd;
    }

    public void ActivateClue(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= ActivateClue;
        YandexSDK.instance.onRewardedAdClosed -= DiactivateClueAd;

        _finishLvl.EnableWallHack();

        if (indicator != null)
        {
            indicator.SetActive(true);
        }
    }

    public void DiactivateClueAd(int i)
    {
        YandexSDK.instance.onRewardedAdReward -= ActivateClue;
        YandexSDK.instance.onRewardedAdClosed -= DiactivateClueAd;
    }

    public void WatchAdForClue()
    {
        YandexSDK.instance.onRewardedAdReward += ActivateClue;
        YandexSDK.instance.onRewardedAdClosed += DiactivateClueAd;
        YandexSDK.instance.ShowRewarded("clue");
    }

    private void Update()
    {
        if (!_isAdShowen)
        {
            dt_CurTime = (DateTime.Now - dt_TimeStarted).TotalSeconds;
            DateTime ctime = new DateTime(1, 1, 1).AddSeconds(dt_CurTime);
            string time = ctime.ToString("mm:ss");
            _timerText.text = time;

            if (Input.GetKeyDown(KeyCode.B))
            {
                WatchAdForClue();
            }
        }
    }

    private void ActivateAd(int i)
    {
        _isAdShowen = true;
    }

    private void DiactivateAd(int i)
    {
        _isAdShowen = false;
    }

    public void End()
    {
        DateTime ctime = new DateTime(1, 1, 1).AddSeconds(dt_CurTime);
        string time = ctime.ToString("mm:ss");
        string timeString = YandexPrefs.GetString("LvlTimer" + SceneManager.GetActiveScene().buildIndex, "00:00");
        print(timeString);
        DateTime savedTime = DateTime.ParseExact(timeString, "mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        Debug.Log(savedTime);
        if (ctime < savedTime)
        {
            YandexPrefs.SetString("LvlTimer" + SceneManager.GetActiveScene().buildIndex, timeString);
        }
    }

    private void SetMobileControls()
    {
        BlueButton.onClick.AddListener(_player.GetComponent<PortalGun>().ShootBlue);
        RedButton.onClick.AddListener(_player.GetComponent<PortalGun>().ShootRed);
        JumpButton.onClick.AddListener(_player.Jump);
        PauseButton.onClick.AddListener(_player.Pause);
        SkipLvl.onClick.AddListener(_player.SkipLvl);
        _clutBtnMobile.onClick.AddListener(WatchAdForClue);
        TakeButton.onClick.AddListener(_player.Carring);
    }
}
