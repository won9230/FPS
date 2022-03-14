using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStateBossPatrol : IState<BossCtrl>
{
	public void OnEnter(BossCtrl qstate)
	{

	}

	public void OnExit(BossCtrl qstate)
	{

	}

	public void OnUpdate(BossCtrl qstate)
	{
		qstate.ChackTrace();
	}
}
