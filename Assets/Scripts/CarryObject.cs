using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    public bool IsCarring;
    public Transform ParentObject;

    private void Update()
    {
        if (IsCarring)
        {
            transform.position = ParentObject.position;
        }
    }

    public void SetCarry(Transform parent)
    {
        ParentObject = parent;
        IsCarring = true;
    }

    public void BreakCarry()
    {
        ParentObject = null;
        IsCarring = false;
    }
}
