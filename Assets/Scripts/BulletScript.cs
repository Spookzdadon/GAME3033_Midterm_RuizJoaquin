using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    ParticleSystem trail;
    [SerializeField]
    ParticleSystem explosion;

    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BulletLifeSpan());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
        }
    }
    IEnumerator BulletLifeSpan()
    {
        yield return new WaitForSeconds(3);
        isActive = false;
        trail.Stop();
        explosion.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    IEnumerator Explode()
    {
        isActive = false;
        trail.Stop();
        explosion.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(Explode());
    }
}
