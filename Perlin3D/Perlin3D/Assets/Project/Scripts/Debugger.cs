using UnityEngine;

public class Debugger {

	public static void DebugTri(Vector3 p1, Vector3 p2, Vector3 p3, Color color)
	{
		//if (!Application.isPlaying)
		//{
			Gizmos.color = color;
			Gizmos.DrawLine(p1, p2);
			Gizmos.DrawLine(p2, p3);
			Gizmos.DrawLine(p3, p1);
		//}
	}

	public static void DebugLine(Vector3 p1, Vector3 p2, Color color)
	{
		//if (!Application.isPlaying)
		//{
			Gizmos.color = color;
			Gizmos.DrawLine(p1, p2);
		//}
	}

	public static void DebugPoint(Vector3 p1, float p2, bool check, Color colorOff, Color colorOn)
	{
		//if (!Application.isPlaying)
		//{
			if (check == false)
			{
				Gizmos.color = colorOff;
			}

			if (check == true)
			{
				Gizmos.color = colorOn;
			}
			Gizmos.DrawSphere(p1, p2);
		//}
	}

	private static readonly object padlock = new object();
	private static Debugger instance = null;
	public static Debugger Instance
	{
		get
		{
			lock (padlock)
			{
				if (instance == null)
				{
					instance = new Debugger();
				}
				return instance;
			}
		}
	}
}
