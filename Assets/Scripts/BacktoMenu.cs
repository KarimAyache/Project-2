using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            ScoreManager.scoreCount += 1500;
            SceneManager.LoadSceneAsync(0);
        }





    }

}
