using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
    public Vector3[] selectLocs = new Vector3[3];
    public int selected = 0;
    public int selectionNum = 0;
    public Transform selector;
	// Use this for initialization
	void Start ()
    {
        selector = transform.FindChild("Selector");
        selectLocs[0] = new Vector3(-198, -63, 0);
        selectLocs[1] = new Vector3(-198, -132, 0);
        selectLocs[2] = new Vector3(-198, -204, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("_NSanityBeach_WH");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("_CustomLevel");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            --selectionNum;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++selectionNum;
        }
        if (selectionNum == -1)
        {
            selectionNum = 2;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch(selectionNum % 3)
            {
                case 0:
                    SceneManager.LoadScene("_NSanityBeach_WH");
                    break;
                case 1:
                    SceneManager.LoadScene("_CustomLevel");
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }
        selector.localPosition = selectLocs[selectionNum % 3];
    }
}
