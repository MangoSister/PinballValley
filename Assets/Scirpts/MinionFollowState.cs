﻿using UnityEngine;
using System.Collections;

public class MinionFollowState : StateMachineBehaviour 
{
	
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
        animator.SetBool("Arrived", false);
        var minion = animator.gameObject.GetComponent<Minion>();

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		var minion = animator.gameObject.GetComponent<Minion>();
        minion.navMeshAgent.SetDestination(minion.gatheringFlag.transform.position);
        minion.navMeshAgent.Resume();

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

        animator.SetBool("Arrived", minion.HasArrived());

	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
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
