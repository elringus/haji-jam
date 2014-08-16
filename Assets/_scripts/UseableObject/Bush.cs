using UnityEngine;
using System.Collections;

public class Bush : MonoBehaviour, IUsableObject
{
	private Vector3 normColliScale;
	private BoxCollider myColli;
	private ParticleSystem myParticles;

	private void Awake ()
	{
		myColli = collider as BoxCollider;
		normColliScale = myColli.size;
		myParticles = GetComponent<ParticleSystem>();
	}

	public void Use ()
	{
		if (!audio.isPlaying) audio.Play();
		if (!PlayerController.I.Invisible)
		{
			PlayerController.I.Invisible = true;
			PlayerController.I.myTransform.position = transform.position;
			myColli.size = new Vector3(100, 100, 10);
			myParticles.Stop();

		}
		else
		{
			PlayerController.I.Invisible = false;
			myColli.size = normColliScale;
			myParticles.Play();
		}
	}
}