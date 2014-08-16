using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
	#region SINGLETON
	private static Arrow _instance;
	public static Arrow I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(Arrow)) as Arrow;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	private Transform myTransform;
	private Renderer myRenderer;
	private Vector3 tarPos;

	private void Awake ()
	{
		myTransform = transform;
		myRenderer = renderer;
		myRenderer.enabled = false;
	}

	private void Update ()
	{
		//myTransform.Rotate(new Vector3(0, Time.deltaTime * 100, 0));
		myTransform.position = new Vector3(tarPos.x, Mathf.Lerp(myTransform.position.y, tarPos.y + Mathf.Sin(Time.time * 10) / 1.5f, Time.deltaTime * 5), tarPos.z); ;

		Ray ray = CameraController.I.myCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 1000, ~(1 << 9)) && hit.collider.CompareTag("AOB"))
		{
			SetArrow(hit.collider.gameObject);
			if (Vector3.Distance(PlayerController.I.myTransform.position, hit.transform.position) < 3)
			{
				myRenderer.material.color = new Color32(188, 255, 0, 130);
				if (Input.GetMouseButtonUp(0)) ((IUsableObject)hit.collider.GetComponent(typeof(IUsableObject))).Use();
			}
			else myRenderer.material.color = new Color32(255, 206, 0, 130);
		}
		else SetArrow(null);
	}

	private void SetArrow (GameObject target)
	{
		if (target != null && !PlayerController.I.Invisible && !PlayerController.I.InBox)
		{
			myRenderer.enabled = true;
			tarPos = target.transform.position + new Vector3(0, 3, 0);
			if (myTransform.position.x != tarPos.x || myTransform.position.z != tarPos.z) myTransform.position = tarPos;
		}
		else myRenderer.enabled = false;
	}
}