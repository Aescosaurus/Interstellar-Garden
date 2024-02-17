using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry
	:
	MonoBehaviour
{
	void Start()
	{
		groundMask = LayerMask.GetMask( "Ground" );

		if( startDisabled ) this.enabled = false;
	}

	void Update()
	{
		if( planted /*&& crowdingStats.GetCrowdingScore() > 0.0f*/ )
		{
			if( germinateTimer.Update( /*crowdingStats.GetCrowdingScore()*/ ) )
			{
				var plant = Instantiate( plantPrefab,plantSpot,Quaternion.identity );
				// Quaternion.LookRotation( hitAngle )
				plant.transform.up = hitAngle;

				Destroy( gameObject );
			}
		}
		else if( despawnTimer.Update() ) Destroy( gameObject );
	}

	void OnCollisionEnter( Collision coll )
	{
		// todo: find a way to make this only trigger once, at the end when we've stopped moving
		if( /*!planted &&*/ coll.gameObject.tag == "Ground" )
		{
			planted = true;

			// PlantManager.SetupCrowdable( gameObject,crowdingStats );

			var ray = new Ray( transform.position,Vector3.down );
			RaycastHit hit;
			if( Physics.Raycast( ray,out hit,1.0f,groundMask ) )
			{
				plantSpot = hit.point;
				hitAngle = hit.normal;
			}
			else
			{
				Vector3 contactAvg = Vector3.zero;
				for( int i = 0; i < coll.contactCount; ++i )
				{
					contactAvg += coll.GetContact( i ).point;
				}
				contactAvg /= coll.contactCount;

				plantSpot = contactAvg;
				hitAngle = Vector3.up;
			}
		}
	}

	LayerMask groundMask;

	[SerializeField] GameObject plantPrefab = null;
	[SerializeField] Growable germinateTimer = new Growable();
	[SerializeField] Growable despawnTimer = new Growable();
	[SerializeField] bool startDisabled = true;
	// [SerializeField] Crowdable crowdingStats = new Crowdable();

	bool planted = false;
	Vector3 plantSpot;
	Vector3 hitAngle;
}