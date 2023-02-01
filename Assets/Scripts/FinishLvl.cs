using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishLvl : MonoBehaviour
{
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            _audio.Play();
            StartCoroutine(OnFinishLvl());
        }
    }

    private IEnumerator OnFinishLvl()
    {
        yield return new WaitForSeconds(1.5f);

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
