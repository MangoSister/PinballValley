using UnityEngine;
using System.Collections;

public abstract class AttackableUnit : MonoBehaviour
{
    public int currentHealth;
	public abstract int MaxHealth { get; }
    public KingdomBase kingdom;
}
