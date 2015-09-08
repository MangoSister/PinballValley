using UnityEngine;
using System.Collections;

public class GatheringFlagController : MonoBehaviour
{
    public float inputSensitivity = 1f;
    public Vector2 lowerBound = new Vector2(-10f, 5f);
    public Vector2 upperBound = new Vector2(10f, 5f);

    public string horiAxisName;
    public string vertAxisName;

    public float smallMoveRange;

    private void Start()
    {
        inputSensitivity = Mathf.Clamp(inputSensitivity, 5f, 10f);
    }
    private void Update()
    {
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit info;
		if(Physics.Raycast(ray, out info, 10f, 1 << LayerMask.NameToLayer("Ground")))
			transform.rotation *= Quaternion.FromToRotation(transform.up, info.normal);
				
        float vertInput = Input.GetAxis(vertAxisName);
        vertInput = Mathf.Clamp(vertInput, -1f, 1f);
        
        float horiInput = Input.GetAxis(horiAxisName);
        horiInput = Mathf.Clamp(horiInput, -1f, 1f);
        
		Vector3 newPos = transform.position;
		newPos += (transform.right * horiInput  * inputSensitivity * Time.deltaTime);
		newPos += (transform.forward * vertInput * inputSensitivity * Time.deltaTime);

		if(Vector3.Dot(newPos, transform.right) > upperBound.x)
			newPos += (upperBound.x - Vector3.Dot(newPos, transform.right)) * transform.right;
		else if(Vector3.Dot(newPos, transform.right) < lowerBound.x)
			newPos += (lowerBound.x - Vector3.Dot(newPos, transform.right)) * transform.right;

		if(Vector3.Dot(newPos, transform.forward) > upperBound.y)
			newPos += (upperBound.y - Vector3.Dot(newPos, transform.forward)) * transform.forward;
		else if(Vector3.Dot(newPos, transform.forward) < lowerBound.y)
			newPos += (lowerBound.y - Vector3.Dot(newPos, transform.forward)) * transform.forward;


		transform.position = newPos;

    }
}
