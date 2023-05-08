using System;
using System.Collections.Generic;
using DefaultNamespace;
using Eiko.YaSDK.Data;
using UnityEngine;

public class ActivateCurrentBibelot : MonoBehaviour
{
    [SerializeField] private CameraBibelot[] _cameraBibelot;
    [SerializeField] private List<BibelotData> _bibelotsData;

    private void Start()
    {
        foreach (CameraBibelot bibelot in _cameraBibelot)
        {
            print($"{bibelot.BibelotObject.BibelotIndex == YandexPrefs.GetInt("ActiveBibelot", -1)}" +
                  $" ~ {bibelot.BibelotObject.BibelotIndex}" +
                  $" ~~ {YandexPrefs.GetInt("ActiveBibelot", -1)}");
            if (bibelot.BibelotObject.BibelotIndex == YandexPrefs.GetInt("ActiveBibelot", -1))
            {
                foreach (BibelotData data in _bibelotsData)
                {
                    if (data.GunIndex == YandexPrefs.GetInt("ActiveGun", 0))
                    {
                        //print(YandexPrefs.GetInt("ActiveGun", 0));
                        bibelot.transform.localPosition = data.Position;
                        bibelot.transform.localRotation = Quaternion.Euler(data.Rotation);
                        bibelot.gameObject.SetActive(true);
                        break;
                    }
                }
            }
            else
            {
                bibelot.gameObject.SetActive(false);
            }
        }
    }
}

[Serializable]
public struct BibelotData
{
    public GunObject GunObject;
    public Vector3 Position;
    public Vector3 Rotation;

    public int GunIndex => GunObject.GunIndex;
}