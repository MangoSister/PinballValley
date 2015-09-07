using UnityEngine;
using System.Collections;

public class StatusBar : MonoBehaviour 
{
	public AttackableUnit unit;
	public Camera mainCamera;

	private GameObject healthImg;

	// Use this for initialization
	void Start () 
	{
		Debug.Assert(unit != null);
		Debug.Assert(mainCamera != null);
		healthImg = transform.FindChild("CurrHealth").gameObject;
		Debug.Assert(healthImg != null);
	}
	
	// Update is called once per frame
	void Update () 
	{
		var scale = healthImg.transform.localScale;
		scale.x = Mathf.Clamp01(((float)unit.currentHealth / (float)unit.MaxHealth));
		healthImg.transform.localScale = scale;

//		transform.LookAt(mainCamera.transform.position, Vector3.up);
		transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
	}
}
