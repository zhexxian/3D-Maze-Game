using UnityEngine;
using System.Collections;

public class MenuSceneAudioSource : MonoBehaviour
{
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayHover()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonHover);
    }

    public void PlayClick()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonClick);
    }
}
