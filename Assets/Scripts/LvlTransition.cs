using UnityEngine;
using DG.Tweening;

public class LvlTransition : Singletone<LvlTransition>
{
    public void OpenLvl()
    {
        GetComponent<RectTransform>().DOSizeDelta(new Vector2(0,0), 1.5f);
    }

    public void CloseLvl()
    {
        GetComponent<RectTransform>().DOSizeDelta(new Vector2(2000, 2000), 1.5f);
    }
}
