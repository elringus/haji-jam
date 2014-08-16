using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour, IUsableObject
{
	private Transform myTransform;
	private bool charIn;
	private Vector3 normColliScale;
	private BoxCollider myColli;
	private ParticleSystem myParticles;

	public void Awake ()
	{
		myTransform = transform;
		myColli = collider as BoxCollider;
		normColliScale = myColli.size;
		myParticles = GetComponentInChildren<ParticleSystem>();
	}

	public void Update ()
	{
		if (charIn)
		{
			myTransform.position = PlayerController.I.MoveSpeed > 2 ? new Vector3(PlayerController.I.myTransform.position.x, Mathf.Lerp(myTransform.position.y, .5f + Mathf.Sin(Time.time * 15) / 3, Time.deltaTime * 5), PlayerController.I.myTransform.position.z) : PlayerController.I.myTransform.position + new Vector3(0, .5f);
			myTransform.rotation = PlayerController.I.myTransform.rotation;
			myTransform.eulerAngles = new Vector3(0, myTransform.eulerAngles.y - 20, 185);
		}
	}

	public void Use ()
	{
		if (Application.loadedLevelName == "lvl_2")
			TV.I.SwitchMGS();

		if (!PlayerController.I.InBox)
		{
			//GA.API.Design.NewEvent("usedbox-level-" + (Application.loadedLevel));
			PlayerController.I.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
			PlayerController.I.InBox = true;
			charIn = true;
			myColli.size = new Vector3(100, 10, 100);
			myParticles.Stop();
		}
		else
		{
			PlayerController.I.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
			PlayerController.I.InBox = false;
			charIn = false;
			myColli.size = normColliScale;
			myParticles.Play();
		}
	}
}