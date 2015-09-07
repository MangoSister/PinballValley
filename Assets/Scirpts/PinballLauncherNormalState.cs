using UnityEngine;
using System.Collections;

public class PinballLauncherNormalState : StateMachineBehaviour
{
    private static readonly float spawnPinballInterval = 30f;
    private float spawnPinballTimer = 0f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var pinballLauncher = animator.gameObject.GetComponent<PinballLauncher>();
        pinballLauncher.currentHealth = pinballLauncher.gameManager.pinballLauncherMaxHealth;
        //visual effect when activated
        animator.gameObject.GetComponent<MeshRenderer>().enabled = true;
        spawnPinballTimer = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var pinballLauncher = animator.gameObject.GetComponent<PinballLauncher>();

        //regularly spawn pinball, but there should be only one or no pinball in total
        spawnPinballTimer += Time.deltaTime;
        if (spawnPinballTimer >= spawnPinballInterval)
        {
            spawnPinballTimer -= spawnPinballInterval;
            if (!pinballLauncher.gameManager.pinballInMap)
            {
                //pinballLauncher.SpawnPinball();
                pinballLauncher.gameManager.pinballInMap = true;
            }
        }

        if (pinballLauncher.currentHealth <= 0)
            animator.SetBool("IsCaptured", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
