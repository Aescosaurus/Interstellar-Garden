using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightCycle
	:
	MonoBehaviour
{
	void Start()
	{
		dayDuration = dayDurationMinutes * 60.0f;
		sunRotSpd = 360.0f / ( dayDuration );

		dayCounterText.text = "Day 1";

		dailySunUnits = CalcDailySunUnits();
	}

	void Update()
	{
		if( disableNight )
		{
			transform.eulerAngles = new Vector3( 50.0f,-30.0f,0.0f );
			sunPercent = 1.0f;
			return;
		}

		transform.Rotate( sunRotSpd * Time.deltaTime,0.0f,0.0f );
		sunPercent = sunPercentCurve.Evaluate( transform.eulerAngles.x );

		dayCounter += sunRotSpd * Time.deltaTime;
		if( dayCounter > 360.0f )
		{
			++nDays;
			dayCounter -= 360.0f;
			dayCounterText.text = "Day " + nDays;
		}
	}

	public static void ToggleNight( bool enabled )
	{
		disableNight = !enabled;
	}

	public static float GetSunPercent()
	{
		return( sunPercent );
	}

	// seconds
	public static float GetDayDuration()
	{
		return( dayDuration );
	}

	public static float GetDailySunUnits()
	{
		return( dailySunUnits );
	}

	// calculate total amount of sun a plant will receive in a day
	float CalcDailySunUnits()
	{
		int steps = 180;
		float dt = ( dayDurationMinutes * 60.0f / 2.0f ) / ( float )steps;
		float total = 0.0f;
		for( int i = 0; i < steps; ++i )
		{
			total += sunPercentCurve.Evaluate( i ) * dt;
		}
		return( total );
	}

	[SerializeField] TextMeshProUGUI dayCounterText = null;
	float dayCounter = 0.0f;
	int nDays = 1;

	[SerializeField] float dayDurationMinutes = 10.0f;
	float sunRotSpd;
	static float sunPercent = 0.0f;
	static float dayDuration;
	[SerializeField] AnimationCurve sunPercentCurve = AnimationCurve.Linear( 0,0,1,1 );

	static float dailySunUnits;

	static bool disableNight = false;
}