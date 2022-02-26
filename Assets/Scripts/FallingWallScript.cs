using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingWallScript : MonoBehaviour
{
    [SerializeField]
    public int locks = 5;
    private int unlocks = 0;
    private bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        unlocks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocks >= locks && !done)
        {
            gameObject.transform.Translate(Vector3.down * Time.deltaTime * 50f);
        }

    }

    public void increaseUnlocks()
    {
        unlocks += 1;
    }

    IEnumerator LowerWallTimer()
    {
        yield return new WaitForSeconds(3);
        done = true;
    }
}
