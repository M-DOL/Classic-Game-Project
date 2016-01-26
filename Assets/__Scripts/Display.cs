﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Display : MonoBehaviour
{

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
    public Vector3 fruitDest, lifeDest, fruitTextPos, lifeTextPos, pauseTextPos;
    public bool visible = true, hiding = false;
    public Vector3 cameraToCanvas, visPos, hidePos;
    float start, select;
    public Text pauseText;
    Transform fruits, lives;
    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        fruits = transform.FindChild("FruitIcon");
        lives = transform.FindChild("LivesIcon");
        livesText = transform.FindChild("NumLives").GetComponent<Text>();
        fruitText = transform.FindChild("NumFruits").GetComponent<Text>();
        pauseText = transform.FindChild("Pause").GetComponent<Text>();
        pauseTextPos = transform.FindChild("Pause").transform.position;
        pauseText.gameObject.SetActive(false);
        cameraToCanvas = Camera.main.transform.position - transform.position;
    }
    void Update()
    {
        start = Input.GetAxis("Submit");
        select = Input.GetAxis("Cancel");

        if (start > 0)
        {
            ScreenFader.S.EndScene();
            Pause();
        }

        if (visible && Time.time - visibleStart > visibleDur)
        {
            Hide();
        }
        if (onBreak && Time.time - breakStart > breakDur)
        {
            if (remainingIncrementFruits > 0)
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
    void FixedUpdate()
    {
        fruitDest = fruits.position;
        lifeDest = lives.position;
        if (hiding)
        {
            transform.position = Vector3.MoveTowards(transform.position, hidePos, Time.deltaTime * hideShowSpeed);
            if (hidePos.y - transform.position.y < .01f)
            {
                hiding = false;
            }
        }
        else if(visible)
        {
            transform.position = Camera.main.transform.position - cameraToCanvas;
        }
        pauseText.transform.position = pauseTextPos;
    }
    public void IncrementLives()
    {
        if (numLives != maxLives)
        {
            ++numLives;
            livesText.text = numLives.ToString();
            Crash.S.PlaySound("ExtraLife");
        }
    }

    public void DecrementLives()
    {
        Show();
        if (numLives != 0)
        {
            --numLives;
            livesText.text = numLives.ToString();
        }
        else
        {
            // Game Over
            SceneManager.LoadScene("_Scene_GameOver");
        }
    }
    public void OrderIncrementFruit()
    {
        if (!onBreak)
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
    public void IncrementFruit()
    {
        Show();
        if (numFruit != 99)
        {
            ++numFruit;
            fruitText.text = numFruit.ToString();
        }
        else
        {
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
        visPos = transform.position;
        hidePos = new Vector3(visPos.x, visPos.y + hideY, visPos.z);
    }
    public void Show()
    {
        hiding = false;
        if (!visible)
        {
            visible = true;
        }
        visibleStart = Time.time;
    }
    public void Restart()
    {
        // Reload _Scene_0 to restart the game
        SceneManager.LoadScene("_Scene0");
    }
    public void Pause()
    {
        start = Input.GetAxisRaw("Submit");
        select = Input.GetAxisRaw("Cancel");
        Time.timeScale = 0;
        pauseText.gameObject.SetActive(true);
        if (select > 0)
        {
            Time.timeScale = 1;
            pauseText.gameObject.SetActive(false);
            Show();
        }
    }
}