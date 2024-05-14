using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
       
            // Reset score when MainMenu scene starts
            ScoreManager.ResetScore();
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
