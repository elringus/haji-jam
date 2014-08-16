using UnityEngine;
using System.Collections;

public class TV : MonoBehaviour
{
	#region SINGLETON
	private static TV _instance;
	public static TV I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(TV)) as TV;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	public Texture CommMovie;
	public Texture MGSTexture;

	private void Start ()
	{
		renderer.materials[1].mainTexture = CommMovie;
		((MovieTexture)renderer.materials[1].mainTexture).loop = true;
		((MovieTexture)renderer.materials[1].mainTexture).Play();
	}

	private void Update ()
	{
		//if (!((MovieTexture)renderer.materials[1].mainTexture).isPlaying) 
		//	((MovieTexture)renderer.materials[1].mainTexture).Play();
	}

	public void SwitchMGS ()
	{
		renderer.materials[1].mainTexture = MGSTexture;
	}
}