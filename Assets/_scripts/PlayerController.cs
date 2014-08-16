using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	#region SINGLETON
	private static PlayerController _instance;
	public static PlayerController I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(PlayerController)) as PlayerController;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	[HideInInspector]
	public Transform myTransform;
	[HideInInspector]
	public Animator myAnimator;
	[HideInInspector]
	public CharacterController MyController;
	public float MoveSpeed;
	public bool Invisible;
	public bool InBox;

	private void Awake ()
	{
		myTransform = transform;
		myAnimator = GetComponentInChildren<Animator>();
		MyController = GetComponent<CharacterController>();
	}

	private void Update ()
	{
		if (LevelManager.I.started && !LevelManager.I.readyToRestart && (!LevelManager.clickToRun || (LevelManager.clickToRun && Input.GetMouseButton(0))))
		{
			MoveSpeed = Mathf.Clamp(Vector3.Distance(myTransform.position, Pointer.I.Transform.position) * 2, 0, InBox ? 2.2f : 15);

			myTransform.LookAt(Pointer.I.Transform.position);
			myTransform.eulerAngles = new Vector3(0, myTransform.eulerAngles.y, 0);
			if (!Invisible && Vector3.Distance(myTransform.position, Pointer.I.Transform.position) > 2) MyController.Move(myTransform.TransformDirection(Vector3.forward) * Time.deltaTime * MoveSpeed);

			if (!MyController.isGrounded) MyController.Move(Vector3.down * Time.deltaTime * 5);
		}

		Animate();
	}

	private void Animate ()
	{
		myAnimator.SetFloat("Forward", MoveSpeed < 4 ? 0 : MoveSpeed / 4, .2f, Time.deltaTime);
		//myAnimator.SetFloat("Turn", 0, .1f, Time.deltaTime);
		myAnimator.SetBool("Crouch", Invisible || InBox || LevelManager.I.readyToRestart ? true : false);
		myAnimator.SetBool("OnGround", true);
		//myAnimator.SetBool("OnGround", myController.isGrounded);
		//if (!myController.isGrounded) myAnimator.SetFloat("Jump", myController.velocity.y);
		//if (myController.isGrounded && myController.velocity.magnitude > 0) myAnimator.speed = speed;
		//else myAnimator.speed = 1;
		myAnimator.speed = MoveSpeed < 2 ? 1 : MoveSpeed / 4;
	}
}