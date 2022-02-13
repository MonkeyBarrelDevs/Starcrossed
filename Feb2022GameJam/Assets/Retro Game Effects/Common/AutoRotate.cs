using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public float m_Speed = 30f;
	public bool m_AxisX = false;
	public bool m_AxisY = false;
	public bool m_AxisZ = false;

	void Update ()
	{
		if (m_AxisX)
			transform.Rotate(m_Speed * Time.deltaTime, 0, 0);
		if (m_AxisY)
			transform.Rotate(0, m_Speed * Time.deltaTime, 0);
		if (m_AxisZ)
			transform.Rotate(0, 0, m_Speed * Time.deltaTime);

		if (!m_AxisX && !m_AxisY && !m_AxisZ)
		{
			float angle = Time.deltaTime * m_Speed;
			transform.Rotate(angle, angle, 0f);
		}
	}
}
