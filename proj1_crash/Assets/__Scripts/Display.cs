using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Display : MonoBehaviour {

	public static Display S;
	public int maxLives;
	public int numFruit = 0;
	public int numLives = 3;
	public Text livesText;
	public Text fruitText;

	void Awake(){
		S = this;
	}

	// Use this for initialization
	void Start () {
		livesText = transform.FindChild ("NumLives").GetComponent<Text> ();
		fruitText = transform.FindChild ("NumFruits").GetComponent<Text> ();
	}
	
	public void IncrementLives(){
		if(numLives != maxLives){
			++numLives;
			livesText.text = numLives.ToString();
		}
	}

	public void DecrementLives(){
		if (numLives != 0) {
			--numLives;
			livesText.text = numLives.ToString ();
		} else {
			// Game Over
		}
	}

	public void IncrementFruit(){
		if (numFruit != 99) {
			++numFruit;
			fruitText.text = numFruit.ToString ();
		} else {
			numFruit = 0;
			fruitText.text = "0";
			IncrementLives();
		}
	}

	public void Restart(){
		// Reload _Scene_0 to restart the game
		SceneManager.LoadScene("_Scene0");
	}
}
