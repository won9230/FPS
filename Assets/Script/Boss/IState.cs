public interface IState<T>
{
	void OnEnter(T qstate);
	void OnExit(T qstate);
	void OnUpdate(T qstate);
}

