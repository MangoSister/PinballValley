using UnityEngine;
using System.Collections;

public class PinballLauncherReadyState : StateMachineBehaviour 
{
    public float waitMaxTime = 5f;
    public float waitTime = 0f;
	public float holdDownMaxTime = 2f;
	private float holdDownTimer = 0f;
	private bool startHeldDown = false;
	private Pinball pinball;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
        waitTime = 0f;
        holdDownTimer = 0f;
		startHeldDown = false;
        animator.SetBool("IsLaunched", false);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		PinballLauncher launcher = animator.gameObject.GetComponent<PinballLauncher>();

		if (!launcher.gameManager.pinballInMap && Random.value > 0.5f)
		{
			pinball = launcher.SpawnPinball();
			pinball.GetComponent<Rigidbody>().isKinematic = true;
			launcher.gameManager.pinballInMap = true;
		}
		
		if(pinball != null)
		{
            if (!startHeldDown && !Input.GetButtonDown(launcher.inputAxis))
            {
                waitTime += Time.deltaTime;
                if (waitTime >= waitMaxTime)
                {
                    pinball.GetComponent<Rigidbody>().isKinematic = false;
                    pinball.gameObject.GetComponent<Rigidbody>().AddForce
                        (new Vector3(0, 0, Random.value * launcher.launchStrength), ForceMode.VelocityChange);
                    animator.SetBool("IsLaunched", true);
                }
            }
			else if(!startHeldDown && Input.GetButtonDown(launcher.inputAxis))
			{
				startHeldDown = true;
			}

			else if(startHeldDown && Input.GetButton(launcher.inputAxis))
			{
				holdDownTimer += Time.deltaTime;
				holdDownTimer = Mathf.Clamp(holdDownTimer, 0f, holdDownMaxTime);
			}
			else if(startHeldDown && Input.GetButtonUp(launcher.inputAxis))
			{	
				startHeldDown = false;
				float chargeLevel = Mathf.Clamp(holdDownTimer, 0f, holdDownMaxTime) / holdDownMaxTime;
				Mathf.Clamp(chargeLevel, 0.2f,1f);
				pinball.GetComponent<Rigidbody>().isKinematic = false;
				pinball.gameObject.GetComponent<Rigidbody>().AddForce
					(new Vector3(0 , 0, chargeLevel * launcher.launchStrength), ForceMode.VelocityChange);
                //				

				//launcher.gameManager.pinballInMap = true;
				animator.SetBool("IsLaunched",true);
			}
		}

		if (launcher.currentHealth <= 0)
			animator.SetBool("IsCaptured", true);

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
	
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
