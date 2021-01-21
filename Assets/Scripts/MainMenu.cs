using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Animator _animator;

    void Start() {
        // FIXME: This returns null for main menu scene
        _animator = gameObject.GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    private void OnEnable() {
        Time.timeScale = 0f;
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void ExitMenu() {
        gameObject.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
