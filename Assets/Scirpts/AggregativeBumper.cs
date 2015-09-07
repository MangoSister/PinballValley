using UnityEngine;
using System.Collections;

public class AggregativeBumper : Bumper
{
	public override float ComputeDist(Vector3 otherPos)
	{
		float minDistSq = float.MaxValue;
		foreach (Transform child in transform)
		{
			var cloestPt = child.gameObject.GetComponent<Collider>().ClosestPointOnBounds(otherPos);
			var currDistSq = (cloestPt - otherPos).sqrMagnitude;
		    if (currDistSq < minDistSq)
		    	minDistSq = currDistSq;
		}
		return Mathf.Sqrt(minDistSq);
	}

	public override Vector3 NearestPos(Vector3 otherPos)
	{
		Vector3 result = transform.position;
		float minDistSq = float.MaxValue;
        foreach (Transform child in transform)
		{
			if(child.gameObject.GetComponent<Collider>() == null)
				continue;
			var cloestPt = child.gameObject.GetComponent<Collider>().ClosestPointOnBounds(otherPos);
			var currDistSq = (cloestPt - otherPos).sqrMagnitude;
            if (currDistSq < minDistSq)
            {
                minDistSq = currDistSq;
                result = cloestPt;
            }
        }
        return result;
	}
}
