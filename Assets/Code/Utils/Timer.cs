using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
	public Timer( float time )
	{
		curTime = 0.0f;
		maxTime = time;
	}

	public bool Update( float dt )
	{
		if( curTime <= maxTime ) curTime += dt;

		return( IsDone() );
	}

	public void Reset()
	{
		curTime = 0.0f;
	}

	public void SetDur( float newDuration )
	{
		maxTime = newDuration;
	}
	
	public void Randomize()
	{
		curTime = Random.Range( 0.0f,maxTime );
	}

	public void Finish()
	{
		curTime = maxTime;
	}

	public bool IsDone()
	{
		return( curTime >= maxTime );
	}

	public float GetPercent()
	{
		return( Mathf.Min( curTime / maxTime,1.0f ) );
	}

	public float GetDur()
	{
		return( maxTime );
	}

	float curTime;
	[SerializeField] float maxTime;
}