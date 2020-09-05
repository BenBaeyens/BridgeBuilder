using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject gameOverMenu;
    public GameObject winMenu;
    bool gameOver = false;
    bool gameWin = false;

    private void Update()
    {
        Debug.Log(playerController.hasStarted + " , " + playerController.destinationIsEndPoint + " , " + playerController.agent.velocity + " , " + gameOver);

        if (playerController.hasStarted && playerController.agent.velocity == Vector3.zero)
        {

            Debug.Log("CHECKING FOR LOSS");
            StartCoroutine(CheckForLoss());
            if (gameOver)
            {
                // Pause the game and set the menu to true
                gameOverMenu.SetActive(true);
                Time.timeScale = 0;
            }
            if (gameWin)
            {
                // Pause the game and set the menu to true
                winMenu.SetActive(true);
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
        Debug.Log(playerController.hasStarted + " , " + playerController.destinationIsEndPoint + " , " + playerController.agent.velocity + " , " + gameOver);
        if (playerController.HasReachedDestination() && playerController.hasStarted && playerController.destinationIsEndPoint && playerController.agent.velocity == Vector3.zero && !gameOver)
        {
            gameWin = true;
        }
        if (playerController.hasStarted && playerController.agent.velocity == Vector3.zero && !gameWin)
        {
            gameOver = true;
        }
    }
}
