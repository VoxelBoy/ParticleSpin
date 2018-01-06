using UnityEngine;

public class Rotate : MonoBehaviour
{
	[SerializeField]
	private Vector3 axis;

	[SerializeField]
	private float speed;

	void Update()
	{
		transform.Rotate(axis, speed * Time.deltaTime);
	}
}
