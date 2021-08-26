using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float Xpadding = 0.6f;
    [SerializeField] float Ypadding = 0.4f;
    [SerializeField] int health = 500;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;

    float xMin, xMax;
    float yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);        
    }

    private void SetupMoveBoundries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + Xpadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - Xpadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + Ypadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - Ypadding;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer enemyShot = other.gameObject.GetComponent<DamageDealer>();
        health -= enemyShot.GetDamage();
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}
