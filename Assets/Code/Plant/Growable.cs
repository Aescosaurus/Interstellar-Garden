using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Growable
{
	public Growable( float startSize,float maxSize,float growthRateDays )
	{
		startGrowthSize = startSize;
		maxGrowthSize = maxSize;
		this.growthRateDays = growthRateDays;
	}

	public Growable() {}

	public bool Update( float modifier = 1.0f )
	{
		if( !fullyGrown )
		{
			curGrowth += GetGrowthRate() * modifier * globalGrowthRate * Time.deltaTime;
			if( curGrowth > maxGrowthSize )
			{
				fullyGrown = true;
				curGrowth = maxGrowthSize;
			}
			ScaleGrowTarget();
		}

		return( IsFullyGrown() );
	}

	public void SetGrowTarget( Transform target,bool resetCurGrowth = true )
	{
		growTarget = target;
		// origScale = growTarget.localScale.normalized;

		if( resetCurGrowth )
		{
			curGrowth = startGrowthSize;
			fullyGrown = false;
		}

		ScaleGrowTarget();
	}
	
	public void Reset()
	{
		curGrowth = startGrowthSize;
		fullyGrown = false;
	}

	void ScaleGrowTarget()
	{
		if( growTarget != null )
		{
			growTarget.localScale = Vector3.one * curGrowth;
		}
	}

	public float GetCurGrowth()
	{
		return( curGrowth );
	}

	public bool IsFullyGrown()
	{
		return( fullyGrown );
	}

	public float GetGrowthPercent()
	{
		return( ( curGrowth - startGrowthSize ) / ( maxGrowthSize - startGrowthSize ) );
	}

	float GetGrowthRate()
	{
		float growthAmount = ( maxGrowthSize - startGrowthSize );

		// idk why we have to multiply by 4 here but it seems to work so
		return( ( growthAmount / growthRateDays * 4.0f ) / ( DayNightCycle.GetDayDuration() ) );
	}

	public static void SetGlobalGrowthRate( float rate )
	{
		globalGrowthRate = rate;
	}

	public static float GetGlobalGrowthRate()
	{
		return( globalGrowthRate );
	}

	Transform growTarget = null;

	[SerializeField] float startGrowthSize = 0.05f;
	[SerializeField] float maxGrowthSize = 1.0f;
	// [SerializeField] float growthRate = 0.05f;
	[Tooltip( "How many days (daytime) it takes to grow to full.")]
	[SerializeField] float growthRateDays = 0.5f;

	float curGrowth = 0.0f;
	bool fullyGrown = false; // todo: can get rid of this & just use curGrowth >= maxGrowthSize in IsFullyGrown

	// Vector3 origScale;

	static float globalGrowthRate = 1.0f;
}