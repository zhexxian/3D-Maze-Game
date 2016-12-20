using UnityEngine;
using System.Collections;

public class GemCountScript : MonoBehaviour {

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string gemString = GlobalVariable.CurrGemNumber + "/" + GlobalVariable.MaxGemNumber;
        GetComponent<UnityEngine.UI.Text>().text = gemString;
    }
}
