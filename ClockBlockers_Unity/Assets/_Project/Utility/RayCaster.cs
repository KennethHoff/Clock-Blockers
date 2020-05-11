using UnityEngine;


namespace ClockBlockers.Utility
{
	public static class RayCaster
	{
		// 8 & 11
		// private const int terrainLayer = 2304;


		// ~ is the "BITWISE NOT". Basically, it flips every bit in the int (int32)

		// Therefore, ~0 = 2^32-1

		private static void RemoveMarkerLayer(ref int layerToChange)
		{
			const int markerLayerInt = 13;
			layerToChange ^= (-1 ^ layerToChange) & (1 << markerLayerInt);
		}

		public static RaycastHit[] CastRayAll(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.RaycastAll(ray, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}

		public static RaycastHit[] CastRayAllTriggers(Ray ray, float distance, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.RaycastAll(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		public static RaycastHit[] CastRayAllTriggersIncludeMarkers(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.RaycastAll(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		public static bool CastRay(Ray ray, float distance, out RaycastHit hit, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, out hit, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}

		public static bool CastRay(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}

		public static bool CastRayTriggers(Ray ray, float distance, out RaycastHit hit, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, out hit, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		public static bool CastRayTriggers(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}


		public static bool CastRayTriggersIncludeMarkers(Ray ray, float distance, out RaycastHit hit, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.Raycast(ray, out hit, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		public static bool CastRayTriggersIncludeMarkers(Ray ray, float distance, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.Raycast(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}


		public static bool BoxCast(Vector3 position, Vector3 halfExtents, Vector3 direction, float distance, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.BoxCast(position, halfExtents, direction, Quaternion.identity, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}
	}
}