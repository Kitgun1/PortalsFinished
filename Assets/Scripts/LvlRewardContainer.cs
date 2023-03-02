using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LvlRewardContainer : MonoBehaviour
{
    [SerializeField] private GunObject _gunObject;
    [SerializeField] private GameObject[] _LvlRewardBoxes;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _disableColor;
    [SerializeField] private Color _lastColor;
    [SerializeField] private FinishLvl _finishLvl;

    private void Start()
    {
        if (_gunObject == null || _gunObject.IsTaken) 
        {
            gameObject.SetActive(false);
        }

        int index = (SceneManager.GetActiveScene().buildIndex % 5) - 1;

        if (index < 0)
        {
            index = 4;
        }

        if (index == 4)
        {
            _finishLvl = FindObjectOfType<FinishLvl>();
            _finishLvl._isRewardedLvl = true;
            _finishLvl.gunObject = _gunObject;
        }

        foreach (var item in _LvlRewardBoxes)
        {
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(37.1132f, 37.1132f);
            item.GetComponent<Image>().color = _disableColor;
        }

        for (int i = 0; i < index + 1; i++)
        {
            _LvlRewardBoxes[i].GetComponent<Image>().color = _activeColor;
        }

        _LvlRewardBoxes[index].GetComponent<Image>().color = _disableColor;
        _LvlRewardBoxes[index].GetComponent<RectTransform>().sizeDelta = new Vector2(46.6376f, 46.6376f);
        _LvlRewardBoxes[_LvlRewardBoxes.Length - 1].GetComponent<Image>().color = _lastColor;
    }
}
