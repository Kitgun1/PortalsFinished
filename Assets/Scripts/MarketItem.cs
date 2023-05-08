using UnityEngine;
using UnityEngine.UI;
using Eiko.YaSDK.Data;
using Eiko.YaSDK;

public class MarketItem : MonoBehaviour
{
    [SerializeField] private GunObject _gunObject;
    [SerializeField] private CharacterObject _charObject;
    [SerializeField] private BibelotObject _bibelotObject;
    [SerializeField] private GameObject _GetButton;
    [SerializeField] private GameObject _SetButton;
    [SerializeField] private GameObject _SettedLable;
    [SerializeField] private Text _adsCountText;
    private MarketContainer _marketContainer;

    private void Start()
    {
        _marketContainer = GetComponentInParent<MarketContainer>();
        if (_gunObject != null)
        {
            if (!_gunObject.IsTaken)
            {
                _GetButton.SetActive(true);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(false);
                _adsCountText.text = _gunObject.AdWatching + "/" + _gunObject.AdToTake;
            }
            else
            {
                if (YandexPrefs.GetInt("ActiveGun", 0) == _gunObject.GunIndex)
                {
                    _GetButton.SetActive(false);
                    _SetButton.SetActive(false);
                    _SettedLable.SetActive(true);
                }
                else
                {
                    _GetButton.SetActive(false);
                    _SetButton.SetActive(true);
                    _SettedLable.SetActive(false);
                }
            }
        }
        else if (_charObject != null)
        {
            if (!_charObject.IsTaken)
            {
                _GetButton.SetActive(true);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(false);
                _adsCountText.text = YandexPrefs.GetInt("Cakes", 0) + "/" + _charObject.CakesToTake;
            }
            else
            {
                if (YandexPrefs.GetInt("ActiveChar", 0) == _charObject.CharIndex)
                {
                    _GetButton.SetActive(false);
                    _SetButton.SetActive(false);
                    _SettedLable.SetActive(true);
                }
                else
                {
                    _GetButton.SetActive(false);
                    _SetButton.SetActive(true);
                    _SettedLable.SetActive(false);
                }
            }
        }
        else if (_bibelotObject != null)
        {
            if (!_bibelotObject.IsTaken)
            {
                _GetButton.SetActive(true);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(false);
                _adsCountText.text = _bibelotObject.Janov.ToString();
            }
            else
            {
                if (YandexPrefs.GetInt("ActiveBibelot", -1) == _bibelotObject.BibelotIndex)
                {
                    _GetButton.SetActive(false);
                    _SetButton.SetActive(false);
                    _SettedLable.SetActive(true);
                }
                else
                {
                    _GetButton.SetActive(false);
                    _SetButton.SetActive(true);
                    _SettedLable.SetActive(false);
                }
            }
        }
    }

    public void WatchAdForGun()
    {
        YandexSDK.instance.ShowRewarded(_gunObject.GunIndex.ToString());
        YandexSDK.instance.onRewardedAdReward += GetGun;
        //YandexSDK.instance.onRewardedAdError += StopAd;
        //YandexSDK.instance.onRewardedAdClosed += StopAd;
    }

    public void BuyForBibelot(string id)
    {
        // buy gun
        YandexSDK.instance.onPurchaseSuccess += GetBibelot;
        YandexSDK.instance.ProcessPurchase(id);
    }

    public void StopAd(int num)
    {
        YandexSDK.instance.onRewardedAdReward -= GetGun;
        YandexSDK.instance.onRewardedAdError -= StopAd;
        YandexSDK.instance.onRewardedAdClosed -= StopAd;
    }

    public void StopAd(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= GetGun;
        YandexSDK.instance.onRewardedAdError -= StopAd;
        YandexSDK.instance.onRewardedAdClosed -= StopAd;
    }

    public void GetChar()
    {
        if (YandexPrefs.GetInt("Cakes", 0) >= _charObject.CakesToTake)
        {
            YandexPrefs.SetInt("CharTaken" + _charObject.CharIndex, 1);
            _charObject.IsTaken = true;
            _GetButton.SetActive(false);
            _SetButton.SetActive(true);
            _SettedLable.SetActive(false);
        }
    }

    public void GetGun(string str)
    {
        YandexSDK.instance.onRewardedAdReward -= GetGun;
        YandexSDK.instance.onRewardedAdError -= StopAd;
        YandexSDK.instance.onRewardedAdClosed -= StopAd;

        int adWatched = _gunObject.AdWatching;
        YandexPrefs.SetInt("GunAdWatched" + _gunObject.GunIndex, adWatched + 1);
        _gunObject.AdWatching = adWatched + 1;
        _adsCountText.text = _gunObject.AdWatching + "/" + _gunObject.AdToTake;

        if (_gunObject.AdWatching >= _gunObject.AdToTake)
        {
            YandexPrefs.SetInt("GunTaken" + _gunObject.GunIndex, 1);
            _gunObject.IsTaken = true;
            _GetButton.SetActive(false);
            _SetButton.SetActive(true);
            _SettedLable.SetActive(false);
        }
    }

    public void GetBibelot(Purchase purchase)
    {
        print($"{purchase.purchaseToken} ~~~ {purchase.productID}!~!!!");

        _adsCountText.text = _bibelotObject.Janov.ToString();
        YandexPrefs.SetInt("BibelotTaken" + _bibelotObject.BibelotIndex, 1);
        _bibelotObject.IsTaken = true;
        _GetButton.SetActive(false);
        _SetButton.SetActive(true);
        _SettedLable.SetActive(false);
        //print($"{_GetButton.activeSelf} ~ {_SetButton.activeSelf} ~ {_SettedLable.activeSelf}");
        YandexSDK.instance.onPurchaseSuccess -= GetBibelot;
    }

    public void SetChar()
    {
        MarketItem[] items = _marketContainer._items;

        foreach (var item in items)
        {
            item.Disable();
        }

        YandexPrefs.SetInt("ActiveChar", _charObject.CharIndex);
        _GetButton.SetActive(false);
        _SetButton.SetActive(false);
        _SettedLable.SetActive(true);
    }

    public void SetGun()
    {
        MarketItem[] items = _marketContainer._items;

        foreach (var item in items)
        {
            item.Disable();
        }

        YandexPrefs.SetInt("ActiveGun", _gunObject.GunIndex);
        _GetButton.SetActive(false);
        _SetButton.SetActive(false);
        _SettedLable.SetActive(true);
    }

    public void SetBibelot()
    {
        MarketItem[] items = _marketContainer._items;

        foreach (var item in items)
        {
            item.Disable();
        }

        YandexPrefs.SetInt("ActiveBibelot", _bibelotObject.BibelotIndex);
        _GetButton.SetActive(false);
        _SetButton.SetActive(false);
        _SettedLable.SetActive(true);
    }

    private void Disable()
    {
        if (_gunObject != null)
        {
            if (!_gunObject.IsTaken)
            {
                _GetButton.SetActive(true);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(false);
            }
            else
            {
                _GetButton.SetActive(false);
                _SetButton.SetActive(true);
                _SettedLable.SetActive(false);
            }
        }
        else if (_charObject != null)
        {
            if (!_charObject.IsTaken)
            {
                _GetButton.SetActive(true);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(false);
            }
            else
            {
                _GetButton.SetActive(false);
                _SetButton.SetActive(true);
                _SettedLable.SetActive(false);
            }
        }
        else if (_bibelotObject != null)
        {
            if (!_bibelotObject.IsTaken)
            {
                _GetButton.SetActive(true);
                _SetButton.SetActive(false);
                _SettedLable.SetActive(false);
            }
            else
            {
                _GetButton.SetActive(false);
                _SetButton.SetActive(true);
                _SettedLable.SetActive(false);
            }
        }
    }
}