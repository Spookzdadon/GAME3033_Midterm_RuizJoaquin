using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClosingPipeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.DOScale(new Vector3(0f, 1f, 1f), 15f);
    }
}
