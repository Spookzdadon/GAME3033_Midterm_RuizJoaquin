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

    private float shipRotation = 0f;
    private float rotationSpeed = 60f;
    void Start()
    {
        playerShip = GameObject.FindGameObjectWithTag("PlayerShipParent");
        animator = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        playerShip.transform.Translate(new Vector3(x, y, 0), Space.World);
        if (x < 0)
        {
            if (shipRotation >= -1f)
            {
                shipRotation -= 0.1f * Time.deltaTime * rotationSpeed;
            }
            else
            {
                shipRotation = -1f;
            }
        }
        else if (x > 0)
        {
            if (shipRotation <= 1f)
            {
                shipRotation += 0.1f * Time.deltaTime * rotationSpeed;
            }
            else
            {
                shipRotation = 1f;
            }
        }
        else
        {
            if (shipRotation < 0f)
            {
                shipRotation += 0.1f * Time.deltaTime * rotationSpeed;
            }
            else if (shipRotation > 0f)
            {
                shipRotation -= 0.1f * Time.deltaTime * rotationSpeed;
            }
            else
            {
                shipRotation = 0f;
            }
        }
        animator.SetFloat("ShipRot", shipRotation);
        playerShip.transform.LookAt(lookAt.transform.position);
    }
}
