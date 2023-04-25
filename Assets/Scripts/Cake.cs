using UnityEngine;
using UnityEngine.SceneManagement;
using Eiko.YaSDK.Data;

public class Cake : MonoBehaviour
{
    private void Start()
    {
        if (YandexPrefs.GetInt("IsCakeTaken" + SceneManager.GetActiveScene().buildIndex, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<AudioSource>().Play();
        if (collision.transform.tag == "Player")
        {
            YandexPrefs.SetInt("IsCakeTaken" + SceneManager.GetActiveScene().buildIndex, 1);
            YandexPrefs.SetInt("Cakes", YandexPrefs.GetInt("Cakes", 0) + 1);
            gameObject.SetActive(false);
        }
    }
}
