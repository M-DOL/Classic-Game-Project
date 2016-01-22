using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public float fadeSpeed = 1.5f;
    public Image panel;
    bool startScene = true;
    public static ScreenFader S;
    void Awake()
    {
        S = this;
        panel = GetComponent<Image>();
        panel.color = Color.black;
    }
    void Update()
    {
        if(startScene)
        {
            StartScene();
        }
        else
        {
            EndScene();
        }
    }
    void FadeToClear()
    {
        panel.color = Color.Lerp(panel.color, Color.clear, fadeSpeed * Time.deltaTime);
    }
    void FadeToBlack()
    {
        panel.color = Color.Lerp(panel.color, Color.black, fadeSpeed * Time.deltaTime);
    }
    void StartScene()
    {
        FadeToClear();
        if(panel.color.a <= .05f)
        {
            panel.color = Color.clear;
            panel.enabled = false;
        }
    }
    public void EndScene()
    {
        panel.enabled = true;
        startScene = false;
        panel.color = Color.black;
        startScene = true;
    }
}
