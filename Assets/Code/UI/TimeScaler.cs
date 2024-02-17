using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaler
	:
	MonoBehaviour
{
	void Start()
	{
		SetScaleText();
	}

	public void SetTimeScale( float scale )
	{
		Time.timeScale = scale;
		SetScaleText();
	}

	public void SetGrowthRate( float rate )
	{
		Growable.SetGlobalGrowthRate( rate );
		SetScaleText();
	}

	void SetScaleText()
	{
		var scale = ( int )( Time.timeScale * Growable.GetGlobalGrowthRate() );
		scaleText.text = "Total Scale: " + scale + "x";
	}

	public void DisableNight()
	{
		DayNightCycle.ToggleNight( !disableNightCheckbox.isOn );
	}

	[SerializeField] TextMeshProUGUI scaleText = null;

	[SerializeField] Toggle disableNightCheckbox = null;
}