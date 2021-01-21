using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 9f;
    [SerializeField] private int _lives = Constants.Lives;
    [SerializeField] private float _FireRate = 0.5f;
    [SerializeField] private GameObject _Shield = null;
    [SerializeField] private GameObject _rightWingFire = null;
    [SerializeField] private GameObject _leftWingFire = null;
    [SerializeField] private GameObject _explosion = null;
    [SerializeField] private AudioClip _laserAudio = null;

    public static Action playerDamage;
    public static Action playerDeath;


    private Vector3 _laserOffsetY = new Vector3(0, 1.05f, 0);
    private Vector3 _laserOffsetX = new Vector3(.8f,0,0);
    private Vector3[] _tripleLaserPositions = new Vector3[3];
    private bool _CoolingDown = false;
    private float _LastShoot = 0f;

    private bool _TriplePA = false;
    private bool _ShieldPA = false;
    private Animator _animator;
    private AudioSource _audioSource;

    private float _lastHInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _tripleLaserPositions[0] = -_laserOffsetX;
        _tripleLaserPositions[1] = _laserOffsetY;
        _tripleLaserPositions[2] = _laserOffsetX;
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = _laserAudio;
        _animator = gameObject.GetComponent<Animator>();
        _rightWingFire.SetActive(false);
        _leftWingFire.SetActive(false);
        _Shield.SetActive(false);
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LaserShoot();
    }

    private void Movement() {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // FIXME: Dude, this is dirty af
        if(hInput < 0 && hInput <= _lastHInput) {
            _animator.SetBool("isTurningLeft", true);
        } else if (hInput > 0 && hInput >= _lastHInput) {
            _animator.SetBool("isTurningRight", true);
        }

        if(_animator.GetBool("isTurningLeft") && hInput > _lastHInput) {
            _animator.SetBool("isTurningLeft", false);
        } else if(_animator.GetBool("isTurningRight") && hInput < _lastHInput) {
            _animator.SetBool("isTurningRight", false);
        } 

        transform.Translate(new Vector3(hInput, vInput, 0) * _speed * Time.deltaTime);
        transform.position = new Vector3(   Mathf.Clamp(transform.position.x, -Constants.xRange, Constants.xRange),
                                            Mathf.Clamp(transform.position.y, -Constants.yRange, Constants.yRange),
                                            0);

        _lastHInput = hInput;
    } 

    private void LaserShoot() {
        _CoolingDown = (Time.time - _LastShoot) < _FireRate;
        if(Input.GetKeyDown(KeyCode.Space) && !_CoolingDown) {
            if(_TriplePA) {
                GameObject[] lasers = LaserObjectPool.Instance.Get3Objects();
                for(var i = 0; i < 3; i++){
                    if (lasers[i] != null){
                        lasers[i].SetActive(true);
                        lasers[i].transform.position = transform.position + _tripleLaserPositions[i];
                    }
                }
            }

            else if(!_TriplePA) {
                GameObject laser = LaserObjectPool.Instance.GetObject();
                if (laser != null){
                    laser.SetActive(true);
                    Vector3 laserSpawnLocation = transform.position + _laserOffsetY;
                    laser.transform.position = laserSpawnLocation;
                }
            }
            _audioSource.Play();
            _CoolingDown = true;
            _LastShoot = Time.time;
        }
    }

    public void GetDamage() {
        if (_ShieldPA) {
            _ShieldPA = false;
            _Shield.SetActive(false);
            return;
        }
        _lives--;
        if(_lives == 2) {
            _rightWingFire.SetActive(true);
        }
        else if(_lives == 1){
            _leftWingFire.SetActive(true);
        }
        playerDamage?.Invoke();
        
        if (_lives < 1) {
            playerDeath?.Invoke();
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void ActivatePA(Constants.PowerUps PowerUpType) {
        // FIXME: Overriding triple power up time does not refresh
        // TODO: Add score for overriding shield power up
        if (PowerUpType == Constants.PowerUps.triple) {
            _TriplePA = true;
            StartCoroutine(PATime(PowerUpType));
        }
        else if (PowerUpType == Constants.PowerUps.speed) {
            _speed += 3f;
            _FireRate = 0.25f;
            StartCoroutine(PATime(PowerUpType));
        }
        else if (PowerUpType == Constants.PowerUps.shield) {
            _ShieldPA = true;
            _Shield.SetActive(true);
        }
    }

    IEnumerator PATime(Constants.PowerUps PowerUpType) {
        if(PowerUpType == Constants.PowerUps.triple) {
            _TriplePA = true;
            yield return new WaitForSeconds(7f);
            _TriplePA = false;
        }
        else if (PowerUpType == Constants.PowerUps.speed) {
            yield return new WaitForSeconds(5f);
            _speed -= 3f;
            _FireRate = 0.5f;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy Laser") {
            other.gameObject.SetActive(false);
            GetDamage();
        }
    }
}
