using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour
{
	private void Awake ()
	{
		GetComponent<Renderer>().enabled = false;
	}
}