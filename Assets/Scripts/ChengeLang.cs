using UnityEngine;
using Lean.Localization;
using Eiko.YaSDK;

public class ChengeLang : MonoBehaviour
{
    private void Start()
    {
        LeanLocalization.SetCurrentLanguageAll(YandexSDK.instance.Lang);
        Debug.Log(YandexSDK.instance.Lang);
    }
}
