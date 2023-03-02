using UnityEngine;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class GunLoader : MonoBehaviour
{
    [SerializeField] private GunObject[] _gunObjects;

    private void Start()
    {
        YandexSDK.instance.onInitializeData += Load;
    }

    private void Load()
    {
        foreach (var item in _gunObjects)
        {
            if (!item.IsTaken)
            {
                item.IsTaken = YandexPrefs.GetInt("GunTaken" + item.GunIndex, 0) == 0 ? false : true;
                item.AdWatching = YandexPrefs.GetInt("GunAdWatched" + item.GunIndex, 0);
            }
        }
        YandexSDK.instance.onInitializeData -= Load;
    }
}
