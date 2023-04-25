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
            YandexSDK.instance.onDataRecived += WaitToInitYandex;
        }
        else
        {
            _PlayBtn.interactable = true;
        }
    }

    private void WaitToInitYandex(GetDataCallback callback)
    {
        _PlayBtn.interactable = true;
        YandexSDK.instance.onDataRecived -= WaitToInitYandex;
    }
}
