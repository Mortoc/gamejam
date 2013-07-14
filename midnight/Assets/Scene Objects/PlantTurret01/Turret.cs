using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour 
{
	public float AttackRange = 750.0f;
	private float _attackRangeSqr;
	public float AttackRate = 1.0f;
	public int AttackDamage = 334;
	
	public GameObject ProjectilePrefab;
	public Transform ProjectileEmitter;
	
	public GameObject LookAtComponent;
	
	void Start()
	{
		_attackRangeSqr = AttackRange * AttackRange;
		
		Scheduler.Run(CheckForEnemiesToAttack());
	}
	
	private IEnumerator<IYieldInstruction> CheckForEnemiesToAttack()
	{
		while( gameObject )
		{
			float nearestSqr = Mathf.Infinity;
			Enemy nearest = null;
			foreach( Enemy enemy in Find.ObjectsInScene<Enemy>() )
			{
				float distSqr = (transform.position - enemy.transform.position).sqrMagnitude;
				if( distSqr < nearestSqr )
				{
					nearestSqr = distSqr;
					nearest = enemy;
				}
			}
						
			if( nearestSqr < _attackRangeSqr && nearest )
			{
				GameObject projectile = (GameObject)Instantiate(ProjectilePrefab);
				projectile.transform.position = ProjectileEmitter.position;
				projectile.GetComponent<FriendlyProjectile>().Target = nearest;
				projectile.GetComponent<FriendlyProjectile>().Damage = AttackDamage;
				yield return new YieldForSeconds(AttackRate);
			}
			else
			{
				yield return Yield.UntilNextFrame;
			}
		}
	}
}
