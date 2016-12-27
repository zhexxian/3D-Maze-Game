using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D fadingTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -900; //render second last
	private float alpha = 1.0f;
	private int fadeDir = -1;


	void OnGUI () {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadingTexture);
	}

	public float BeginFade (int direction){
		alpha = 1.0f;
		fadeDir = direction;
		return(fadeSpeed);
	}

}
