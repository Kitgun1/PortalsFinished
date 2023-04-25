using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketContainer : MonoBehaviour
{
    public MarketItem[] _items;

    private void Start()
    {
        _items = GetComponentsInChildren<MarketItem>();
    }
}
