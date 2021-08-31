using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game Scene");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOverScene()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game Over");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
