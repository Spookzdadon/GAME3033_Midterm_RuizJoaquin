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
    [SerializeField]
    public AudioClip laserShot;
    [SerializeField]
    public AudioClip explosionSound;
    [SerializeField]
    AudioSource audioSource;


    private bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserShot;
        audioSource.Play();
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
        audioSource.clip = explosionSound;
        audioSource.Play();
        isActive = false;
        trail.Stop();
        explosion.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    IEnumerator Explode()
    {
        audioSource.clip = explosionSound;
        audioSource.Play();
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
