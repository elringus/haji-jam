using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour
{
	#region SINGLETON
	private static Pointer _instance;
	public static Pointer I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(Pointer)) as Pointer;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	private LineRenderer line;
	[HideInInspector]
	public Transform Transform;

	private void Awake ()
	{
		line = GetComponent<LineRenderer>();
		Transform = transform;
		line.enabled = false;
	}

	private void Start ()
	{
		Transform.position = PlayerController.I.myTransform.position;
		line.SetPosition(0, PlayerController.I.myTransform.position);
		line.SetPosition(1, PlayerController.I.myTransform.position);
	}

	private void Update ()
	{
		if (LevelManager.I.started && !LevelManager.I.readyToRestart)
		{
			line.SetPosition(0, PlayerController.I.myTransform.position);
			line.SetPosition(1, Transform.position);

			if (Input.GetMouseButton(1) || PlayerController.I.Invisible)
			{
				line.enabled = false;
			}
			else
			{
				line.enabled = true;
				Ray ray = CameraController.I.myCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1000, 1 << 8)) Transform.position = new Vector3(hit.point.x, hit.point.y + .2f, hit.point.z);

				//if (Physics.Raycast(S_CharacterController.I.myTransform.position, (myTransform.position - S_CharacterController.I.myTransform.position).normalized, out hit, Vector3.Distance(myTransform.position, S_CharacterController.I.myTransform.position))) {
				//	if (hit.collider.CompareTag("Hideout")) line.SetColors(Color.green, Color.green);
				//	else line.SetColors(Color.red, Color.red);
				//}
				//else line.SetColors(Color.white, Color.white);
			}
		}
		else line.enabled = false;

	}
}