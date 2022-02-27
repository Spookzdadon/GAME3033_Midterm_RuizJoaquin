using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject pauseCanvas;
    [SerializeField]
    public LayerMask ignoreLayer;
    [SerializeField]
    public GameObject lookAt;
    [SerializeField]
    public float speed;
    [SerializeField]
    public GameObject playerShip;
    [SerializeField]
    public GameObject playerMesh;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    public CinemachineDollyCart dollyCart;
    [SerializeField]
    public GameObject muzzleLocation;
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    RawImage crosshair;
    [SerializeField]
    ParticleSystem explosion;
    [SerializeField]
    AudioSource explosionSound;
    [SerializeField]
    GameObject restartButton;
    [SerializeField]
    GameObject winCanvas;

    private float shipRotationX = 0f;
    private float rotationSpeed = 60f;
    private float shipSpeed = 20f;
    private float x;
    private float y;
    private Transform cameraOriginPos;
    public bool isAlive = true;
    private bool canBoost = true;
    private bool canBarrelRoll = true;
    public bool isPaused = false;

    void Start()
    {
        explosionSound = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        cameraOriginPos = mainCamera.transform;
        dollyCart = GetComponent<CinemachineDollyCart>();
        playerMesh = GameObject.FindGameObjectWithTag("PlayerShip");
        playerShip = GameObject.FindGameObjectWithTag("PlayerShipParent");
        animator = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Animator>();

        dollyCart.m_Speed = 140f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive && !isPaused)
        {
            x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
            y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

            if (canBoost)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Boost();
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    Brake();
                }
                else
                {
                    dollyCart.m_Speed = 140f;
                    mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -40f), 0.3f);
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, ignoreLayer))
                {
                    return;
                }

                else if (Physics.Raycast(ray, out hit))
                {
                    muzzleLocation.transform.LookAt(hit.point);
                    Instantiate<GameObject>(bulletPrefab, muzzleLocation.transform.position, muzzleLocation.transform.rotation);
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && canBarrelRoll)
            {
                StartCoroutine(BarrelRollRight());
            }

            else if (Input.GetKeyDown(KeyCode.Q) && canBarrelRoll)
            {
                StartCoroutine(BarrelRollLeft());
            }

            RotateShipX();
            ClampPosition();

            crosshair.transform.position = Input.mousePosition;

            playerShip.transform.LookAt(lookAt.transform.position);
            playerShip.transform.Translate(new Vector3(x, y, 0), Space.Self);
        }
        else
        {
            dollyCart.m_Speed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseCanvas.SetActive(true);
            isPaused = true;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    private void RotateShipX()
    {
        if (x < 0)
        {
            if (shipRotationX >= -1f)
            {
                shipRotationX -= 0.1f * Time.deltaTime * rotationSpeed;
            }
            else
            {
                shipRotationX = -1f;
            }
        }
        else if (x > 0)
        {
            if (shipRotationX <= 1f)
            {
                shipRotationX += 0.1f * Time.deltaTime * rotationSpeed;
            }
            else
            {
                shipRotationX = 1f;
            }
        }
        else
        {
            if (shipRotationX < 0f)
            {
                shipRotationX += 0.1f * Time.deltaTime * rotationSpeed;
            }
            else if (shipRotationX > 0f)
            {
                shipRotationX -= 0.1f * Time.deltaTime * rotationSpeed;
            }
            else
            {
                shipRotationX = 0f;
            }
        }
        animator.SetFloat("ShipRotX", shipRotationX);
    }

    private void Boost()
    {
        dollyCart.m_Speed = 170f;
        mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -60f), 0.3f);
    }

    private void Brake()
    {
        dollyCart.m_Speed = 60f;
        mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -30f), 0.3f);
    }

    private void ClampPosition()
    {
        Vector3 pos = (Camera.main.WorldToViewportPoint(playerShip.transform.position));
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        playerShip.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            isAlive = false;
            playerMesh.SetActive(false);
            explosion.Play();
            explosionSound.Play();
            Cursor.visible = true;
            restartButton.SetActive(true);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BoostPipe"))
        {
            dollyCart.m_Speed = 200f;
            mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -100f), 0.3f);
            canBoost = false;
        }

        if(other.gameObject.CompareTag("WinTrigger"))
        {
            winCanvas.SetActive(true);
            isAlive = false;
            Cursor.visible = true;
        }
    }

    IEnumerator BarrelRollRight()
    {
        canBarrelRoll = false;
        animator.SetTrigger("BarrelRoll");
        yield return new WaitForSeconds(0.5f);
        canBarrelRoll = true;

    }

    IEnumerator BarrelRollLeft()
    {
        canBarrelRoll = false;
        animator.SetTrigger("BarrelRollLeft");
        yield return new WaitForSeconds(0.5f);
        canBarrelRoll = true;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
    }
}
