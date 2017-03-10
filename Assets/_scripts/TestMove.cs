using UnityEngine;
using UnityEngine.AI;

public class TestMove : MonoBehaviour
{
    public Transform Target;

    private void Awake ()
    {

    }

    private void Update ()
    {
        GetComponent<NavMeshAgent>().destination = Target.position;
    }
}
