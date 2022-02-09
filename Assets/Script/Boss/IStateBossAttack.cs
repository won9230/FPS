using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStateBossAttack : IState<BossCtrl>
{
	public void OnEnter(BossCtrl qstate)
	{
		qstate.StartCoroutine(qstate.BossAttackCorutine());
	}

	public void OnExit(BossCtrl qstate)
	{

	}

	public void OnUpdate(BossCtrl qstate)
	{

	}
}
