using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    public bool IsCarring;
    public Transform ParentObject;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

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
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
        }
    }

    public void BreakCarry()
    {
        ParentObject = null;
        IsCarring = false;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
        }
    }
}
