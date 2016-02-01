using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AkuAkuMask : MonoBehaviour {
	public bool follow = false;

    public Color[] originalColors;
    public Material[] materials; // All the Materials of this & its children

    void Awake()
    {
        materials = GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Crash"){
            Crash.S.numMasks++;
            if(Crash.S.numMasks > 1)
            {
                Destroy(this.gameObject);
            }
			if (Crash.S.numMasks == 3) {
				Crash.S.StartCoroutine("Invincible");
			}
            else
            {
                Crash.S.PlaySound("AkuAkuPickup");
            }
            follow = true;
        }
	}

	void FixedUpdate () {
        if (follow && !Crash.S.spinning) {
            gameObject.transform.parent = Crash.S.transform;
            gameObject.transform.name = "AkuAkuMask";
            transform.localPosition = new Vector3(1.2f, .5f, -.1f);
            transform.localRotation = Quaternion.Euler(270, 270, 90);
            if (Crash.S.numMasks == 3)
            {
                gameObject.transform.parent = Crash.S.transform;
                transform.localRotation = Quaternion.Euler(270, 270, 90);
                transform.localPosition = new Vector3(-0.229f, 0.5f, 1.4f);
            }
		}
        if (Crash.S.numMasks == 2 && gameObject.transform.parent == Crash.S.transform)
        {
            foreach (Material m in materials)
            {
                m.color = Color.red;
            }
        }
        else {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = originalColors[i];
            }
        }
	}

    // Returns a list of all Materials on this GameObject or its children
    static public Material[] GetAllMaterials(GameObject go)
    {
        List<Material> mats = new List<Material>();
        if (go.GetComponent<Renderer>() != null)
        {
            mats.Add(go.GetComponent<Renderer>().material);
        }
        foreach (Transform t in go.transform)
        {
            mats.AddRange(GetAllMaterials(t.gameObject));
        }
        return (mats.ToArray());
    }

}
