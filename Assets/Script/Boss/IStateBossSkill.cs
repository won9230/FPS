using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStateBossSkill : IState<BossCtrl>
{
	public void OnEnter(BossCtrl qstate)
	{
		qstate.StopAllCoroutines();
		qstate.StartCoroutine(qstate.BossSkillCorutine());
	}

	public void OnExit(BossCtrl qstate)
	{

	}

	public void OnUpdate(BossCtrl qstate)
	{

	}
}
