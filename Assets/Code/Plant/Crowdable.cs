using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crowdable
{
	public void CalcCrowdingScore( Plant self,List<Plant> nearbyPlants )
	{
		crowdingScore = 1.0f;
		foreach( var plant in nearbyPlants )
		{
			var growth = plant.GetGrowthPercent();
			var dist = ( self.transform.position - plant.transform.position ).sqrMagnitude;
			if( dist < Mathf.Pow( nearbyKillDist,2 ) ) crowdingScore -= nearbyKillAmount * growth;
			else crowdingScore -= nearbySlowAmount * growth;
		}
	}

	// max dist that we care about other plants at
	public float GetNearbyPlantSpacing()
	{
		return( nearbySlowDist );
	}

	public float GetCrowdingScore()
	{
		return( crowdingScore );
	}

	public float GetCrowdingHurtAmount()
	{
		return( crowdingHurtRate * ( 1.0f - crowdingScore ) );
	}
	
	[SerializeField] float nearbySlowDist = 2.0f;
	[SerializeField] float nearbySlowAmount = 0.3f;
	[SerializeField] float nearbyKillDist = 1.0f;
	[SerializeField] float nearbyKillAmount = 1.1f;
	[SerializeField] float crowdingHurtRate = 0.2f;
	float crowdingScore = 0.0f;
}