using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBerryBush
	:
	Plant
{
	protected override void Start()
	{
		base.Start();
		
		berrySpot = transform.Find( "BerrySpot" );
		plantGrowth.SetGrowTarget( transform );
	}

	protected override void Grow( float growthRateMod )
	{
		if( plantGrowth.Update( growthRateMod ) )
		{
			if( berry == null )
			{
				berry = Instantiate( berryPrefab,berrySpot.position,Quaternion.identity );
				// berry.GetComponent<Berry>().enabled = false;
				berryGrowth.SetGrowTarget( berry.transform );
			}
			
			if( crowdingStats.GetCrowdingScore() > crowdingStopReproduce )
			{
				if( berryGrowth.Update( growthRateMod ) )
				{
					// launch berry
					var body = berry.AddComponent<Rigidbody>();
					LaunchBerry( body,transform.up );
					// body.AddForceAtPosition( Vector3.up * berryLaunchForce + new Vector3(
					// 	Random.Range( -1.0f,1.0f ),
					// 	0.0f,
					// 	Random.Range( -1.0f,1.0f ) ).normalized * launchHorizVariation,
					// 	transform.position,
					// 	ForceMode.Impulse );
					
					berry.GetComponent<Berry>().enabled = true;
					berry = null;

					// if( --reproductionLifespan < 0 ) Kill();
				}
			}
			else
			{
				Destroy( berry );
				berry = null;
			}
		}
	}

	protected override void Kill()
	{
		if( berry != null ) Destroy( berry );

		base.Kill();
	}
	
	Transform berrySpot;
	[SerializeField] GameObject berryPrefab = null;
	GameObject berry = null;

	[SerializeField] Growable berryGrowth = new Growable();
	
	[Tooltip( "If crowding score is less than this val we stop producing berries")]
	[SerializeField] float crowdingStopReproduce = 0.5f;
	// [SerializeField] AnimationCurve crowdingCurve;
	// [Tooltip( "Destroy after this many times reproducing")]
	// [SerializeField] int reproductionLifespan = 5;
}