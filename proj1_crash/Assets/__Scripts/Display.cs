using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Display : MonoBehaviour {

	public static Display S;
	public int maxLives;
	public int numFruit = 0;
	public int numLives = 3;
    bool onBreak = false;
    public float breakStart;
    public float breakDur = .5f;
    public int remainingIncrementFruits = 0;
    public float visibleDur = 2f;
    public float visibleStart;
    public float hideY = 30f;
    public float hideShowSpeed = 30f;
    public Text livesText;
	public Text fruitText;
    public Vector3 fruitDest, lifeDest, fruitTextPos, lifeTextPos;
    private bool visible = true, hiding = false, showing = true;
    void Awake(){
		S = this;
	}

	// Use this for initialization
	void Start () {
		livesText = transform.FindChild ("NumLives").GetComponent<Text> ();
        fruitText = transform.FindChild ("NumFruits").GetComponent<Text> ();
        fruitDest = transform.FindChild("FruitIcon").transform.position;
        lifeDest = transform.FindChild("LivesIcon").transform.position;
        fruitTextPos = transform.FindChild("NumFruits").transform.position;
        lifeTextPos = transform.FindChild("NumLives").transform.position;
    }
	void Update()
    {
        if(visible && Time.time - visibleStart > visibleDur)
        {
            Hide();
        }
        if(hiding)
        {

        }
        else if(showing)
        {

        }
        if(onBreak && Time.time - breakStart > breakDur)
        {
            if(remainingIncrementFruits > 0)
            {
                --remainingIncrementFruits;
                IncrementFruit();
                onBreak = true;
                breakStart = Time.time;
            }
            else
            {
                onBreak = false;
            }
        }
    }
	public void IncrementLives(){
		if(numLives != maxLives){
			++numLives;
			livesText.text = numLives.ToString();
            Crash.S.PlaySound("ExtraLife");
		}
	}

	public void DecrementLives(){
		if (numLives != 0) {
			--numLives;
			livesText.text = numLives.ToString ();
		} else {
            // Game Over
            SceneManager.LoadScene("_Scene_GameOver");
		}
	}
    public void OrderIncrementFruit()
    {
        if(!onBreak)
        {
            onBreak = true;
            breakStart = Time.time;
            IncrementFruit();
        }
        else
        {
            ++remainingIncrementFruits;
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
    public void Hide()
    {
        if(visible)
        {
            visible = false;
            hiding = true;
        }
    }
    public void Show()
    {
        if(!visible)
        {
            visible = true;
            showing = true;
        }
        visibleStart = Time.time;
    }
	public void Restart(){
		// Reload _Scene_0 to restart the game
		SceneManager.LoadScene("_Scene0");
	}
}
