using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public bool onYes = true;
    public Transform selectorY, selectorN;
    public Color on, off;
    // Use this for initialization
    void Start()
    {
        selectorY = transform.FindChild("SelectorY");
        selectorN = transform.FindChild("SelectorN");
        on = selectorY.GetComponent<Text>().color;
        off = selectorN.GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && onYes)
        {
            selectorN.GetComponent<Text>().color = on;
            selectorY.GetComponent<Text>().color = off;
            onYes = !onYes;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !onYes)
        {
            selectorY.GetComponent<Text>().color = on;
            selectorN.GetComponent<Text>().color = off;
            onYes = !onYes;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetInt("Fruits", 0);
            PlayerPrefs.SetInt("Lives", 3);
            if (!onYes)
            {
                SceneManager.LoadScene("_Scene_Title");
            }
            else
            {
                SceneManager.LoadScene("_NSanityBeach_WH");
            }
        }
    }
}
