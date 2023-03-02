using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private Button _PlayBtn;

    private void Start()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onInitializeData += WaitToInitYandex;
        }
        else
        {
            _PlayBtn.interactable = true;
        }
    }

    private void WaitToInitYandex()
    {
        _PlayBtn.interactable = true;
        YandexSDK.instance.onInitializeData -= WaitToInitYandex;
    }
}
