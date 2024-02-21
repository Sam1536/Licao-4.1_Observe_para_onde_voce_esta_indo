using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rig;
    public float speed = .5f;
    private GameObject focalPoint;

    public bool hasPowerUp;

    private float powerupStrength = 15f;

    public GameObject powerupIndicator;
    public GameObject missilePrefab;

    [Header("PowerJump")]
    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    bool smashing = false;
    float floorY= 2f;
   

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Ponto Focal ");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        rig.AddForce(focalPoint.transform.forward * speed * forwardInput);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (Input.GetKey(KeyCode.X) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
            Debug.Log("gg");
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                Vector3 playerPosition = transform.position;
                Vector3 enemyPosition = enemy.transform.position;

                // Calcula la dirección desde el jugador al enemigo
                Vector3 directionToEnemy = (enemyPosition - playerPosition).normalized;

                // Ajusta la posición de instanciación del proyectil
                Vector3 spawnPosition = playerPosition + directionToEnemy * 2f;

                GameObject newMissile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
                MissileSphere missileScript = newMissile.GetComponent<MissileSphere>();
                missileScript.SetTarget(enemy.transform);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody rigEnemy = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("esta colidindo " + collision.gameObject.name + "Pegou PowerUp " + hasPowerUp);
            rigEnemy.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);

    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        //Store the y position before taking off
        floorY = transform.position.y;
        //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;
        Debug.Log("gg2");

        while (Time.time < jumpTime)
        {
            //move the player up while still keeping their x velocity.
            rig.velocity = new Vector2(rig.velocity.x, smashSpeed);
            yield return null;
            Debug.Log("gg3");

        }
        //Now move the player down
        while (transform.position.y > floorY)
        {
            rig.velocity = new Vector2(rig.velocity.x, -smashSpeed * 2);
            yield return null;
            Debug.Log("gg4");

        }
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            Debug.Log("gg5");

        }
        //We are no longer smashing, so set the boolean to false
        smashing = false;
        Debug.Log("gg6");



    }
}