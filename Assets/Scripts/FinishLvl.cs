using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Eiko.YaSDK;

public class FinishLvl : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] private ParticleSystem _finishParticle;
    private int _winCounter;
    private YandexSDK _yandex;

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
                StartCoroutine(OnFinishLvl());
            }
        }
    }

    private IEnumerator OnFinishLvl()
    {
        yield return new WaitForSeconds(1.5f);

        if (_yandex != null)
        {
            _yandex.ShowInterstitial();
        }

        PlayerPrefs.SetInt("CompleteLvl" + SceneManager.GetActiveScene().buildIndex, 1);
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
