using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private Button _PlayBtn;

    private void Start()
    {
        StartCoroutine(WaitToInitYandex());
    }

    private IEnumerator WaitToInitYandex()
    {
        yield return YandexPrefs.Init();

        _PlayBtn.interactable = true;
    }
}
