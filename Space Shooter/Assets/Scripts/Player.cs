using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField]
    private float _speed = 5.0f;
    private float _doubleSpeed = 2.0f;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _speedPowerupPrefab;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;

    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isShieldActive = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null.");
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
        if (_isSpeedPowerupActive == true)
        {
            transform.Translate(new Vector3(horizontalInput, VerticalInput, 0) * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, VerticalInput, 0) * _speed * Time.deltaTime);
        }

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
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

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
        _speed *= _doubleSpeed;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedPowerupActive = false;
        _speed /= _doubleSpeed; 
    }
    //*******************************


    // Shield Powerup
    //*******************************
    public void ShieldPowerupActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    //*******************************


    // Score
    //*******************************
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
