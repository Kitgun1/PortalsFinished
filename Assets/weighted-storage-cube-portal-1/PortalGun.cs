using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public Portal Red;
    public Portal Blue;
    public Animation _animation;
    public ParticleSystem _RedParticle;
    public ParticleSystem _BlueParticle;
    [SerializeField] private AudioSource _portalShootSound;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer != 12) return;
                
                if (Input.GetMouseButtonDown(0))
                {
                    Red.transform.rotation = Quaternion.LookRotation(hit.normal);
                    Red.transform.position = hit.point + Red.transform.forward * 0.3f;
                    _RedParticle.Play();
                }
                else
                {
                    Blue.transform.rotation = Quaternion.LookRotation(hit.normal);
                    Blue.transform.position = hit.point + Blue.transform.forward * 0.3f;
                    _BlueParticle.Play();
                }

                _portalShootSound.Play();
            }
            _animation.Play();
        }
    }
}
