using UnityEngine;
using System.Collections;

public class TriggerEndLevel : MonoBehaviour
{
	public AudioSource sndL1End;
	private Transform model;

	private void Awake ()
	{
		model = transform.FindChild("model");
	}

	public void Update ()
	{
		model.localScale = Vector3.Lerp(model.localScale, new Vector3(.9f + Mathf.Abs(Mathf.Sin(Time.time)) / 1.25f, .11f, .75f + Mathf.Abs(Mathf.Sin(Time.time))) / 1.5f, Time.deltaTime);
	}

	public void OnTriggerEnter (Collider colli)
	{
		if (colli.CompareTag("Player"))
		{
			if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
			if (Application.loadedLevelName == "lvl_1" && !sndL1End.isPlaying) sndL1End.Play();
			LevelManager.I.NextLevel();
		}
	}
}