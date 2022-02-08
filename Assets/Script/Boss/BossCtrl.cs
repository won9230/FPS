using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCtrl : LivingEntity
{
	public enum eState
	{
		Patrol,
		TraceReady,
		Trace,
		Attack,
		Skill,
		DIE,
	}
	private IStateMachine<BossCtrl> m_sm;
	private Dictionary<eState, IState<BossCtrl>> m_states = new Dictionary<eState, IState<BossCtrl>>();
	public float traceDist = 15.0f;
	public float attackDist = 7.0f;
	protected override void Start()
	{
		m_sm = new IStateMachine<BossCtrl>(this, m_states[eState.Patrol]);
	}
	public void ChangeState(eState state)
	{
		//Debug.LogWarning(state);
		m_sm.SetState(m_states[state]);
	}
	public bool CheakState(eState state)
	{
		return m_sm.CurState == m_states[state];
	}
	void Update()
	{
		m_sm.OnUpdate();
		if (hp <= 0)
			ChangeState(eState.DIE);
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, attackDist);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.transform.position, traceDist);
	}
}
