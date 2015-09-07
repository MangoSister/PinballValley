using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : AttackableUnit
{
    public GatheringFlagController gatheringFlag;

    public int attackDamage = 1;
    public float attackMinionRange = 0.8f;
    public float attackConstructionRange = 1.2f;
	public float sightRange = 1.5f;
    public float attackInterval = 1f;

	private AttackableUnit currTarget;

	public NavMeshAgent navMeshAgent {private set; get;}

    public enum TargetType { None, Minion, Construction };

    public bool HasArrived()
	{
		if (!navMeshAgent.pathPending)
		{
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
				{
					return true;
                }
            }
        }
		return false;
    }
    
	public override int MaxHealth { get{return kingdom.gameManager.minionMaxHealth;} }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Assert(navMeshAgent != null);
		currentHealth = MaxHealth;
		currTarget = null;

        Debug.Assert(attackMinionRange < sightRange && attackConstructionRange < sightRange);
    }

    private void Update()
    {
		if(currentHealth <= 0)
		{
			Die();
			return;
		}
    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.DrawSphere(transform.position, attackMinionRange);
//    }

    private AttackableUnit RetriveUnit(Collider c)
    {
		if (c.gameObject.tag != "Attackable" && c.gameObject.tag != "Minion")
            return null;
		//minion || circular bumper || pinball launcher
        AttackableUnit unit = c.gameObject.GetComponent<AttackableUnit>();

		//triangular bumper
        if (unit == null)
        {
            unit = c.gameObject.transform.parent.gameObject.GetComponent<AttackableUnit>();
            if (unit == null)
                throw new UnityException("Couldn't retrive attackable unit from collider");
        }
        return unit;
    }


    public TargetType UpdateTarget()
	{
        //target priority: minion > construction

        //if current target is valid && minion -> unchanged
		if(currTarget != null && currTarget.currentHealth > 0 && currTarget is Minion &&
		   (currTarget.gameObject.transform.position - transform.position).sqrMagnitude <= sightRange * sightRange)
			return TargetType.Minion;

        //else search and find the one with highest priority (and the nearest)
        List<Collider> colliders = (new List<Collider>(Physics.OverlapSphere(transform.position, sightRange))).
            FindAll((Collider c) =>
            {
                var unit = RetriveUnit(c);
                return unit != null &&
					!ReferenceEquals(unit.kingdom, kingdom) &&
						unit.currentHealth > 0;
            });
		
		//no enemy, return
		if(colliders.Count == 0)
			return TargetType.None;

        //choose target based on priority and distance
        TargetType currType = TargetType.Construction;
        float minDistSq = float.MaxValue;
		//Collider targetCollider = null;
		foreach(var c in colliders)
		{
            var unit = RetriveUnit(c);
            if (unit is Minion && currType == TargetType.Construction)
                currType = TargetType.Minion;
            else if (unit is CapturableConstruction && 
                currType == TargetType.Minion)
                continue;

            float currDistSq = (c.gameObject.transform.position - transform.position).sqrMagnitude;
			if(currDistSq < minDistSq)
			{
				minDistSq = currDistSq;
                currTarget = unit;
            }
        }
        return currType;
    }

	public void ChaseEnemy()
	{
		if(currTarget == null)
			return;
        float range;
        if (currTarget is Minion)
        {
            range = attackMinionRange;

            if ((currTarget.gameObject.transform.position - transform.position).sqrMagnitude >
                range * range)
            {
                navMeshAgent.SetDestination(currTarget.gameObject.transform.position);
                navMeshAgent.Resume();
            }
            else navMeshAgent.Stop();
        }
        else
        {
            range = attackConstructionRange;
            Vector3 nearest = (currTarget as CapturableConstruction).NearestPos(transform.position);
            if ((nearest - transform.position).sqrMagnitude > range * range)
            {
                navMeshAgent.SetDestination(nearest);
                navMeshAgent.Resume();
            }
            else navMeshAgent.Stop();
        }

	}
    
	public void AttackEnemy()
    {
		if(currTarget == null)
			return;

        float range;
        if (currTarget is Minion)
        {
            range = attackMinionRange;
            if ((currTarget.gameObject.transform.position - transform.position).sqrMagnitude <=
                range * range)
            {
                currTarget.currentHealth -= attackDamage;
                if (currTarget.currentHealth <= 0)
                    currTarget = null;
            }
        }
        else
        {
            range = attackConstructionRange;
            Vector3 nearest = (currTarget as CapturableConstruction).NearestPos(transform.position);
            if ((nearest - transform.position).sqrMagnitude <= range * range)
            {
                currTarget.currentHealth -= attackDamage;
                if (currTarget.currentHealth <= 0)
                    currTarget = null;
            }
        }


    }

    private void Die()
    {
        Destroy(gameObject);
        kingdom.CurrMinionNum--;
    }
}
