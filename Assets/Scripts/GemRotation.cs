using UnityEngine;
using System.Collections;

public class GemRotation : MonoBehaviour {

    public AudioClip sfx;
    public GameObject PrefabAudioSource;

    int[] gemCoordinate;
	// Use this for initialization
	void Start () {
        gemCoordinate = GlobalVariable.ConvertPositionToCoordinate(transform.position.x, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(0, -2, 0);

        /*
        print(GlobalVariable.GetPlayerCoordinate()[0].ToString() + " " +
            GlobalVariable.GetPlayerCoordinate()[1].ToString() + " " +
            GlobalVariable.GetPlayerCoordinate()[2].ToString() + " " +
            gemCoordinate[0].ToString() + " " +
            gemCoordinate[1].ToString() + " " +
            gemCoordinate[2].ToString()
            );
        */
        int[] playerCoordinate = GlobalVariable.GetPlayerCoordinate();
        if ((playerCoordinate[0]==gemCoordinate[0]) &&
            (playerCoordinate[1] == gemCoordinate[1]) &&
            (playerCoordinate[2] == gemCoordinate[2]))
        {
            GameObject audio = (GameObject)Instantiate(PrefabAudioSource);
            audio.GetComponent<AudioSource>().PlayOneShot(sfx);
            GameObject.Find("MainPlayer/PSGemCollect").GetComponent<ParticleSystem>().Play();
            GlobalVariable.CurrGemNumber += 1;
            GlobalVariable.nonActiveGem.Add(gameObject.name);
            gameObject.SetActive(false);
        }
	}
}
