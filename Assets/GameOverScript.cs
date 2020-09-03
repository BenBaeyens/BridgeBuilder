using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject gameOverMenu;
    bool gameOver = false;

    private void Update()
    {
        if (playerController.hasStarted && playerController.agent.velocity == Vector3.zero)
        {
            StartCoroutine(CheckForLoss());
            if (gameOver)
            {
                // Pause the game and set the menu to true
                gameOverMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void RestartGame()
    {
        // Reset the time and reload the scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    IEnumerator CheckForLoss()
    {
        yield return new WaitForSeconds(0.5f);
        if (playerController.hasStarted && playerController.agent.velocity == Vector3.zero)
        {
            gameOver = true;
        }
    }
}
