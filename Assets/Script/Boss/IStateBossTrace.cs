using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStateBossTrace : IState<BossCtrl>
{
	public void OnEnter(BossCtrl qstate)
	{
		qstate.anim.SetBool("Trace", true);
		qstate.StartCoroutine(qstate.BossSkillTimeCorutine());
	}
	public void OnExit(BossCtrl qstate)
	{

	}
	public void OnUpdate(BossCtrl qstate)
	{
		qstate.MoveCheak();
	}
}
