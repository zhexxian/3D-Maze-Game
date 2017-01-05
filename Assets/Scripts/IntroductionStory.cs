using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroductionStory : MonoBehaviour {
    private string storyAlign1;
    private string storyAlign2;
    private string storyAlign3;
    private string storyAlign4;
    private string storyAlign5;
    private string storyHint;
    private GameObject textStory;

    // Use this for initialization
    void Start () {
        storyAlign1 = "\t In the beginning, when the universe was created, the gods brought life to the world by creating different creatures." +
            "Each god was responsible for creating one type of creature. \n";
		storyAlign2 = "\n \t After all animals and plants are created, Jefa, the god of wisdom, created human." +
			"Jefa loves the human beings he created, that he decided to go down to earth to live with them.\n";
		storyAlign3 = "\n \t He taught human beings the skills of making fire, healing, reading and writing. " +
			"Most of the other gods admired Jefa's creation, and were amazed by human beings' ability to think and learn.\n";
		storyAlign4 = "\n \t However, several gods were worried about human becoming too powerful to be controlled. " +
			"They created a complicated maze on earth, and imprisoned Jefa, preventing him from returning to heaven to continue help human beings. \n";
		storyAlign5 = "\n \t The maze is guarded by titans. The only way to get out of the maze is by collecting gems to unlock the exit gate." +
			"The gems are scattered in all three worlds: the earth world, the sea world, and the sky world\n";
		storyHint = "\n \t Will you help Jafa?";
        textStory = GameObject.Find("txt_story");
        textStory.GetComponent<UnityEngine.UI.Text>().text = storyAlign1 + storyAlign2 + storyAlign3 + storyAlign4 + storyAlign5 + storyHint;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return"))
        {
            SceneManager.LoadScene("game-scene");
        }
    }
}
