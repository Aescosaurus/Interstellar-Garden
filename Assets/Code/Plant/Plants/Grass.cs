using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass
	:
	Plant
{
	protected override void Start()
	{
		base.Start();

		SpawnBlade();
	}

	protected override void Grow( float growthRateMod )
	{
		if( seed != null )
		{
			if( seedGrowth.Update( growthRateMod ) )
			{
				seed.GetComponent<Berry>().enabled = true;
				var body = seed.AddComponent<Rigidbody>();
				LaunchBerry( body,seed.transform.forward );
				seedGrowth.Reset();
				seed = null;
			}
		}
		else if( bladeGrowth.Update( growthRateMod ) )
		{
			if( Random.Range( 0.0f,1.0f ) < seedSpawnChance || blades.Count >= maxBlades - 1 )
			{
				var chosenBlade = blades[Random.Range( 0,blades.Count )];
				seed = Instantiate( seedPrefab,chosenBlade.Find( "SeedSpot" ).position,chosenBlade.rotation );
				seedGrowth.SetGrowTarget( seed.transform );
			}
			else
			{
				SpawnBlade();
				bladeGrowth.Reset();
			}
		}
		else
		{
			// scale blade
			var growthScale = bladeGrowth.GetCurGrowth();
			var curBlade = blades[blades.Count - 1];

			var scale = curBlade.localScale;
			scale.z = growthScale;
			curBlade.localScale = scale;

			curBlade.position = transform.position + curBlade.forward * ( growthScale / 2.0f );
		}
	}

	void SpawnBlade()
	{
		var blade = Instantiate( bladePrefab,transform.position,Quaternion.LookRotation( transform.up ),transform );
		blade.transform.Rotate( new Vector3(
			Random.Range( -1.0f,1.0f ),
			Random.Range( -1.0f,1.0f ),
			Random.Range( -1.0f,1.0f ) ).normalized * Random.Range( 0.0f,bladeAngDev ) );
		blades.Add( blade.transform );
	}

	protected override void Kill()
	{
		base.Kill();

		Destroy( seed );
	}

	public override float GetGrowthPercent()
	{
		return( ( float )blades.Count / ( float )maxBlades );
	}

	[SerializeField] GameObject bladePrefab = null;
	[SerializeField] GameObject seedPrefab = null;

	List<Transform> blades = new List<Transform>();

	[SerializeField] Growable bladeGrowth = new Growable( 0.03f,0.4f,1.0f );
	[SerializeField] int maxBlades = 10;
	[SerializeField] float bladeAngDev = 70.0f;

	[SerializeField] float seedSpawnChance = 0.3f;
	[SerializeField] Growable seedGrowth = new Growable( 0.03f,0.05f,1.3f );

	GameObject seed = null;
}