﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    private bool _isSpeedPowerupActive = false;
    [SerializeField]
    private GameObject _speedPowerupPrefabe;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    // Player Movement 
    //*******************************
    void CalculateMovement()
    {
        // set up inputs for movement 
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        // the value of the movment 
        transform.Translate(new Vector3(horizontalInput, VerticalInput, 0) * _speed * Time.deltaTime);

        // restrict the vertical movment 
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        // restrict the horizontal movemnt
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    //*******************************

    // Fire Laser
    //*******************************
    void FireLaser()
    {
        _canFire = Time.time + _fireRate; // Fire cool down
    
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, (transform.position + new Vector3(0, 1.05f, 0)), Quaternion.identity);
        }
     
    }
    //*******************************

    // Damage Player
    //*******************************
    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    //*******************************

    // Triple Shot
    //*******************************
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }
    //*******************************

    // Speed Powerup
    //*******************************
    public void SpeedPowerupActive()
    {
        _isSpeedPowerupActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedPowerupActive = false;
    }
    //*******************************
}
