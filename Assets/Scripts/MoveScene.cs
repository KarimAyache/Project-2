using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            ScoreManager.scoreCount += 1000;
            SceneManager.LoadSceneAsync(2);
        }





    }
}
