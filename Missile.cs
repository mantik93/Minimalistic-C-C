using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody rb;
    private Health aggressor;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.Translate(Vector3.forward, Space.Self);
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * 50, ForceMode.Impulse);
        Invoke(nameof(Off), 5f);
    }
    private void Start()
    {
        aggressor = GetComponentInParent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red, 10, false);
    }
    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            Debug.Log(aggressor.name + ", " + health.name);
            health.Damage(aggressor);
        }
        Off();
    }
    private void OnCollisionEnter(Collision collision)
    {
        var health = collision.gameObject.GetComponentInParent<Health>();
        if (health != null)
        {
            Health aggressor = gameObject.GetComponentInParent<Health>();
            health.Damage(aggressor);
        }
        //Debug.Log(collision.gameObject.name);
        //Debug.Log(gameObject.GetInstanceID());
        gameObject.SetActive(false);
    }
    void Off()
    {
        transform.position = transform.parent.position;
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
