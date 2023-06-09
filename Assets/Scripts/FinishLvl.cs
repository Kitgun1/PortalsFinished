using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class FinishLvl : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] private ParticleSystem _finishParticle;
    private int _winCounter;
    private YandexSDK _yandex;
    public bool _isRewardedLvl;
    public bool _canLoadLvl;
    public GunObject gunObject;
    public RewardedPanel rewardedPanel;
    public Outline outline;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _yandex = YandexSDK.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            _winCounter++;
            if (_winCounter == 1)
            {
                _audio.Play();
                _finishParticle.Play();
                other.GetComponent<PlayerControls>().End();
                StartCoroutine(OnFinishLvl());
            }
        }
    }

    public void StartFinishLvl()
    {
        StartCoroutine(OnFinishLvl());
    }

    public void SetLoadable()
    {
        _canLoadLvl = true;
    }

    public void EnableWallHack()
    {
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
    }

    private IEnumerator OnFinishLvl()
    {
        yield return new WaitForSeconds(1.5f);

        if (_yandex != null)
        {
            if (YandexPrefs.GetInt("IsAd", 0) == 0)
            {
                _yandex.ShowInterstitial();
            }
        }

        YandexPrefs.SetInt("CompleteLvl" + SceneManager.GetActiveScene().buildIndex, 1);

        if (_isRewardedLvl && !gunObject.IsTaken && gunObject != null)
        {
            rewardedPanel.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            rewardedPanel.OpenPanel(gunObject, this);
            yield return new WaitUntil(() => _canLoadLvl == true);
        }

        LvlTransition.Instance.CloseLvl();

        yield return new WaitForSeconds(1.5f);
        
        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
