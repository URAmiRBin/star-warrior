using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _ScoreText = null;
    [SerializeField] private Text _BestScoreText = null;
    [SerializeField] private Text _gameOverText = null; 
    [SerializeField] private Text _gameOverRestartHint = null;
    [SerializeField] private Sprite[] _livesSprites = null;
    [SerializeField] private Image _livesUI = null;
    private int _Score, _BestScore = 0;
    private int _internalLives = Constants.Lives;


    private void OnEnable() {
        Enemy.ScoreAction += AddScore;
        Player.playerDamage += ReduceLive;
        Player.playerDeath += ShowGameOver;
        _internalLives = Constants.Lives;
    }
    void OnDisable()
    {
        Enemy.ScoreAction -= AddScore;
        Player.playerDamage -= ReduceLive;
        Player.playerDeath -= ShowGameOver;
    }
    private void Start() {
        _Score = 0;
        _gameOverRestartHint.gameObject.SetActive(false);
        _BestScore = PlayerPrefs.GetInt("HighScore");
        _BestScoreText.text = "Best: " + _BestScore.ToString();
        _livesUI.sprite = _livesSprites[_internalLives--];
        _ScoreText.text = "Score: 0";
    }
    

    private void AddScore(int amount) {
        _Score += amount;
        _ScoreText.text = "Score: " + _Score.ToString();
    }

    private void ShowGameOver() {
        if(_Score > _BestScore) {
            _BestScoreText.text = "Best: " + _Score.ToString();
            PlayerPrefs.SetInt("HighScore", _Score);
        }
        _gameOverRestartHint.gameObject.SetActive(true);
        StartCoroutine(FlickerText(_gameOverText));
    }

    private void ReduceLive() {
        Debug.Log(_internalLives);
        _livesUI.sprite = _livesSprites[_internalLives--];
    }

    IEnumerator FlickerText(Text text) {
        while(true) {
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            text.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }


}
