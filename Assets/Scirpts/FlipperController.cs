using UnityEngine;
using System.Collections;

public class FlipperController : MonoBehaviour
{
    private HingeJoint hinge;

    public string inputAxis;
    public bool inputSign;

    public const float idleAngle = 0f;
    public bool counterClockWise = false;
    public float maxAngle = 70f;
    public float fireForce = 100f;
    public float restoreForce = 10f;

	public KingdomBase kingdom;

    private void Start()
    {
		Debug.Assert(inputAxis != string.Empty);

		//Debug.Assert(kingdom != null);

        maxAngle = Mathf.Clamp(maxAngle, 0f, 90f);
        fireForce = Mathf.Clamp(fireForce, 10f, 500f);
        restoreForce = Mathf.Clamp(restoreForce, 5f, fireForce);

        hinge = GetComponent<HingeJoint>();
	
        //DO NOT ROTATE THE FLIPPER AROUND X OR Z AXIS!!!!!
        if (counterClockWise)
            hinge.axis = transform.up;
        else
			hinge.axis = -transform.up;

        hinge.useSpring = true;
        var spring = hinge.spring;
        spring.spring = restoreForce;
        spring.targetPosition = idleAngle;
        hinge.spring = spring;
    }

    private void Update()
    {

        if ((Input.GetAxis(inputAxis) > 0f && inputSign) ||
            (Input.GetAxis(inputAxis) < 0f && !inputSign))
        {
            var spring = hinge.spring;
            spring.spring = fireForce;
            spring.targetPosition = maxAngle;
            hinge.spring = spring;
        }
        //        if (Mathf.Approximately(hinge.angle, maxAngle) ||
        //            hinge.angle > maxAngle)
        else
        {
            var spring = hinge.spring;
            spring.spring = restoreForce;
            spring.targetPosition = idleAngle;
            hinge.spring = spring;
        }
    }

    private void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("Pinball"))
		{
			if(kingdom.name == "JusticeBase")
			{
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Pinball"),LayerMask.NameToLayer("Justice OneSide"),true);
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Pinball"),LayerMask.NameToLayer("Evil OneSide"),false);
			}
			else
			{
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Pinball"),LayerMask.NameToLayer("Justice OneSide"),false);
				Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Pinball"),LayerMask.NameToLayer("Evil OneSide"),true);
            }
//			other.gameObject.layer
		}
	}
}