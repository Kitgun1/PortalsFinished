using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;

public class CakeLoader : MonoBehaviour
{
    [SerializeField] private Text _cakeText;

    private void Start()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onDataRecived += LoadCake;
        }
        else
        {
            _cakeText.text = YandexPrefs.GetInt("Cakes", 0).ToString();
        }
    }

    private void LoadCake(GetDataCallback callback)
    {
        _cakeText.text = YandexPrefs.GetInt("Cakes", 0).ToString();
    }
}
