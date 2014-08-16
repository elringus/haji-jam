using UnityEngine;
using System.Collections;

public class Milk : MonoBehaviour
{
	private Transform myTransform;

	private void Awake ()
	{
		myTransform = transform;
	}

	private void Update ()
	{
		myTransform.Rotate(new Vector3(0, 0, Time.deltaTime * 50));
	}
}