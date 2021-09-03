using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float Xpadding = 0.6f;
    [SerializeField] float Ypadding = 0.4f;
    [SerializeField] int health = 500;
    [SerializeField] Slider slider;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathSoundVolume;
    [SerializeField] float fireCost = 0.01f;
    bool isShootable = true;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] AudioClip projectileSound;
    [SerializeField] float projectileVolume = 1f;

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
        IsShootable();
        DecreaseSliderValue();
        Fire();
    }

    private void DecreaseSliderValue()
    {
        if (!isShootable || Input.GetKey(KeyCode.Space) == false)
        {
            slider.value -= fireCost * Time.deltaTime * Time.deltaTime * 10;
        }
    }

    private void IsShootable()
    {
        if (slider.value <= 0.5f) { isShootable = true; }
        if (slider.value >= 0.97f) { isShootable = false; }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1") || !isShootable)
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (isShootable)
        {
            slider.value += fireCost * Time.deltaTime;
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(projectileSound, Camera.main.transform.position, projectileVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
        yield return new WaitForEndOfFrame();
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
        FindObjectOfType<Level>().LoadGameOverScene();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth()
    {
        return health;
    }
}
