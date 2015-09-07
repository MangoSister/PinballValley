using UnityEngine;
using System.Collections;

public class BumperReaction : MonoBehaviour
{
    public float bounceStrength = 100f;
    public float bounceRange = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Pinball"))
            return;
		collision.rigidbody.AddExplosionForce(bounceStrength, transform.position, bounceRange);

    }
}