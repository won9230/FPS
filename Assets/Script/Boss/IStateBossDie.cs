using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStateBossDie : IState<BossCtrl>
{
	public void OnEnter(BossCtrl qstate)
	{
		qstate.StartCoroutine(qstate.BossDie());
	}

	public void OnExit(BossCtrl qstate)
	{

	}

	public void OnUpdate(BossCtrl qstate)
	{

	}
}
