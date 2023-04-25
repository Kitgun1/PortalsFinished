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
        Debug.Log("000");
        var process = new PurchaseProcess();
        yield return process.InitPurchases();
        Debug.Log("111");
        GetComponent<Button>().onClick.AddListener(Purchase);
    }

    public void Purchase()
    {
        Debug.Log("12");
        PurchaseProcess.instance.ProcessPurchase("1", () => {
            YandexPrefs.SetInt("IsAd", 1);
            gameObject.SetActive(false);
        });
    }
}
