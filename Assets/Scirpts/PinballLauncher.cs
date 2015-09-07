using UnityEngine;
using System.Collections;

public sealed class PinballLauncher : CapturableConstruction
{
	public string inputAxis;
	
    public Pinball pinballPrefab;
    private Animator animFsm;
	public float launchStrength = 5f;

    protected override void InitializeHealth()
    {
		currentHealth = MaxHealth;
    }

    public override float ComputeDist(Vector3 otherPos)
    {
        return (transform.position - otherPos).magnitude;
    }

    public override Vector3 NearestPos(Vector3 otherPos)
    {
        return transform.position;
    }

	public override int MaxHealth { get{ return  gameManager.pinballLauncherMaxHealth; } }

    protected override void Start()
    {
        base.Start();
        animFsm = GetComponent<Animator>();
        InitializeHealth();
		Debug.Assert(inputAxis != string.Empty);
        //respawnPinballTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();

        //if (IsCaptured)
        //{
        //    //visual effect...
        //    GetComponent<MeshRenderer>().enabled = false;
        //}
        //else
        //{
        //    respawnPinballTimer += Time.deltaTime;
        //    if (respawnPinballTimer >= respawnPinballInterval)
        //    {
        //        respawnPinballTimer = 0f;
        //        //SpawnPinball();
        //    }
        //}
    }

    public Pinball SpawnPinball()
    {
		//chargeLevel = Mathf.Clamp(chargeLevel, 0f, 1f);
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
        return Instantiate(pinballPrefab, transform.position, transform.rotation) as Pinball;
    }
}
