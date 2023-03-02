using UnityEngine;
using Eiko.YaSDK.Data;

public class ActivateCurrentGun : MonoBehaviour
{
    [SerializeField] private CameraGun[] _cameraGuns;

    private void Start()
    {
        foreach (var item in _cameraGuns)
        {
            if (item.gunObject.GunIndex == YandexPrefs.GetInt("ActiveGun", 0))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
