using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviourSingleton<SpawnManager>
{
    [SerializeField] private float _Intervals = 3f;
    [SerializeField] private GameObject _Enemy = null;
    // TODO: Change enemy spawning speed for difficulty sake

    [SerializeField] private GameObject _EnemyContainer = null;
    [SerializeField] private GameObject[] _PowerUps = null;
    // TODO: for testing purposes, make power up spawn change modifiable

    private bool StopSpawning = false;

    private void OnEnable() {
        Player.playerDeath += OnPlayerDeath;
    }
    void OnDisable()
    {
        Player.playerDeath -= OnPlayerDeath;    
    }

    // Update is called once per frame
    void Start()
    {
        StopSpawning = true;
    }

    public void StartSpawning() {
        StopSpawning = false;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPARoutine());
    }

    IEnumerator SpawnEnemyRoutine() {
        while(!StopSpawning) {
            // TODO: Make spawning fast according to score
            // TODO: Generate faster and more reactive enemies according to score
            GameObject NewEnemy = Instantiate(_Enemy);
            NewEnemy.transform.parent = _EnemyContainer.transform;
            yield return new WaitForSeconds(_Intervals);
        }
    }

    IEnumerator SpawnPARoutine() {
        while(!StopSpawning) {
            yield return new WaitForSeconds(Random.Range(5, 10));
            int index = Random.Range(0, _PowerUps.Length);
            Instantiate(_PowerUps[index]);
        }
    }

    

    public void OnPlayerDeath() {
        StopSpawning = true;
    }
}
