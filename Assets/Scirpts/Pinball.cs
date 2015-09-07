using UnityEngine;
using System.Collections;

public class Pinball : MonoBehaviour 
{
	public int damageFactor;
	public float downPressure = 10f;
	public float maxSpeed = 10f;

	private Vector3 prevPos;
	private Vector3 prevVelo;
	private Vector2 prevAngularVelo;
	private Rigidbody rigidBody;

	public int ComputeDamage()
	{
		return damageFactor;
	}

	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//add constant down pressure
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit info;
		if(Physics.Raycast(ray, out info, 10f, 1 << LayerMask.NameToLayer("Ground")))
			rigidBody.AddForce(-info.normal * downPressure,ForceMode.Acceleration);

		//limit speed
		if(rigidBody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
			rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;

		prevPos = rigidBody.position;
		prevVelo = rigidBody.velocity;
		prevAngularVelo = rigidBody.angularVelocity;
	}

	private void OnCollisionEnter(Collision collision) 
	{
		if(collision.gameObject.CompareTag("Minion"))
		{
			collision.gameObject.GetComponent<Minion>().currentHealth = 0;
			rigidBody.position = prevPos;
			rigidBody.velocity = prevVelo;
			rigidBody.angularVelocity = prevAngularVelo;
		}
	}
}
