using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject currentCanvas;
    [SerializeField]
    public GameObject nextCanvas;
    [SerializeField]
    public GameObject creditsCanvas;
    [SerializeField]
    GameObject playerController;

    public void OpenNextScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OpenNextCanvas()
    {
        nextCanvas.SetActive(true);
        currentCanvas.SetActive(false);
    }

    public void OpenCreditsCanvas()
    {
        creditsCanvas.SetActive(true);
        currentCanvas.SetActive(false);
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        playerController.GetComponent<PlayerController>().isPaused = false;
        currentCanvas.SetActive(false);
        Time.timeScale = 1;
    }
}
