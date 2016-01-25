using UnityEngine;
using System.Collections;

public class FruitGen : MonoBehaviour
{
    public static FruitGen S;
    public GameObject fruitPrefab;
    void Awake()
    {
        S = this;
    }
    public void ManageFruits(int num, float x, float y, float z)
    {
        StartCoroutine(CollectFruit(num, new Vector3(x, y, z)));
    }
    IEnumerator CollectFruit(int num, Vector3 pos)
    {
        for (int i = 0; i < num; ++i)
        {
            GameObject newFruit = Instantiate(fruitPrefab, pos, Quaternion.identity) as GameObject;
            yield return new WaitForSeconds(.2f);
            newFruit.GetComponent<WumpaFruit>().FlyToCounter();
        }
    }
}
