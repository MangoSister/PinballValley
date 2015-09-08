using UnityEngine;
using System.Collections;

public class KingdomBase : AttackableUnit
{
    public GameObject gameManagerObj;
    public GameManager gameManager { get; private set; }
    public GameObject gatheringFlag;
    public Minion MinionPrefab;
    public Vector3 spawningOffset;

    public Color kingdomColor;

    public float minionSpawnInterval = 5f;
    private float minionSpawnTimer;
    public int maxMinionNum = 5;
    public int CurrMinionNum { get; set; }

	public override int MaxHealth { get {return gameManager.baseMaxHealth; } }

	private void Start()
	{
        Debug.Assert(gameManagerObj != null);
        gameManager = gameManagerObj.GetComponent<GameManager>();
        Debug.Assert(gameManager != null);
		currentHealth = MaxHealth;

        Debug.Assert(gatheringFlag != null);

        Debug.Assert(MinionPrefab != null);

        minionSpawnInterval = Mathf.Clamp(minionSpawnInterval, 1f, 10f);
        minionSpawnTimer = 0f;

        CurrMinionNum = 0;
    }

    private void Update()
    {
        minionSpawnTimer += Time.deltaTime;
        if (minionSpawnTimer >= minionSpawnInterval)
        {
            minionSpawnTimer = 0f;
            if (CurrMinionNum < maxMinionNum)
            {
                SpawnMinion();
                CurrMinionNum++;
            }
        }
    }

	private void OnTriggerEnter(Collider other)
	{
		if(!other.gameObject.CompareTag( "Pinball"))
			return;

		//destroy pinball
		Destroy(other.gameObject);
        gameManager.pinballInMap = false;

        //pinball explosion special effect & sound

        if (gameManager.IsGameOver)
            return;

        //pinball deals damage to base
        GameObject pinballObj = other.gameObject;
		currentHealth -= pinballObj.GetComponent<Pinball>().ComputeDamage();

		//UI stuff

		//base has been destroyed, one team is out
		if(currentHealth <= 0)
		{
            //base explosion effect

			gameManager.OnBaseDestroy();
		}

	}

    private void SpawnMinion()
    {
        Minion newMinion = Instantiate(MinionPrefab, transform.position + spawningOffset, Quaternion.identity) as Minion;
        newMinion.gatheringFlag = gatheringFlag.GetComponent<GatheringFlagController>();
        newMinion.kingdom = this;
        newMinion.GetComponent<MeshRenderer>().material.color = kingdomColor;
    }
}
