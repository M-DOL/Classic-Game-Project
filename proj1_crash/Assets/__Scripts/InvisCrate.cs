using UnityEngine;
using System.Collections;

public class InvisCrate : MonoBehaviour
{
    public GameObject cratePrefab;
    public void ReplaceBox()
    {
        Destroy(gameObject);
        GameObject newObj = Instantiate(cratePrefab, transform.position, transform.rotation) as GameObject;
        newObj.GetComponent<Rigidbody>().useGravity = false;
    }
}
