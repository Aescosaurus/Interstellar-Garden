using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		PlantManager.RegisterPlant( this );
	}

	void Update()
	{
		if( crowdingStats.GetCrowdingScore() < 0.0f )
		{
			hp -= crowdingStats.GetCrowdingHurtAmount() * Time.deltaTime;
			if( hp < 0.0f ) Kill();
		}
		else
		{
			Grow( GetGrowthRateModifier() );

			// no modifiers for crowding, if anything it should make this shorter
			if( lifespan.Update() ) Kill();

			if( hp < maxHP )
			{
				hp += hpRecovery * GetGrowthRateModifier() * Time.deltaTime;
			}
		}
	}

	protected abstract void Grow( float growthRateMod );

	protected void LaunchBerry( Rigidbody body,Vector3 launchDir )
	{
		body.AddForceAtPosition( launchDir * berryLaunchForce + new Vector3(
			Random.Range( -1.0f,1.0f ),
			0.0f,
			Random.Range( -1.0f,1.0f ) ).normalized * launchHorizVariation,
			transform.position,
			ForceMode.Impulse );
	}

	public virtual float GetGrowthPercent()
	{
		return( plantGrowth.GetGrowthPercent() );
	}

	public Crowdable GetCrowdingStats()
	{
		return( crowdingStats );
	}

	protected virtual void Kill()
	{
		PlantManager.DeregisterPlant( this );
		Destroy( gameObject );
	}

	float GetGrowthRateModifier()
	{
		return( crowdingStats.GetCrowdingScore() * DayNightCycle.GetSunPercent() );
	}

	float hp = 0.0f; // start at 0 hp
	[SerializeField] float maxHP = 1.5f;
	// hp recovery doubles as hp growth, the longer you live the stronger you become
	[SerializeField] float hpRecovery = 0.05f;

	[SerializeField] protected Growable plantGrowth = new Growable();
	
	[SerializeField] protected Crowdable crowdingStats = new Crowdable();

	[SerializeField] Growable lifespan = new Growable();
	
	[Header( "Reproduction" )]
	[SerializeField] float berryLaunchForce = 0.5f;
	[SerializeField] float launchHorizVariation = 0.2f;
}