using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileSphere : MonoBehaviour
{
    [SerializeField] private float speed = 1.7f;
    [SerializeField] private float initialSpeed = 8f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float missileStrength = 3f;
    private Rigidbody rb;
    private Transform target;
    private Vector3 lookDirection;
    private float timeElapsed = 0f;
    private bool speedIncreasing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calcula la dirección hacia el objetivo
            Vector3 moveDirection = (target.position - transform.position).normalized;

            // Rota hacia la dirección del movimiento (el objetivo)
            transform.LookAt(target);

            // Aplica la fuerza en la dirección del movimiento
            if (speedIncreasing)
            {
                if (timeElapsed < 3f)
                {
                    speed = Mathf.Lerp(initialSpeed, maxSpeed, timeElapsed / 3f);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    speed = maxSpeed;
                }
            }

            rb.AddForce(moveDirection * speed);
        }
    }


    void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromenemy = (other.gameObject.transform.position - transform.position);
            enemyRb.AddForce(awayFromenemy * missileStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        speedIncreasing = true;
    }


 
}