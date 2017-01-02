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
        storyAlign1 = "\t At the beginning of the creation of the world, there was a war between the two factions of gods, better known as big-bang." +
            "Jefa, the god who has the ability to see the future knows the end of the war and decided to join in Sett's faction.\n";
        storyAlign2 = "\t The war of gods finally end with Sett's faction as the winner. After the war, the world was still covered by void." +
            " The gods decided to start creating some creatures to stay and fill this world. The other gods agreed to hand over the task of creation to Jefa.\n";
        storyAlign3 = "\t Jefa has created so many creatures and the last Jefa created human that he created based on the entity of the gods. " +
            " The other gods really admire and appreciate Jefa's creation. They were amazed because some of the creature have the ability to think and speak." +
            " Because Jefa really loves the human beings he created, he is concerned over their lives. " +
            "Thus, Jefa decides to go down to earth and lives with humans to teach them skills such as reading, writing, sailing, taming animals, healing, and constructing a vehicles for transportation.\n";
        storyAlign4 = "\t Some gods are very interested in the development of human, but most of the other gods are concerned and worried that humans will become too powerful and overtake the gods." +
            " Sett forbids Jefa for helping humans, however Jefa ignores it. With the talent to see the future, Jefa is sure that human will be very developed and advanced in every way, " +
            "so once again he helped the human by giving the Ancient Fire, which is the one of the element of life that only appear in the heaven of gods.\n";
        storyAlign5 = "\t Knowing what Jefa has done, the other gods are very angry and agree to punish him. Jefa is imprisoned in the hundreds layered maze which guarded by some titans. " +
            "With the gift and the knowledge he has, he knew he have to collect some fragments of gem as the key to be able to unlock and get out from the maze.\n";
        storyHint = "\t In order to free, Jefa need to collect gems from three world: the land world, the sea world, and the sky world";
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
