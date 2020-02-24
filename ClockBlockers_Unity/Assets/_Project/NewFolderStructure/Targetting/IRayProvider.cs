using UnityEngine;


namespace ClockBlockers.Targetting
{
	public interface IRayProvider
	{
		Ray CreateRay(Camera cam, Vector2 mousePos);
	}
}