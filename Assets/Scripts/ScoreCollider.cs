using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCollider : MonoBehaviour
{
    // Start is called before the first frame update


    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            ScoreManager.scoreCount += 500;
            Destroy(gameObject);
        }





    }

}
