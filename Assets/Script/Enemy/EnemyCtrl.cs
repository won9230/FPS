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
	//[HideInInspector] public Transform enemyTr;
	public float speed = 4f;
	public float attackDist = 1.0f;
	public float damage;
	private NavMeshAgent agent;
	//private bool attackbool = true;
	private Animator anim;
	private BoxCollider box;

	protected override void Start()
	{
		//enemyTr = GetComponent<Transform>();
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		box = GetComponent<BoxCollider>();
		hp = maxHp;
		StartCoroutine(CheckState());
		StartCoroutine(Action());
		StartCoroutine(FindPlayer());
	}
	private void Update()
	{
		Quaternion rot;
		if(agent.desiredVelocity != Vector3.zero)
		{
			rot = Quaternion.LookRotation(agent.desiredVelocity);
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 7f); 
		}
	}
	IEnumerator FindPlayer()
	{
		while (true)
		{
			GameObject player = GameObject.Find("Player(Clone)");
			yield return new WaitForSeconds(0.3f);
			if (player != null)
			{
				playerTr = player.transform;
				StopCoroutine(FindPlayer());
			}
		}
	}
	IEnumerator CheckState()
	{
		while (true)
		{
			if(hp <= 0)
			{
				state = eState.Die;
				agent.isStopped = true;
				anim.SetBool("dead", true);
				agent.enabled = false;
				box.isTrigger = true;
				Destroy(gameObject,3f);
				StopAllCoroutines();
			}
			if(state != eState.Ready || state != eState.Die )
			{
				float dist;
				if (dead) yield break;
				if (playerTr != null)
				{
					dist = (playerTr.position - transform.position).sqrMagnitude;
					if (dist <= attackDist * attackDist)
						state = eState.Attack;
					else
						state = eState.Trace;
				}
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
					anim.SetBool("Move", false);
					if (playerTr != null)
						state = eState.Trace;
					break;
				case eState.Trace:
					anim.SetBool("Move", true);
					anim.SetFloat("MoveSpeed", Random.Range(0.9f, 1.2f));
					TraceTarget(playerTr.position);
					break;
				case eState.Attack:
					MoveStop();
					anim.SetTrigger("Attack");
					yield return new WaitForSeconds(0.3f);
					break;
			}
		}
	}
	private void TraceTarget(Vector3 pos)
	{
		if (agent.isPathStale) return;
		agent.speed = speed;
		agent.SetDestination(pos);
		agent.isStopped = false;
	}
	private void MoveStop()
	{
		agent.isStopped = true;
		agent.velocity = Vector3.zero;
	}
	private void OnDrawGizmosSelected()//범위 기즈모
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, attackDist);
	} 
}
