using UnityEngine;
using System.Collections;

public class MinionAttackMinionState : StateMachineBehaviour 
{
	private float attackInterval;
	private float attackTimer;
    private Vector3 enterFlagPos;
    private GatheringFlagController flag;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
        animator.SetBool("FlagMove", false);
        var minion = animator.gameObject.GetComponent<Minion>();
        flag = minion.gatheringFlag;
        enterFlagPos = flag.transform.position;
        attackInterval = animator.gameObject.GetComponent<Minion>().attackInterval;
		attackTimer = 0f;	
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		var minion = animator.gameObject.GetComponent<Minion>();
        
        minion.ChaseEnemy();
		attackTimer += Time.deltaTime;
		if(attackTimer >= attackInterval)
		{
			attackTimer -= attackInterval;
			minion.AttackEnemy();
		}

        Vector3 currFlagPos = flag.transform.position;

        if ((enterFlagPos - currFlagPos).sqrMagnitude >
        flag.smallMoveRange *
        flag.smallMoveRange)
            animator.SetBool("FlagMove", true);
        else animator.SetBool("FlagMove", false);

        Minion.TargetType targetType = minion.UpdateTarget();
        if (targetType == Minion.TargetType.Minion)
        {
            animator.SetTrigger("TargetMinion");
            animator.ResetTrigger("TargetConstruction");
            animator.ResetTrigger("NoTarget");
        }
        else if (targetType == Minion.TargetType.Construction)
        {
            animator.SetTrigger("TargetConstruction");
            animator.ResetTrigger("TargetMinion");
            animator.ResetTrigger("NoTarget");
        }
        else
        {
            animator.SetTrigger("NoTarget");
            animator.ResetTrigger("TargetConstruction");
            animator.ResetTrigger("TargetMinion");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger("Arrived");
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
