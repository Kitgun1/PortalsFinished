using System;
using System.Collections;
using Eiko.YaSDK;
using Eiko.YaSDK.Data;
using UnityEngine;

namespace EikoYandex.Scripts
{
    public class StartUp : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return YandexSDK.instance.InitDataPrefs();

            PlayerPrefs.DeleteKey("ActiveGun");
            PlayerPrefs.DeleteKey("ActiveBibelot");
            for (int i = 0; i < 15; i++)
            {
                PlayerPrefs.DeleteKey("GunTaken" + i);
                PlayerPrefs.DeleteKey("BibelotTaken" + i);
            }
        }
    }
}