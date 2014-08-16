using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour
{
	private void Awake ()
	{
		animation["ani_Cat"].speed = .9f;
	}

	private void Start ()
	{
		InvokeRepeating("Animate", 3, 10);
	}

	private void Animate ()
	{
		animation.CrossFade("ani_Cat");
	}
}