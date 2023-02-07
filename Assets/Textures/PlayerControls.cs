using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Eiko.YaSDK;

public class PlayerControls : MonoBehaviour, ITeleportable
{
    public float speed = 6.0f;
    public float MaxSpeed;
    public float jumpSpeed = 8.0f;
    public float rotateSpeed = 10f;
    public float forceout;
    private float _startHeath;
    [SerializeField] float _health;

    private Rigidbody _rigidbody;
    public Transform playerCamera;

    private bool _isTeleported;

    [SerializeField] private GameObject _placeForCatch;
    private bool _isCarring;

    [SerializeField] private ParticleSystem _carringEffect;

    private bool _isGrounded;

    [SerializeField] private AudioSource _carringSound;

    [SerializeField] private GameObject _menuPopUp;

    [SerializeField] private Animator _animator;

    private YandexSDK _yandexSDK;

    [SerializeField] private Image _reflectionDamageImage;

    public bool _isMenuOpen;

    private bool _isJumpPressed = false;

    public bool IsTeleported()
    {
        return _isTeleported;
    }

    public void OnTeleportEnd()
    {
        _isTeleported = false;
    }

    public void OnTeleportStart()
    {
        _isTeleported = true;
        StartCoroutine(onExit());
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        LvlTransition.Instance.OpenLvl();
        _yandexSDK = YandexSDK.instance;
        _startHeath = _health;
    }

    void Update()
    {
        if (!_isMenuOpen)
        {
            rotateSpeed = PlayerPrefs.GetFloat("PlayerSensitivity", 10);
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);

            playerCamera.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
            if (playerCamera.localRotation.eulerAngles.y != 0)
            {
                playerCamera.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed, 0, 0);
            }

            if (Input.GetMouseButtonDown(2))
            {
                if (!_isCarring)
                {
                    Ray ray = playerCamera.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.layer == 14 || hit.collider.gameObject.layer == 7 && Vector3.Distance(hit.collider.transform.position, transform.position) <= 3)
                        {
                            hit.collider.transform.SetParent(_placeForCatch.transform);
                            hit.collider.transform.GetComponent<CarryObject>().SetCarry(_placeForCatch.transform);
                            hit.collider.GetComponent<Rigidbody>().useGravity = false;
                            _carringEffect.Play();
                            _isCarring = true;
                            _carringSound.Play();
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                if (_isCarring)
                {
                    Transform child = _placeForCatch.transform.GetChild(0);

                    TeleportObject teleport = child.GetComponent<TeleportObject>();

                    child.GetComponent<Rigidbody>().useGravity = true;
                    child.GetComponent<CarryObject>().BreakCarry();
                    child.transform.parent = null;
                    _isCarring = false;
                    _carringEffect.Stop();
                    if (teleport != null)
                    {
                        teleport.OnTeleportEnd();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _menuPopUp.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                _isMenuOpen = true;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                _yandexSDK.ShowRewarded("SkipLvl");
                _yandexSDK.onRewardedAdReward += SkipLvl;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJumpPressed = true;
            }

            if (_health < _startHeath)
            {
                _health += Time.deltaTime / 1.5f;

                if (_reflectionDamageImage != null)
                {
                    _reflectionDamageImage.color = new Color(1, 0, 0, Mathf.Lerp(0f, 0.4f, (float)Normalize((double)_health, 0d, 7d, 1d, 0d)));
                }
            }
        }
    }

    private void SkipLvl(string str)
    {
        _yandexSDK.onRewardedAdReward -= SkipLvl;
        StartCoroutine(OnFinishLvl());
    }

    private IEnumerator OnFinishLvl()
    {
        yield return new WaitForSeconds(1.5f);

        PlayerPrefs.SetInt("CompleteLvl" + SceneManager.GetActiveScene().buildIndex, 1);
        LvlTransition.Instance.CloseLvl();

        yield return new WaitForSeconds(1.5f);

        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void FixedUpdate()
    {
        MovementLogic();
        if (_isJumpPressed) JumpLogic();
    }

    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        float moveVertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
        {
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        movement = transform.TransformDirection(movement);

        if (Mathf.Abs(_rigidbody.velocity.x) >= MaxSpeed || Mathf.Abs(_rigidbody.velocity.z) >= MaxSpeed) return;

        //_rigidbody.AddForce(movement * speed, ForceMode.Force);
        _rigidbody.velocity = transform.TransformDirection(new Vector3(moveHorizontal * speed, _rigidbody.velocity.y, moveVertical * speed));
    }

    private void JumpLogic()
    {
        if (_isGrounded)
        {
            _isJumpPressed = false;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpSpeed, _rigidbody.velocity.z);
        }
    }

    private IEnumerator onExit()
    {
        yield return new WaitForSeconds(0.4f);

        _isTeleported = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void OnCollisionStay(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }

    private IEnumerator OnLoadScene()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator OnReLoadScene()
    {
        LvlTransition.Instance.CloseLvl();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetDamage(float damage, Transform damagePoint)
    {
        _health -= damage;
        if (_health <= 0)
        {
            StartCoroutine(OnReLoadScene());
        }
        _rigidbody.AddForce((transform.position - damagePoint.position) * 2f, ForceMode.Impulse);
    }

    double Normalize(double val, double valmin, double valmax, double min, double max)
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }
}
