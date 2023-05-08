using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObject : MonoBehaviour
{
    [SerializeField] private float _timeToDespawn = -20;
    public bool IsCarring;
    public Transform ParentObject;
    private Rigidbody _rigidbody;

    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;

    private float _delayCheck = 3;
    private float _time;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _defaultPosition = transform.position;
        _defaultRotation = transform.rotation;
    }

    private void Update()
    {
        if (IsCarring)
        {
            transform.position = ParentObject.position;
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _delayCheck)
            {
                if (transform.position.y < _timeToDespawn)
                {
                    ResetTransform();
                }

                _time = 0;
            }
        }
    }

    public void ResetTransform()
    {
        transform.position = _defaultPosition;
        transform.rotation = _defaultRotation;
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