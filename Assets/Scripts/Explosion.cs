using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        Destroy(gameObject, 3f);
        _audioSource.Play();
    }
}
