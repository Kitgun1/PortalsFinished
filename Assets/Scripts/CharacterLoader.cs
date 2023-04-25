using Eiko.YaSDK;
using Eiko.YaSDK.Data;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    [SerializeField] private CharacterObject[] _charObjects;

    private void Start()
    {
        if (YandexSDK.instance.IsFirstOpen)
        {
            YandexSDK.instance.onDataRecived += Load;
        }
        else
        {
            foreach (var item in _charObjects)
            {
                if (!item.IsTaken)
                {
                    item.IsTaken = YandexPrefs.GetInt("CharTaken" + item.CharIndex, 0) == 0 ? false : true;
                }
            }
        }
    }

    private void Load(GetDataCallback callback)
    {
        foreach (var item in _charObjects)
        {
            if (!item.IsTaken)
            {
                item.IsTaken = YandexPrefs.GetInt("CharTaken" + item.CharIndex, 0) == 0 ? false : true;
            }
        }
        YandexSDK.instance.onDataRecived -= Load;
    }
}
