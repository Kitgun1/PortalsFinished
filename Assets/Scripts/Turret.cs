using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private PlayerControls _player;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireDelay;
    [SerializeField] private float _fireDistance;
    private AudioSource _audio;
    private bool _isActive = true;
    private bool _isDamagable = true;
    private int _killing;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _explosion;
    private AudioSource _audioExp;

    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
        }
    }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isActive)
        {
            transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));

            Ray ray = new Ray(transform.position, _player.transform.position - transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == 8 && Vector3.Distance(transform.position, hit.collider.transform.position) <= _fireDistance)
                {
                    if (!_audio.isPlaying)
                    {
                        _audio.Play();
                    }

                    if (_isDamagable)
                    {
                        StartCoroutine(SendDamage());
                    }
                }
                else
                {
                    if (_killing < 1)
                    {
                        StopAllCoroutines();
                    }
                    _isDamagable = true;
                    if (_audio.isPlaying)
                    {
                        _audio.Stop();
                    }
                }
            }
        }
    }

    private IEnumerator SendDamage()
    {
        _isDamagable = false;
        yield return new WaitForSeconds(_fireDelay);

        _player.GetDamage(_damage, transform);
        _isDamagable = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _fireDistance);
    }

    public void Kill()
    {
        _killing++;
        if (_killing == 1)
        {
            StartCoroutine(StartKilling());
        }
    }

    private IEnumerator StartKilling()
    {
        _fire.Play();
        yield return new WaitForSeconds(1f);
        _audioExp = _explosion.GetComponent<AudioSource>();
        _explosion.Play();
        _audioExp.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
