using UnityEngine;
using System.Collections;
using System;

public abstract class Bumper : CapturableConstruction
{    
	public override int MaxHealth { get{return gameManager.bumperMaxHealth;} }
    protected override void InitializeHealth()
    {
        currentHealth = gameManager.bumperMaxHealth;
    }

    protected override void Start()
    {
        base.Start();
        InitializeHealth();
    }

    protected override void Update()
    {
        base.Update();
    }
}
