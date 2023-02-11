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

    private void Start()
    {
        _player = GetComponent<PlayerControls>();
    }

    public void ShootBlue()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer != 12) return;


            Blue.transform.rotation = Quaternion.LookRotation(hit.normal);
            Blue.transform.position = hit.point + Blue.transform.forward * 0.3f;
            _BlueParticle.Play();
            OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
            if (portalPlace != null) portalPlace.portal = Blue;

            _portalShootSound.Play();
        }
        _animation.Play();
    }

    public void ShootRed()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer != 12) return;

            Red.transform.rotation = Quaternion.LookRotation(hit.normal);
            Red.transform.position = hit.point + Red.transform.forward * 0.3f;
            _RedParticle.Play();
            OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
            if (portalPlace != null) portalPlace.portal = Red;

            _portalShootSound.Play();
        }
        _animation.Play();
    }

    private void Update()
    {
        if (!_player._isMenuOpen && !_player._isMobile)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer != 12) return;

                    if (Input.GetMouseButtonDown(1))
                    {
                        Red.transform.rotation = Quaternion.LookRotation(hit.normal);
                        Red.transform.position = hit.point + Red.transform.forward * 0.3f;
                        _RedParticle.Play();
                        OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                        if (portalPlace != null) portalPlace.portal = Red;
                    }
                    else
                    {
                        Blue.transform.rotation = Quaternion.LookRotation(hit.normal);
                        Blue.transform.position = hit.point + Blue.transform.forward * 0.3f;
                        _BlueParticle.Play();
                        OffPortalOnMove portalPlace = hit.collider.GetComponent<OffPortalOnMove>();
                        if (portalPlace != null) portalPlace.portal = Blue;
                    }
                    _portalShootSound.Play();
                }
                _animation.Play();
            }
        }
    }
}
