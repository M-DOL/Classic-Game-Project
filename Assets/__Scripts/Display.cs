using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Display : MonoBehaviour
{
    public static Display S;
    public bool lifeFlying = false;
    public int numFruit = 0;
    public int numLives = 3;
    bool onBreak = false;
    public float breakStart;
    public float breakDur = .5f;
    public int remainingIncrementFruits = 0;
    public float visibleDur = 3.5f;
    public float visibleStart = 1f;
    public float hideY = 120f;
    public float hideShowSpeed = 150f;
    public Text livesText;
    public Text fruitText;
    public Text congrats;
    public Vector3 fruitTextPos, lifeTextPos;
    public bool visible = true, hiding = false;
    float start, select;
    public Text pauseText;
    public GameObject newLife;
    bool flickering = false;
    public int flickerTimes = 4;
    Vector3 desPos;
    Transform fruitIcon, fruitNum, livesNum, livesIcon;
    Transform[] elements;
    Vector3[] visPos, hidePos;
    Vector3 fruitIconVis, livesIconVis, fruitNumVis, livesNumVis;
    Vector3 fruitIconHide, livesIconHide, fruitNumHide, livesNumHide;
    public GameObject checkPointCrate;
    void Awake()
    {
        S = this;
    }

    // Use this for initialization
    void Start()
    {
        Show();
       PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        fruitIcon = transform.FindChild("FruitIcon");
        livesIcon = transform.FindChild("LivesIcon");
        livesNum = transform.FindChild("NumLives");
        fruitNum = transform.FindChild("NumFruits");
        congrats = transform.FindChild("Congrats").GetComponent<Text>();
        congrats.enabled = false;
        elements = new Transform[] { fruitIcon, livesIcon, livesNum, fruitNum };
        livesText = livesNum.GetComponent<Text>();
        fruitText = fruitNum.GetComponent<Text>();
        visPos = new Vector3[] { fruitIcon.localPosition, livesIcon.localPosition, livesNum.localPosition, fruitNum.localPosition };
        hidePos = new Vector3[] { fruitIcon.localPosition, livesIcon.localPosition, livesNum.localPosition, fruitNum.localPosition };
        for (int i = 0; i < hidePos.Length; ++i)
        {
            hidePos[i].y += hideY;
        }
        pauseText = transform.FindChild("Pause").GetComponent<Text>();
        pauseText.gameObject.SetActive(false);

        SetFruits(PlayerPrefs.GetInt("Fruits"));
        SetLives(PlayerPrefs.GetInt("Lives"));
    }
    void Update()
    {
        start = Input.GetAxis("Submit");
        select = Input.GetAxis("Cancel");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Reset();
            SceneManager.LoadScene("_NSanityBeach_WH");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Reset();
            SceneManager.LoadScene("_CustomLevel");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Show();
        }
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
        if (lifeFlying)
        {
            newLife.transform.localPosition = Vector3.MoveTowards(newLife.transform.localPosition, desPos, 8f);
            if (Vector3.Magnitude(newLife.transform.localPosition - desPos) < 1f)
            {
                StartCoroutine(Flicker());
                lifeFlying = false;
                desPos.x += 100f;
            }
        }
        if (flickering)
        {
            newLife.transform.localPosition = Vector3.MoveTowards(newLife.transform.localPosition, desPos, 4f);
            if (Vector3.Magnitude(newLife.transform.localPosition - desPos) < 1f)
            {
                IncrementLives();
                Destroy(newLife);
                flickering = false;
            }
        }
        if (hiding)
        {
            for (int i = 0; i < elements.Length; ++i)
            {
                elements[i].localPosition = Vector3.MoveTowards(elements[i].localPosition, hidePos[i], Time.deltaTime * hideShowSpeed);
            }
            if (hidePos[0].y - elements[0].localPosition.y < .01f)
            {
                hiding = false;
            }
        }
        else if (visible)
        {
            for (int i = 0; i < elements.Length; ++i)
            {
                elements[i].localPosition = visPos[i];
            }
        }
    }
    public void SetLives(int setLife)
    {
        numLives = setLife;
        livesText.text = numLives.ToString();
    }
    public void IncrementLives()
    {
        ++numLives;
        livesText.text = numLives.ToString();
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
    public void SetFruits(int setFruits)
    {
        numFruit = setFruits;
        fruitText.text = numFruit.ToString();
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
            Crash.S.PlaySound("ExtraLife");
            newLife = Instantiate(Resources.Load("Prefabs/LivesIcon")) as GameObject;
            newLife.transform.SetParent(gameObject.transform);
            newLife.transform.localScale = 2.2f * livesIcon.transform.localScale;
            Vector3 pos = visPos[1];
            pos.x -= 100f;
            desPos = pos;
            newLife.transform.localPosition = pos;
            lifeFlying = true;
            Show();
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
        hiding = false;
        if (!visible)
        {
            visible = true;
        }
        visibleStart = Time.time;
    }
    public void Pause()
    {
        AudioListener.pause = true;
        start = Input.GetAxisRaw("Submit");
        select = Input.GetAxisRaw("Cancel");
        Time.timeScale = 0;
        pauseText.gameObject.SetActive(true);
        if (select > 0)
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            pauseText.gameObject.SetActive(false);
            Show();
        }
    }
    public void lifeFly(Vector3 lifePos)
    {
        lifeFlying = true;
        Vector2 startPos = Camera.main.WorldToViewportPoint(lifePos);
        newLife = Instantiate(Resources.Load("Prefabs/LivesIcon")) as GameObject;
        RectTransform loc = newLife.GetComponent<RectTransform>();
        loc.position = Camera.main.ViewportToScreenPoint(startPos);
        newLife.transform.SetParent(gameObject.transform);
        newLife.transform.localScale = 2.2f * livesIcon.transform.localScale;
        desPos = visPos[1];
        desPos.x -= 100f;
    }
    public IEnumerator Flicker()
    {
        Image newLifeImage = newLife.GetComponent<Image>();
        for (int i = 0; i < flickerTimes; ++i)
        {
            newLifeImage.color = Color.clear;
            yield return new WaitForSeconds(.2f);
            newLifeImage.color = Color.white;
            yield return new WaitForSeconds(.2f);
        }
        flickering = true;
    }
    void OnApplicationQuit()
    {
        Reset();
    }
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Lives", 3);
    }
}