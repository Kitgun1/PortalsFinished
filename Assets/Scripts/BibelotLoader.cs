using Eiko.YaSDK;
using Eiko.YaSDK.Data;
using UnityEngine;

namespace DefaultNamespace
{
    public class BibelotLoader : MonoBehaviour
    {
        [SerializeField] private BibelotObject[] _bibelotObjects;

        private void Start()
        {
            if (YandexSDK.instance.IsFirstOpen)
            {
                YandexSDK.instance.onDataRecived += Load;
            }
            else
            {
                foreach (BibelotObject item in _bibelotObjects)
                {
                    if (!item.IsTaken)
                    {
                        item.IsTaken = YandexPrefs.GetInt("BibelotTaken" + item.BibelotIndex, 0) != 0;
                    }
                }
            }
        }

        private void Load(GetDataCallback callback)
        {
            foreach (var item in _bibelotObjects)
            {
                if (!item.IsTaken)
                {
                    item.IsTaken = YandexPrefs.GetInt("BibelotTaken" + item.BibelotIndex, 0) != 0;
                    //item.Janov = YandexPrefs.GetInt("GunAdWatched" + item.BibelotIndex, 0);
                }
            }
            YandexSDK.instance.onDataRecived -= Load;
        }
    }
}