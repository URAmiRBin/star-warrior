using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public bool _isGameOver = false;
    public bool _isPause = false;
    [SerializeField] GameObject _pauseMenu;

    private void OnEnable() {
        _pauseMenu.SetActive(false);
        Player.playerDeath += GameOver;
    }


    // Update is called once per frame
    void Update()
    {
        if(_isGameOver && Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(1);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !_isPause) {
            _pauseMenu.SetActive(true);
            _isPause = true;
        }

        else if(Input.GetKeyDown(KeyCode.Escape) && _isPause) {
            _pauseMenu.SetActive(false);
            _isPause = false;
        }
    }

    private void GameOver() {
        _isGameOver = true;
    }
}
