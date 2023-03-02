using Eiko.YaSDK;
using UnityEngine;
using UnityEngine.UI;

public class ActivateShopButton : MonoBehaviour
{
    private Button _btn;
    [SerializeField] GameObject _mainLayer;
    [SerializeField] GameObject _shopLayer;

    private void Start()
    {
        _btn = GetComponent<Button>();

        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onInitializeData += Activate;
        }
        else
        {
            _btn.onClick.AddListener(OpenShopLayer);
        }
    }

    public void Activate()
    {
        YandexSDK.instance.onInitializeData -= Activate;
        _btn.onClick.AddListener(OpenShopLayer);
    }

    public void OpenShopLayer()
    {
        _shopLayer.SetActive(true);
        _mainLayer.SetActive(false);
    }
}
