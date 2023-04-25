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
        }
    }
}