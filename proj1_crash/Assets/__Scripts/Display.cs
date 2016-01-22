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
    public float visibleDur = 3.5f;
    public float visibleStart = 1f;
    public float hideY = 60f;
    public float hideShowSpeed = 90f;
    public Text livesText;
	public Text fruitText;
    public Vector3 fruitDest, lifeDest, fruitTextPos, lifeTextPos;
    private bool visible = true, hiding = false, showing = false;
    public Vector3 visPos, hidePos;
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
        visPos = transform.position;
        hidePos = Vector3.zero;
        hidePos.x = visPos.x;
        hidePos.y = visPos.y + hideY;
        hidePos.z = visPos.z;
    }
	void Update()
    {
        if(visible && Time.time - visibleStart > visibleDur)
        {
            Hide();
        }
        if(hiding)
        {
            transform.position = Vector3.MoveTowards(transform.position, hidePos, Time.deltaTime * hideShowSpeed);
            if(hidePos.y - transform.position.y < .01f)
            {
                hiding = false;
            }
        }
        if (onBreak && Time.time - breakStart > breakDur)
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
        Show();
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
        if (visible)
        {
            visible = false;
            hiding = true;
        }
    }
    public void Show()
    {
        transform.position = visPos;
        if (!visible)
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
