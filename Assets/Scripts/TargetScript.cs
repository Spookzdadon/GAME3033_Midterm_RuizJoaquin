using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField]
    public Material lockedMat;
    [SerializeField]
    public Material unlockedMat;
    [SerializeField]
    public FallingWallScript fallingWallScript;

    private bool isUnlocked;

    private void Start()
    {
        isUnlocked = false;
        gameObject.GetComponent<MeshRenderer>().material = lockedMat;
        fallingWallScript = GetComponentInParent<FallingWallScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isUnlocked)
        {
            isUnlocked = true;
            gameObject.GetComponent<MeshRenderer>().material = unlockedMat;
            fallingWallScript.increaseUnlocks();
        }
    }
}
