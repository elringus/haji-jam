using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	private Transform myTransform;
	private Animator myAnimator;
	private Light myLight;
	private Vector3 prevPos;
	public UnityEngine.AI.NavMeshAgent myAgent;

	public Transform[] wPoints;
	public Transform[] lPoints;

	private Transform currentPoint;
	private float lookTimer;
	public float viewAngle;
	public float viewRange;
	public float alaramRange;
	public float detectRange;
	public float lookTime;

	private bool spotting;

	private AudioSource myAudio;
	public AudioClip[] sndMurs;
	public AudioClip sndSpotted;


	private void Awake ()
	{
		myTransform = transform;
		myAnimator = GetComponentInChildren<Animator>();
		myLight = GetComponentInChildren<Light>();
		myLight.spotAngle = viewAngle * 2;
		myLight.range = viewRange + 3;
		//myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		myAudio = GetComponent<AudioSource>();
	}

	private void Start ()
	{
		if (wPoints.Length > 0)
		{
			myAgent.destination = wPoints[0].position;
			currentPoint = wPoints[0];
		}
		else if (lPoints.Length > 0)
		{
			myAgent.enabled = false;
			currentPoint = lPoints[0];
			myTransform.LookAt(currentPoint.position);
			myTransform.eulerAngles = new Vector3(0, myTransform.eulerAngles.y, 0);
		}
	}

	private void Update ()
	{
        if (myTransform.name == "enemy-grandbaba" && Random.Range(0, 2000) == 0 && !myAudio.isPlaying) { myAudio.clip = sndMurs[Random.Range(0, 3)]; myAudio.Play(); }

		if (!LevelManager.I.readyToRestart && LevelManager.I.started)
		{
			if (wPoints.Length > 0 && Vector3.Distance(myTransform.position, currentPoint.position) < 1.5f) SetNextDestination();
			else if (lPoints.Length > 0)
			{
				myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.LookRotation((currentPoint.position - myTransform.position).normalized), Time.deltaTime * 4);
				myTransform.eulerAngles = new Vector3(0, myTransform.eulerAngles.y, 0);
				lookTimer += Time.deltaTime;
				if (lookTimer > lookTime) { lookTimer = 0; LookNextDestination(); }
			}

			if (CanSeePlayer())
			{
				if (PlayerController.I.InBox)
				{
					if (!spotting && PlayerController.I.MoveSpeed > 2) { StopCoroutine("SpotSomething"); StartCoroutine(SpotSomething(PlayerController.I.myTransform.position)); }
					if (spotting && PlayerController.I.MoveSpeed > 2 && Vector3.Distance(myTransform.position, PlayerController.I.myTransform.position) < viewRange / 2)
					{
						myLight.color = Color.red;
						if (!LevelManager.I.readyToRestart) LevelManager.I.RestartLevel();
					}
				}
				else
				{
					myLight.color = Color.red;
					if (!LevelManager.I.readyToRestart) LevelManager.I.RestartLevel();
				}
			}
			myLight.color = Color.Lerp(myLight.color, spotting ? Color.yellow : Color.white, Time.deltaTime * 2);

			if (LOS() && Vector3.Distance(myTransform.position, PlayerController.I.myTransform.position) < alaramRange && !PlayerController.I.Invisible)
			{
				if (Vector3.Distance(myTransform.position, PlayerController.I.myTransform.position) < detectRange)
				{
					if (!LevelManager.I.readyToRestart) { myLight.color = Color.red; LevelManager.I.RestartLevel(); }
				}
				else if (!PlayerController.I.InBox)
				{
					StopCoroutine("SpotSomething");
					StartCoroutine(SpotSomething(PlayerController.I.myTransform.position));
				}
			}
		}
		//else if (wPoints.Length > 0) myAgent.Stop();

		Animate();
	}

	private IEnumerator SpotSomething (Vector3 poisition)
	{
		myAudio.clip = sndSpotted;
		myAudio.Play();
		spotting = true;
		if (wPoints.Length > 0) myAgent.SetDestination(poisition);
		else currentPoint.position = poisition;
		yield return new WaitForSeconds(3);
		spotting = false;
		if (wPoints.Length > 0) SetNextDestination();
		else LookNextDestination();
	}

	private void SetNextDestination ()
	{
		for (int i = 0; i <= wPoints.Length; i++)
		{
			if (wPoints.Length == i + 1) { currentPoint = wPoints[0]; myAgent.SetDestination(currentPoint.position); break; }
			else if (wPoints[i] == currentPoint) { currentPoint = wPoints[i + 1]; myAgent.SetDestination(currentPoint.position); break; };
		}
	}

	private void LookNextDestination ()
	{
		for (int i = 0; i <= lPoints.Length; i++)
		{
			if (lPoints.Length == i + 1) { currentPoint = lPoints[0]; break; }
			else if (lPoints[i] == currentPoint) { currentPoint = lPoints[i + 1]; break; };
		}
	}

	private bool CanSeePlayer ()
	{
		if (Vector3.Distance(myTransform.position, PlayerController.I.myTransform.position) <= viewRange && LOS() &&
			Vector3.Angle(PlayerController.I.myTransform.position - myTransform.position, myTransform.forward) < viewAngle &&
			!PlayerController.I.Invisible) return true;
		else return false;
	}

	private bool LOS ()
	{
		RaycastHit hit;
		if (Physics.Raycast(myTransform.position + new Vector3(0, 1), (PlayerController.I.myTransform.position + new Vector3(0, 1) - myTransform.position).normalized, out hit, 1000) &&
			(hit.collider.CompareTag("Player") || hit.collider.GetComponent<Box>())) return true;
		else return false;
	}

	private void Animate ()
	{
		float speed = (myTransform.position - prevPos).magnitude / Time.deltaTime;

		myAnimator.SetFloat("Forward", speed / 4, .1f, Time.deltaTime);
		//myAnimator.SetFloat("Turn", 0, .1f, Time.deltaTime);
		//myAnimator.SetBool("Crouch", invisible || inBox ? true : false);
		myAnimator.SetBool("OnGround", true);
		//myAnimator.SetBool("OnGround", myController.isGrounded);
		//if (!myController.isGrounded) myAnimator.SetFloat("Jump", myController.velocity.y);
		//if (myController.isGrounded && myController.velocity.magnitude > 0) myAnimator.speed = speed;
		//else myAnimator.speed = 1;
		myAnimator.speed = speed < 1 ? 1 : speed / 2;

		prevPos = myTransform.position;
	}
}