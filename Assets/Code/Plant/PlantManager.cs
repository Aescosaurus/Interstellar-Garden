using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager
	:
	MonoBehaviour
{
	void Start()
	{
		if( plantMan == null ) plantMan = this;
	}

	public static void RegisterPlant( Plant plant )
	{
		plantList.Add( plant );

		foreach( var curPlant in plantList )
		{
			curPlant.GetCrowdingStats().CalcCrowdingScore( curPlant,
				GetNearbyPlants( curPlant,curPlant.GetCrowdingStats().GetNearbyPlantSpacing() ) );
		}
	}

	// public static void SetupCrowdable( GameObject self,Crowdable crowdingStats )
	// {
	// 	crowdingStats.CalcCrowdingScore(
	// 		GetAllNearbyPlantDists( self,crowdingStats.GetNearbyPlantSpacing() ) );
	// }

	public static void DeregisterPlant( Plant plant )
	{
		plantList.Remove( plant );
	}

	static List<float> GetNearbyPlantDists( Plant self,float dist )
	{
		float sqrDist = dist * dist;
		var dists = new List<float>();
		foreach( var plant in plantList )
		{
			if( plant != self )
			{
				var curDist = ( plant.transform.position - self.transform.position ).sqrMagnitude;
				if( curDist < sqrDist ) dists.Add( curDist );
			}
		}
		return( dists );
	}

	static List<Plant> GetNearbyPlants( Plant self,float dist )
	{
		float sqrDist = dist * dist;
		var plants = new List<Plant>();
		foreach( var plant in plantList )
		{
			if( plant != self )
			{
				var curDist = ( plant.transform.position - self.transform.position ).sqrMagnitude;
				if( curDist < sqrDist ) plants.Add( plant );
			}
		}
		return( plants );
	}

	static List<float> GetAllNearbyPlantDists( GameObject self,float dist )
	{
		float sqrDist = dist * dist;
		var dists = new List<float>();
		foreach( var plant in plantList )
		{
			var curDist = ( plant.transform.position - self.transform.position ).sqrMagnitude;
			if( curDist < sqrDist ) dists.Add( curDist );
		}
		return( dists );
	}

	static PlantManager plantMan = null;

	static List<Plant> plantList = new List<Plant>();
}