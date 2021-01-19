using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private AudioClip _explosionAudio = null;
    [SerializeField] private LayerMask layerMask;
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isDead = false;
    private bool _canShoot = true;

    public static Action<int> ScoreAction;


    void OnEnable()
    {
        _isDead = false;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosionAudio;
        transform.position = GetSpawnPoint();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        CheckForPlayer();


        if (transform.position.y < -(Constants.yRange + Constants.OutOffset) && !_isDead)
        {
            // Destroy(gameObject);
            transform.position = GetSpawnPoint();
            _canShoot = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("enemy collided with player");
            other.GetComponent<Player>().GetDamage();
            _animator.SetBool("onEnemyExplosion", true);
            Destroy(GetComponent<BoxCollider2D>());
            _isDead = true;
            _audioSource.Play();
            Destroy(gameObject, 2.5f);
        }

        else if (other.gameObject.tag == "Laser")
        {
            other.gameObject.SetActive(false);
            ScoreAction?.Invoke(1);
            _animator.SetTrigger("onEnemyExplosion");
            Destroy(GetComponent<BoxCollider2D>());
            _isDead = true;
            _audioSource.Play();
            Destroy(gameObject, 2.5f);
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return new Vector3(UnityEngine.Random.Range(-Constants.xRange, Constants.xRange),
                                            Constants.yRange + Constants.OutOffset, 0);
    }

    private void Shoot()
    {
        if (_canShoot)
        {
            GameObject laser = LaserObjectPool.Instance.GetEnemyObject();
            if (laser != null)
            {
                laser.SetActive(true);
                laser.transform.position = transform.position + Vector3.down;
            }
            _canShoot = false;
        }

    }

    private void CheckForPlayer()
    {
        var hit = Physics2D.RaycastAll(transform.position, Vector3.down, 5f, layerMask);
        for (var i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.CompareTag("Player"))
            {
                Shoot();
                break;
            }
        }
    }
}
