using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObjectPool : MonoBehaviourSingleton<LaserObjectPool>
{
    private List<GameObject> _pool;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private GameObject laser = null;
    [SerializeField] private GameObject enemyLaser = null;

    void Awake()
    {
        _pool = new List<GameObject>();
        for(var i = 0; i < poolSize/2; i++) {
            GameObject go = Instantiate(laser);
            go.transform.parent = transform;
            _pool.Add(go);
            go.SetActive(false);
        }
        for(var i = poolSize/2; i < poolSize; i++) {
            GameObject go = Instantiate(enemyLaser);
            go.transform.parent = transform;
            _pool.Add(go);
            go.SetActive(false);
        }    
    }

    public GameObject GetObject(){
        for(var i = 0; i < poolSize/2; i++) {
            if(!_pool[i].activeInHierarchy) {
                return _pool[i];
            }
        }
        return null;
    }

    public GameObject GetEnemyObject() {
        for(var i = poolSize/2; i < poolSize; i++) {
            if(!_pool[i].activeInHierarchy) {
                return _pool[i];
            }
        }
        return null;
    }

    public GameObject[] Get3Objects() {
        GameObject[] laserBatch = new GameObject[3];
        int j = 0;
        for(var i = 0; i < poolSize/2 && j < 3; i++) {
            if(!_pool[i].activeInHierarchy) {
                laserBatch[j++] = _pool[i];
            }
        }
        return laserBatch;
    }
}
