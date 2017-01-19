using UnityEngine;
using System.Collections;

public class GameSceneAudioSource : MonoBehaviour
{
    float elapsedTime;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = elapsedTime + Time.deltaTime;
        if ((!GetComponent<AudioSource>().isPlaying) && (elapsedTime<3))
        {
            Destroy(gameObject);
        }
    }
}
