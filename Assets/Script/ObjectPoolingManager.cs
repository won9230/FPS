using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
	//public static ObjectPoolingManager instance;

	//public GameObject OPprefab = null;
	private MyWeapon myWeapon;

	public Queue<GameObject> m_queue = new Queue<GameObject>();

	private void Awake()
	{
		myWeapon = GetComponent<MyWeapon>();
	}
	private void Start()
	{
		//instance = this;
		for (int i = 0; i < 30; i++)
		{
			GameObject t_object = Instantiate(myWeapon.bullet, Vector3.zero, Quaternion.identity);
			if (myWeapon.bulletPos == null)
			{
				Debug.Log("오브젝트가 없습니다");
			}
			else
			{
				t_object.transform.parent = myWeapon.bulletPos.transform;
			}
			m_queue.Enqueue(t_object);
			t_object.SetActive(false);
		}
	}
	public void InsertQueue(GameObject p_object)
	{
		m_queue.Enqueue(p_object);
		p_object.SetActive(false);
	}
	public GameObject GetQueue()
	{
		GameObject t_object = m_queue.Dequeue();
		t_object.SetActive(true);
		return t_object;
	}
}
