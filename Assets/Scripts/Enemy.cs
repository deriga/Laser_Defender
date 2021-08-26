using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //config params
    [SerializeField] float health = 200;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.5f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    [SerializeField] GameObject enemyProjectilePrefab;
    [SerializeField] float projectileSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(0.5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        CounterDownAndShoot();
    }

    private void CounterDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject enemyShot = Instantiate(enemyProjectilePrefab, transform.position, Quaternion.identity) as GameObject;
        enemyShot.GetComponent<Rigidbody2D>().velocity = new Vector2(0 ,-projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
