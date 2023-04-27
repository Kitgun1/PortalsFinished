using System;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public Portal Red;
    public Portal Blue;
    public Animation _animation;
    public ParticleSystem _RedParticle;
    public ParticleSystem _BlueParticle;
    [SerializeField] private AudioSource _portalShootSound;
    private PlayerControls _player;
    [SerializeField] LayerMask _mask;
    private Camera _camera;
    [SerializeField] private LayerMask _maskForBoxCast;

    private void Start()
    {
        _camera = Camera.main;
        _player = GetComponent<PlayerControls>();
    }

    public void ShootBlue()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;

            Transform blue;
            (blue = Blue.transform).rotation = Quaternion.LookRotation(hit.normal);
            blue.position = hit.point + blue.forward * 0.3f;
            _BlueParticle.Play();
            OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
            if (portalPlace != null) portalPlace.portal = Blue;

            _portalShootSound.Play();
        }

        _animation.Play();
    }

    public void ShootRed()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;


            Transform red;
            (red = Red.transform).rotation = Quaternion.LookRotation(hit.normal);
            red.position = hit.point + red.forward * 0.3f;
            _RedParticle.Play();
            OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
            if (portalPlace != null) portalPlace.portal = Red;

            _portalShootSound.Play();
        }

        _animation.Play();
    }

    private void Update()
    {
        if (_player.IsMenuOpen || _player.IsMobile) return;
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _mask))
        {
            if (hit.collider.gameObject.layer != 12) return;
            Collider[] overlapBox = Physics.OverlapBox(hit.point + hit.normal * 0.3F, new Vector3(1F, 1.8F, 0.2F) / 2,
                Quaternion.LookRotation(hit.normal), _maskForBoxCast);
            if (overlapBox.Length == 0)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Transform redTransform;
                    (redTransform = Red.transform).rotation = Quaternion.LookRotation(hit.normal);
                    redTransform.position = hit.point + redTransform.forward * 0.3f;
                    _RedParticle.Play();
                    OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                    if (portalPlace != null) portalPlace.portal = Red;
                }
                else
                {
                    Transform blueTransform;
                    (blueTransform = Blue.transform).rotation = Quaternion.LookRotation(hit.normal);
                    blueTransform.position = hit.point + blueTransform.forward * 0.3f;
                    _BlueParticle.Play();
                    OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                    if (portalPlace != null) portalPlace.portal = Blue;
                }

                _portalShootSound.Play();
            }
        }

        _animation.Play();
    }
}