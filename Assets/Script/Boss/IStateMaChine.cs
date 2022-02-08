using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStateMachine<T>
{
	public IState<T> CurState { get; protected set; }

	private T m_sender;

	public IStateMachine(T sender, IState<T> state)
	{
		m_sender = sender;
		SetState(state);
	}

	public void SetState(IState<T> state)
	{
		if (m_sender == null)
		{
			Debug.Log("m_sender 없음");
			return;
		}
		if (CurState == state)
		{
			Debug.LogWarningFormat("Already Define State - {0}", state);
			return;
		}
		if (CurState != null)
			CurState.OnExit(m_sender);

		CurState = state;

		if (CurState != null)
			CurState.OnEnter(m_sender);
	}
	public void OnUpdate()
	{
		if (m_sender == null)
		{
			Debug.Log("m_sener 없음");
			return;
		}
		CurState.OnUpdate(m_sender);
	}
}

