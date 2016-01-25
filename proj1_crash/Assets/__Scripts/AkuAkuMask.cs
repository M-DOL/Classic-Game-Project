using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AkuAkuMask : MonoBehaviour {
	public bool follow = false;
    public static AkuAkuMask mask;

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

    void OnCollisionEnter(Collision col){
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
        mask = this;
        if (follow) {
			Vector3 pos = mask.transform.position;
			pos.x = Crash.S.transform.position.x + 0.5f;
			pos.y = Crash.S.transform.position.y + 2f;
			pos.z = Crash.S.transform.position.z - 1f;
            mask.transform.position = pos;
            if (Crash.S.numMasks == 3)
            {
                mask.gameObject.transform.parent = Crash.S.transform;
                mask.transform.localRotation = Quaternion.identity;
                Vector3 temp = new Vector3(-0.229f, 0.384f, 1.252f);
                mask.transform.localPosition = temp;
            }
            
            if(!Crash.S.spinning)
            {
                transform.eulerAngles = new Vector3(270, 270, 90) + Crash.S.transform.eulerAngles;
            }
		}
        if (Crash.S.numMasks == 2)
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

    public virtual void LoseMask()
    {
        Crash.S.numMasks--;
        if (Crash.S.numMasks == 0) {
            Destroy(mask.gameObject);
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
