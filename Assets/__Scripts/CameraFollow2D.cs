using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraFollow2D : MonoBehaviour
{
	public float rightFollowDistance = 4.8f;
	public float leftFollowDistance = 7.5f;
	public bool rightFacing = true;
	public float speed = 2f;
	public float distThres = .01f;
	public Vector3 newPos;
	public static CameraFollow2D S;
	void Awake()
	{
		S = this;
	}
	void Start()
	{
		newPos = Camera.main.transform.position;
		newPos.z = Crash.S.transform.position.z - 5f;
		Camera.main.transform.position = newPos;
	}
	void Update()
	{
		newPos = Camera.main.transform.position;
		newPos.x = Crash.S.rigid.velocity.x * Time.deltaTime;
		if (Crash.S.rigid.velocity.x > .01f)
		{
			rightFacing = true;
		}
		else if (Crash.S.rigid.velocity.x < -.01f)
		{
			rightFacing = false;
		}

		newPos.y = Crash.S.sceneCenter.y + 4f;
		newPos.x = Crash.S.transform.position.x - (rightFacing ? rightFollowDistance : leftFollowDistance);

		if (Mathf.Abs(newPos.z - Camera.main.transform.position.z) > distThres)
		{
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPos, Time.deltaTime * speed + Mathf.Abs(Crash.S.rigid.velocity.z) / 30f);
		}
		else
		{
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, newPos, Time.deltaTime * speed);
		}
	}
}
