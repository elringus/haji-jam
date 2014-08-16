using UnityEngine;
using System.Collections;

public class SGUI : MonoBehaviour
{
	#region SINGLETON
	private static SGUI _instance;
	public static SGUI I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(SGUI)) as SGUI;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	public dfRichTextLabel labelInfo;
	public dfTweenFloat tweenPaper;
	public dfPanel panelMain;
	public Camera guiCamera;

	private void Start ()
	{
		panelMain.Opacity = .01f;
		DontDestroyOnLoad(gameObject);
	}

	public void OnLevelWasLoaded (int level)
	{
		if (Application.loadedLevelName != "scn_Start" && Application.loadedLevelName != "scn_End" && Application.loadedLevelName != "scn_GUI")
		{
			if (LevelManager.restartCount < 3) labelInfo.Text = LevelManager.I.infoText;
			else if (LevelManager.restartCount < 6) labelInfo.Text = LevelManager.I.hint1Text;
			else labelInfo.Text = LevelManager.I.hint2Text;
			tweenPaper.StartValue = 0;
			tweenPaper.EndValue = .9f;
			tweenPaper.Play();
		}
		else panelMain.Opacity = .01f;
	}
}