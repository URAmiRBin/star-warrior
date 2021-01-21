using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private GameObject _explosionEffect = null;
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _speed);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser") {
            other.gameObject.SetActive(false);
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(GetComponent<CircleCollider2D>());
            SpawnManager.Instance.StartSpawning();
            Destroy(gameObject, 1f); 
        }
   
    }
}
