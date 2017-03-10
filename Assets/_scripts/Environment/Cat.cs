using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour
{
	private void Awake ()
	{
		GetComponent<Animation>()["ani_Cat"].speed = .9f;
	}

	private void Start ()
	{
		InvokeRepeating("Animate", 3, 10);
	}

	private void Animate ()
	{
		GetComponent<Animation>().CrossFade("ani_Cat");
	}
}