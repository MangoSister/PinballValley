using UnityEngine;
using System.Collections;

public class SingleBumper : Bumper
{		
	protected override void Start()
	{
		base.Start();
	}
	
    public override float ComputeDist(Vector3 otherPos)
    {
		return (otherPos - NearestPos(otherPos)).magnitude;
    }

    public override Vector3 NearestPos(Vector3 otherPos)
    {  
		return gameObject.GetComponent<Collider>().ClosestPointOnBounds(otherPos);
    }
}
