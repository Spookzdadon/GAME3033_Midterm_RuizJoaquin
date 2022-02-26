using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject lookAt;
    [SerializeField]
    public float speed;
    [SerializeField]
    public GameObject playerShip;    // Start is called before the first frame update
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    public CinemachineDollyCart dollyCart;

    private float shipRotationX = 0f;
    private float rotationSpeed = 60f;
    private float shipSpeed = 20f;
    private float x;
    private float y;
    private Transform cameraOriginPos;

    void Start()
    {
        cameraOriginPos = mainCamera.transform;
        dollyCart = GetComponent<CinemachineDollyCart>();
        playerShip = GameObject.FindGameObjectWithTag("PlayerShipParent");
        animator = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Animator>();

        dollyCart.m_Speed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

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
            shipSpeed = 20f;
            mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -40f), 0.3f);
        }

        RotateShipX();
        ClampPosition();


        playerShip.transform.LookAt(lookAt.transform.position);
        playerShip.transform.Translate(new Vector3(x, y, 0), Space.Self);
        transform.Translate(new Vector3(0f, 0f, dollyCart.m_Speed), Space.World);
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
        shipSpeed = 100f;
        mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -60f), 0.3f);
    }

    private void Brake()
    {
        shipSpeed = 10f;
        mainCamera.transform.DOLocalMove(new Vector3(0, 4.6f, -30f), 0.3f);
    }

    private void ClampPosition()
    {
        Vector3 pos = (Camera.main.WorldToViewportPoint(playerShip.transform.position));
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        playerShip.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}