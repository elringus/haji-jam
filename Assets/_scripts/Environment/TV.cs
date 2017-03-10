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

	public WebGLMovieTexture CommMovie;
	public Texture MGSTexture;

	private void Start ()
	{
        CommMovie = new WebGLMovieTexture("StreamingAssets/vid_Comm.ogg");

        GetComponent<Renderer>().materials[1].mainTexture = CommMovie;
        CommMovie.Play();
  //      ((MovieTexture)GetComponent<Renderer>().materials[1].mainTexture).loop = true;
		//((MovieTexture)GetComponent<Renderer>().materials[1].mainTexture).Play();
	}

	private void Update ()
	{
        if (CommMovie != null) CommMovie.Update();
        //if (!((MovieTexture)renderer.materials[1].mainTexture).isPlaying) 
        //	((MovieTexture)renderer.materials[1].mainTexture).Play();
    }

	public void SwitchMGS ()
	{
		GetComponent<Renderer>().materials[1].mainTexture = MGSTexture;
    }
}