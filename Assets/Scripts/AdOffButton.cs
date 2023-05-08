using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class AdOffButton : MonoBehaviour
{
    private void Start()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onDataRecived += Load;
        }
        else
        {
            Load();
        }
    }

    public void Load(GetDataCallback callback = null)
    {
        if (YandexPrefs.GetInt("IsAd", 0) == 0)
        {
            StartCoroutine(StartPurch());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator StartPurch()
    {
        var process = new PurchaseProcess();
        yield return process.InitPurchases();
        GetComponent<Button>().onClick.AddListener(Purchase);
    }

    public void Purchase()
    {
        PurchaseProcess.instance.ProcessPurchase("1", () =>
        {
            YandexPrefs.SetInt("IsAd", 1);
            gameObject.SetActive(false);
        });
    }
}