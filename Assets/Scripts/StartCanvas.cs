using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private Button _PlayBtn;

    private void Start()
    {
        StartCoroutine(WaitToInitYandex());
    }

    private IEnumerator WaitToInitYandex()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            yield return YandexPrefs.Init();
        }
        YandexSDK.instance.IsFirstOpen = false;
        _PlayBtn.interactable = true;
    }
}
