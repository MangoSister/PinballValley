using UnityEngine;
using System.Collections;

public abstract class CapturableConstruction : AttackableUnit
{
    public GameObject gameManagerObj;
    public GameManager gameManager { get; private set; }

    private float damagableRange;

    //public bool IsCaptured { get; set; }

    protected abstract void InitializeHealth();
    public abstract float ComputeDist(Vector3 otherPos);
    public abstract Vector3 NearestPos(Vector3 otherPos);

    protected virtual void Start()
    {
        Debug.Assert(gameManagerObj != null);
        gameManager = gameManagerObj.GetComponent<GameManager>();
        Debug.Assert(gameManager != null);
        
    }

    protected virtual void Update()
    {
    }
}
