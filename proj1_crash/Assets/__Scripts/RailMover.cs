using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public struct DestroyedElement
{
	public string tag;
	public Vector3 placement;
	public Quaternion rotation;
}

public class RailMover : MonoBehaviour {

	public Rail rail;
	public Transform lookAt;
	public bool smoothMove = true;
	public float moveSpeed = 2.5f;

	public ArrayList crateTags = new ArrayList{ "Crate", "MultiCrate", "BounceCrate", "LifeCrate", "TriggerCrate", "MaskCrate", "RandomCrate" };
	public List<DestroyedElement> destroyed;
	public float frontFollowDistance = 4.8f;
	public float backFollowDistance = 7.5f;
	public bool frontFacing = true;
	public float speed = 2f;
	public float distThres = .01f;
	Vector3 newPos;
	public static RailMover S;

	private Transform thisTransform;
	private Vector3 lastPosition;

	void Awake()
	{
		S = this;
		destroyed = new List<DestroyedElement>();
	}

	private void Start(){
		thisTransform = transform;
		lastPosition = thisTransform.position;
	}

	private void Update(){
		Vector3 pos = rail.ProjectPositionOnRail (lookAt.position);
		pos.z -= 5f;
		if (smoothMove) {
			lastPosition = Vector3.Lerp (lastPosition, pos, moveSpeed * Time.deltaTime);
			thisTransform.position = lastPosition;
		} else {
			thisTransform.position = pos;
		}
		thisTransform.LookAt (lookAt.position);
	}

	public void RespawnItems()
	{
		GameObject toInst = null;
		foreach (DestroyedElement gone in destroyed)
		{
			toInst = Resources.Load("Prefabs/" + gone.tag) as GameObject;
			if(toInst != null)
			{
				Instantiate(toInst, gone.placement, gone.rotation);
			}
		}
		destroyed.Clear();
	}

	public void AddToRespawn(GameObject g)
	{
		DestroyedElement dest = new DestroyedElement();
		dest.placement = Vector3.zero;
		dest.placement.x = g.transform.position.x;
		dest.placement.y = g.transform.position.y;
		dest.placement.z = g.transform.position.z;
		dest.rotation.w = g.transform.rotation.w;
		dest.rotation.x = g.transform.rotation.x;
		dest.rotation.y = g.transform.rotation.y;
		dest.rotation.z = g.transform.rotation.z;
		dest.tag = g.tag;
		destroyed.Add(dest);
	}
}
