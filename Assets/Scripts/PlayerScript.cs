using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    public GameObject lookAt;
    [SerializeField]
    public float speed;
    [SerializeField]
    public GameObject playerShip;    // Start is called before the first frame update
    [SerializeField]
    private Animator animator;

    private float shipRotationX = 0f;
    private float rotationSpeed = 60f;
    private float x;
    private float y;
    void Start()
    {
        playerShip = GameObject.FindGameObjectWithTag("PlayerShipParent");
        animator = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        playerShip.transform.Translate(new Vector3(x, y, 0), Space.World);

        RotateShipX();
        

        playerShip.transform.LookAt(lookAt.transform.position);
        ClampPosition();
        transform.Translate(new Vector3(0f, 0f, 20f * Time.deltaTime), Space.World);
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
    private void ClampPosition()
    {
        Vector3 pos = (Camera.main.WorldToViewportPoint(playerShip.transform.position));
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        playerShip.transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
