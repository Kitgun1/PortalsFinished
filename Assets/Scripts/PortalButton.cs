using System.Collections.Generic;
using UnityEngine;

public class PortalButton : MonoBehaviour
{
    [SerializeField] private List<AnimationAfterPressAbstract> _animations;

    private int _collidersCount;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            if (_collidersCount == 0)
            {
                foreach (var item in _animations)
                {
                    item.Enter();
                }

                _audio.Play();
            }
            _collidersCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            _collidersCount--;

            if (_collidersCount == 0)
            {
                foreach (var item in _animations)
                {
                    item.Exit();
                }
            }
        }
    }
}
