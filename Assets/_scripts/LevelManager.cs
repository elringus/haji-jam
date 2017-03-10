using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	#region SINGLETON
	private static LevelManager _instance;
	public static LevelManager I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	private Vignetting vignetting;
	private bool fadingOut;
	public bool started;
	public bool starting;
	public string infoText;
	public string hint1Text;
	public string hint2Text;
	public bool readyToRestart;

	public static int restartCount;
	public static bool clickToRun;

	private void Awake ()
	{
		vignetting = Camera.main.GetComponent<Vignetting>();
		vignetting.blur = 50;
		vignetting.blurSpread = 50;
		started = false;
	}

	private void Update ()
	{
		if (!starting && Input.GetMouseButton(0))
		{
			starting = true;
			Invoke("StartLevel", 1.25f);
			SGUI.I.tweenPaper.StartValue = .9f;
			SGUI.I.tweenPaper.EndValue = 0;
			SGUI.I.tweenPaper.Play();
		}
		if (readyToRestart && Input.GetMouseButton(0)) RestartLevel();


		vignetting.blur = Mathf.Lerp(vignetting.blur, fadingOut ? 50 : 0, Time.deltaTime);
		vignetting.blurSpread = Mathf.Lerp(vignetting.blurSpread, fadingOut ? 50 : -.1f, Time.deltaTime * 2);

		if (Input.GetKeyUp(KeyCode.F1)) Application.LoadLevel(1);
		if (Input.GetKeyUp(KeyCode.F2)) Application.LoadLevel(2);
		if (Input.GetKeyUp(KeyCode.F3)) Application.LoadLevel(3);
		if (Input.GetKeyUp(KeyCode.F4)) Application.LoadLevel(4);
		if (Input.GetKeyUp(KeyCode.F5)) Application.LoadLevel(5);
		if (Input.GetKeyUp(KeyCode.F6)) Application.LoadLevel(6);
		if (Input.GetKeyUp(KeyCode.F7)) Application.LoadLevel(7);

		if (Input.GetKeyUp(KeyCode.F12)) clickToRun = !clickToRun;
	}

	private void StartLevel ()
	{
		started = true;
	}

	public void NextLevel ()
	{
		fadingOut = true;
		Invoke("LoadNextLevel", 1);
	}

	public void RestartLevel ()
	{
		if (!fadingOut)
		{
			if (!readyToRestart)
			{
				//GA.API.Design.NewEvent("restart-level-" + (Application.loadedLevel), restartCount);
				restartCount++;
				GetComponent<AudioSource>().Play();
				//S_CameraController.I.cameraFollow = true;
				readyToRestart = true;
				SGUI.I.labelInfo.Text = "О небо, я обнаружен!";
				SGUI.I.tweenPaper.StartValue = 0;
				SGUI.I.tweenPaper.EndValue = .9f;
				SGUI.I.tweenPaper.Play();
			}
			else
			{
				readyToRestart = false;
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	private void LoadNextLevel ()
	{
		//GA.API.Design.NewEvent("completed-level-" + (Application.loadedLevel));
		restartCount = 0;
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}