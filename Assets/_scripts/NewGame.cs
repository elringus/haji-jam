using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour
{
	public dfLabel labelLoading;
	private AsyncOperation async;
	private bool loading;
	private bool readyToLoad;

	private void Start ()
	{
		audio.Play();
		labelLoading.GetComponent<dfTweenFloat>().Play();
	}

	private void Update ()
	{
		if (readyToLoad && Input.GetMouseButtonUp(0) && !loading) StartCoroutine("Load");

		if (Application.GetStreamProgressForLevel(1) + Application.GetStreamProgressForLevel(2) < 2) labelLoading.Text = "loading " +
			(int)(((Application.GetStreamProgressForLevel(1) + Application.GetStreamProgressForLevel(2)) / 2) * 100) + "%";
		else if (!readyToLoad)
		{
			readyToLoad = true;
			Application.LoadLevelAdditiveAsync("scn_GUI");
			labelLoading.Text = "click to play";
			labelLoading.GetComponent<dfTweenFloat>().Stop();
			labelLoading.Opacity = 1;
		}
	}

	IEnumerator Load ()
	{
		loading = true;
		async = Application.LoadLevelAsync(2);
		async.allowSceneActivation = true;
		yield return async;
	}
}