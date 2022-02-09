using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCtrl : LivingEntity
{
	public enum eState
	{
		Patrol,
		TraceReady,
		Trace,
		Attack,
		Skill,
		Die,
	}
	private IStateMachine<BossCtrl> m_sm;
	private Dictionary<eState, IState<BossCtrl>> m_states = new Dictionary<eState, IState<BossCtrl>>();
	private Transform playerTra;
	private Transform bossTra;
	private NavMeshAgent agent;
	[HideInInspector] public Animator anim;
	public float damage;
	public float traceDist = 15.0f;
	public float attackDist = 7.0f;
	private Vector3 _traceTaget;
	protected override void Start()
	{
		m_states.Add(eState.Patrol, new IStateBossPatrol());
		m_states.Add(eState.Trace, new IStateBossTrace());
		m_states.Add(eState.Attack, new IStateBossAttack());
		m_states.Add(eState.Die, new IStateBossDie());
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		playerTra = PlayerCtrl.instance.transform;
		bossTra = this.transform;

		m_sm = new IStateMachine<BossCtrl>(this, m_states[eState.Patrol]);
	}
	public void ChangeState(eState state) //스테이트 바꾸기
	{
		//Debug.LogWarning(state);
		m_sm.SetState(m_states[state]);
	}
	public bool CheakState(eState state) //스테이트 체크
	{
		return m_sm.CurState == m_states[state];
	}
	void Update()
	{
		m_sm.OnUpdate();
		if (hp <= 0)
			ChangeState(eState.Die);
	}
	public float dist() //플레이어와 거리 계산
	{
		return (playerTra.position - bossTra.position).sqrMagnitude;
	}
	public void ChackTrace() //가까이 오면 추척
	{
		if (dist() <= traceDist * traceDist)
			ChangeState(eState.Trace);
	}
	public void MoveCheak() //움직이는 거
	{
		if (dist() <= traceDist * traceDist)
		{
			agent.isStopped = false;
			agent.SetDestination(playerTra.position);
			anim.SetBool("Trace", true);

		}
		if (dist() >= traceDist * traceDist)
		{
			agent.isStopped = false;
			agent.SetDestination(playerTra.position);
			anim.SetBool("Trace", true);
		}
		if (dist() <= attackDist * attackDist)
		{
			ChangeState(eState.Attack);
		}
	}
	public IEnumerator BossAttackCorutine()
	{
		agent.isStopped = true;
		agent.velocity = Vector3.zero;
		anim.SetTrigger("Attack");
		yield return new WaitForSeconds(1.6f);
		ChangeState(eState.Trace);
	}
	public IEnumerator BossDie()
	{
		yield return new WaitForSeconds(0.2f);
		anim.SetBool("Dead", true);
		agent.isStopped = true;
		yield return new WaitForSeconds(1.0f);
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, attackDist);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, traceDist);
	}
}
