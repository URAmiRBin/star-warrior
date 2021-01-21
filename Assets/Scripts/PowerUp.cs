using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _Speed = 3f;
    [SerializeField] private Constants.PowerUps _PowerUpType = Constants.PowerUps.triple;
    [SerializeField] private AudioClip _powerUpSound = null;
    void OnEnable()
    {
        transform.position = GetSpawnPoint();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_Speed * Time.deltaTime * Vector3.down);
        if(transform.position.y < -(Constants.yRange + Constants.OutOffset)) {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().ActivatePA(_PowerUpType);
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject, 1);
        }        
    }

    public Vector3 GetSpawnPoint() {
        return new Vector3(   Random.Range(-Constants.xRange, Constants.xRange), 
                                            Constants.yRange + Constants.OutOffset, 0);
    }
}
