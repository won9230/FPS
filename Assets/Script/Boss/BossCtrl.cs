using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCtrl : LivingEntity
{
	public enum eState
	{
		Patrol,     //정찰
		TraceReady, //추격준비
		Trace,      //추격
		Attack,     //공격
		Skill,      //스킬
		Die,        //사망
	}
	private IStateMachine<BossCtrl> m_sm;
	private Dictionary<eState, IState<BossCtrl>> m_states = new Dictionary<eState, IState<BossCtrl>>();
	private Transform playerTra; //플레이어 위치
	private Transform bossTra; //보스 위치
	private NavMeshAgent agent;
	[HideInInspector] public Animator anim;
	public float damage;
	public float traceDist = 15.0f;
	public float attackDist = 7.0f;
	public float bossSpeed = 7.0f;
	protected override void Start()
	{
		m_states.Add(eState.Patrol, new IStateBossPatrol());
		m_states.Add(eState.Trace, new IStateBossTrace());
		m_states.Add(eState.Attack, new IStateBossAttack());
		m_states.Add(eState.Die, new IStateBossDie());
		m_states.Add(eState.Skill, new IStateBossSkill());
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		bossTra = this.transform;
		//playerTra = GameObject.Find("Player").transform;
		StartCoroutine(FindPlayer());
	
		m_sm = new IStateMachine<BossCtrl>(this, m_states[eState.Patrol]);//시작 스테이트
	}
	IEnumerator FindPlayer()
	{
		while (true)
		{
			GameObject player = GameObject.Find("Player(Clone)");
			yield return new WaitForSeconds(0.3f);
			if (player != null)
			{
				playerTra = player.transform;
				StopCoroutine(FindPlayer());
			}
		}
	}
	public void ChangeState(eState state) //스테이트 바꾸기
	{
		Debug.LogWarning(state);
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
	public void ChackTrace() //플레이어가 가까이 오면 추척
	{
		if (playerTra != null)
		{
			if (dist() <= traceDist * traceDist)
				ChangeState(eState.Trace);
		}
	}
	public void MoveCheak() //보스 이동 및 공격
	{
		if (playerTra != null)
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
	}

	public IEnumerator BossAttackCorutine()//보스 공격 코루틴
	{
		agent.isStopped = true;
		agent.velocity = Vector3.zero;
		anim.SetTrigger("Attack");
		yield return new WaitForSeconds(1.6f);
		ChangeState(eState.Trace);
	}
	public IEnumerator BossDie() //보스 죽음 코루틴
	{
		yield return new WaitForSeconds(0.2f);
		hp = 0.1f;
		anim.SetBool("Dead", true);
		agent.isStopped = true;
		yield return new WaitForSeconds(1.0f);
	}
	public IEnumerator BossSkillTimeCorutine()//보스 스킬 사용 여부
	{
		int i = Random.Range(0, 100);
		Debug.Log(i);
		yield return new WaitForSeconds(3f);
		if (i > 0)
		{
			ChangeState(eState.Skill);
		}
	}
	public IEnumerator BossSkillCorutine() //보스 스킬 사용
	{
		agent.speed = bossSpeed * 5f;
		yield return new WaitForSeconds(1f);
		anim.SetTrigger("Skill");
		yield return new WaitForSeconds(3f);
		agent.isStopped = true;
		yield return new WaitForSeconds(1f);
		agent.isStopped = false;
		agent.speed = bossSpeed;
		ChangeState(eState.Trace);
	}
	private void OnDrawGizmosSelected() //공격과 추격 할 때
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, attackDist);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, traceDist);
	}
}
