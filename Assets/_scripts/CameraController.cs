using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	#region SINGLETON
	private static CameraController _instance;
	public static CameraController I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(CameraController)) as CameraController;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	[HideInInspector]
	public Transform Transform;
	private GameObject target;
	private Vector3 lookOffset;
	[HideInInspector]
	public Camera myCamera;

	private Transform startPosition;
	private Transform failPosition;

	public float CurrentPanAngle = 0, PanAngle = 0;
	public float CurrentTiltAngle = -15, TiltAngle = -15;

	public float CurrentDistance, Distance, MaxDistance;

	private int steps = 1;
	private RaycastHit hit;

	public bool CameraFollow;

	private Vector3 tarPos;
	private float tarFOV;

	private void Awake ()
	{
		Transform = transform;
		myCamera = GetComponent<Camera>();
		startPosition = GameObject.Find("point-camera").transform;
		failPosition = GameObject.Find("point-camera-fail").transform;

	}

	private void Start ()
	{
		target = GameObject.FindWithTag("Player");
		lookOffset = new Vector3(0, .5f, 0);
	}

	private void LateUpdate ()
	{
		if (Input.GetKey("mouse 1"))
		{
			//Screen.lockCursor = true;
			//Screen.showCursor = false;
			if (!Input.GetMouseButton(0)) PanAngle += Input.GetAxis("Mouse X") * 5;
			else PanAngle = target.transform.eulerAngles.y;
			TiltAngle += Input.GetAxis("Mouse Y") * 5;
		}


		//if (Physics.SphereCast(target.transform.position + lookOffset, .15f * target.transform.localScale.x, (myTransform.position - (target.transform.position + lookOffset)).normalized, out hit, Mathf.Abs(maxDistance))) distance = -hit.distance;


		tarPos = Vector3.Lerp(tarPos, PlayerController.I.myTransform.position, Time.deltaTime);
		tarFOV = Mathf.Lerp(tarFOV, Mathf.Clamp(PlayerController.I.MoveSpeed * 6, 35, 60), Time.deltaTime / 2);

		//myCamera.fieldOfView = tarFOV;
		//myTransform.LookAt(tarPos);

		SetCamera();

		if (LevelManager.I.starting && !LevelManager.I.readyToRestart)
		{
			Transform.position = Vector3.Lerp(Transform.position, startPosition.position, Time.deltaTime * 2);
			Transform.rotation = Quaternion.Lerp(Transform.rotation, startPosition.rotation, Time.deltaTime * 2);
		}
		if (LevelManager.I.readyToRestart)
		{
			Transform.position = Vector3.Lerp(Transform.position, failPosition.position, Time.deltaTime);
			Transform.rotation = Quaternion.Lerp(Transform.rotation, failPosition.rotation, Time.deltaTime);
		}
	}


	private void SetCamera ()
	{
		if (Distance > MaxDistance && Input.GetAxis("Mouse ScrollWheel") < 0) Distance += Input.GetAxis("Mouse ScrollWheel") * 5;
		if (Distance < -1 && Input.GetAxis("Mouse ScrollWheel") > 0) Distance += Input.GetAxis("Mouse ScrollWheel") * 5;

		if (TiltAngle != CurrentTiltAngle || PanAngle != CurrentPanAngle || Distance != CurrentDistance)
		{

			if (PanAngle < 0) PanAngle = (PanAngle % 360) + 360;
			else PanAngle = PanAngle % 360;

			if (PanAngle - CurrentPanAngle < -180) CurrentPanAngle -= 360;
			else if (PanAngle - CurrentPanAngle > 180) CurrentPanAngle += 360;

			if (TiltAngle < -80) TiltAngle = -80;
			else if (TiltAngle > 30) TiltAngle = 30;

			CurrentTiltAngle += (TiltAngle - CurrentTiltAngle) / (steps + 1);
			CurrentPanAngle += (PanAngle - CurrentPanAngle) / (steps + 1);
			CurrentDistance += (Distance - CurrentDistance) / (steps + 1);

			if ((Mathf.Abs(TiltAngle - CurrentTiltAngle) < .01) && (Mathf.Abs(PanAngle - CurrentPanAngle) < .01) && (Mathf.Abs(Distance - CurrentDistance) < .01))
			{
				CurrentTiltAngle = TiltAngle;
				CurrentPanAngle = PanAngle;
				CurrentDistance = Distance;
			}
		}

		if (CameraFollow)
		{
			Transform.position = new Vector3(
				target.transform.position.x + lookOffset.x + CurrentDistance * Mathf.Sin(CurrentPanAngle * Mathf.Deg2Rad) * Mathf.Cos(CurrentTiltAngle * Mathf.Deg2Rad),
				target.transform.position.y + lookOffset.y + CurrentDistance * Mathf.Sin(CurrentTiltAngle * Mathf.Deg2Rad),
				target.transform.position.z + lookOffset.z + CurrentDistance * Mathf.Cos(CurrentPanAngle * Mathf.Deg2Rad) * Mathf.Cos(CurrentTiltAngle * Mathf.Deg2Rad)
			);

			Transform.LookAt(target.transform.position + lookOffset);
		}
	}
}