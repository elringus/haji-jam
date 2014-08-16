using UnityEngine;
using System.Collections;

public class TrashBag : MonoBehaviour
{
	private Rigidbody myRigid;

	private void Awake ()
	{
		myRigid = rigidbody;
	}

	public void OnCollisionEnter (Collision colli)
	{
		foreach (ContactPoint contact in colli.contacts)
			if (contact.otherCollider.CompareTag("Player")) 
				myRigid.AddForceAtPosition(contact.normal, contact.point, ForceMode.Impulse);
	}
}