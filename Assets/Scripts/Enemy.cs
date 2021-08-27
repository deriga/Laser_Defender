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
    [SerializeField] GameObject VFXExplosion;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip projectileSound;
    [SerializeField] [Range(0, 1)] float projectileVolume = 0.5f;
    [SerializeField] AudioClip hitSound;
    [SerializeField] [Range(0,1)] float hitVolume = 0.5f;

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
            AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileVolume);
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
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(VFXExplosion, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitVolume);
        Destroy(explosion, durationOfExplosion);
    }
}
