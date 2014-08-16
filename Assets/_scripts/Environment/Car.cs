using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
	private Transform myTransform;
	public float Speed;

	private void Awake ()
	{
		myTransform = transform;
	}

	private void Update ()
	{
		if (LevelManager.I.started) myTransform.Translate(Vector3.right * Time.deltaTime * Speed);
		if (myTransform.position.x > 60) myTransform.position -= new Vector3(120, 0);
	}
}