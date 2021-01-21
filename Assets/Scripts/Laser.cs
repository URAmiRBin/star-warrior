using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if(transform.position.y > Constants.yRange + Constants.OutOffset) {
            gameObject.SetActive(false);
        }
        if(transform.position.y < - Constants.yRange - Constants.OutOffset) {
            gameObject.SetActive(false);
        }
    }
}
