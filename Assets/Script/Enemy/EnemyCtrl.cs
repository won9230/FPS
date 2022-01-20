using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : LivingEntity
{
    public enum eState
	{
		Ready,
		Trace,
		Attack,
		Die
	}
	public eState state = eState.Ready;
	[HideInInspector] public Transform playerTr;
	[HideInInspector] public Transform enemyTr;
	public float attackDist = 1.0f;
	public NavMeshAgent agent;
	private bool attackbool = true;

	protected override void Start()
	{
		playerTr = PlayerCtrl.instance.transform;
		enemyTr = GetComponent<Transform>();
		agent = GetComponent<NavMeshAgent>();
	}
	IEnumerator CheckState()
	{
		while (true)
		{
			if(hp <= 0)
			{
				state = eState.Die;
				StopAllCoroutines();
			}
			if(state != eState.Ready || state != eState.Die)
			{
				if (dead) yield break;
				float dist = (playerTr.position - enemyTr.position).sqrMagnitude;
				if (dist <= attackDist * attackDist)
					state = eState.Attack;
				else
					state = eState.Trace;
			}
			yield return new WaitForSeconds(0.3f);
		}
	}
	IEnumerator Action()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.3f);
			switch (state)
			{
				case eState.Ready:
					yield return new WaitForSeconds(1f);
					state = eState.Trace;
					break;
				case eState.Trace:
					break;
				case eState.Attack:
					yield return new WaitForSeconds(1f);
					break;
			}
		}
	}
}
