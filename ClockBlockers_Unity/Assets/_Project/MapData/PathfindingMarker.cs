using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData
{
    [ExecuteInEditMode]
    public class PathfindingMarker : MonoBehaviour
    {

        // TODO: During pathfinding, find the Marker closest to the 'destination', and then recursive find the marker that's closest to you

        [SerializeReference]
        public List<PathfindingMarker> connectedMarkers;

        public int ConnMarkerCount
        {
            get
            {
                if (connectedMarkers == null) return -1;

                return connectedMarkers.Count(marker => marker != null);
            }
        }

        public float scale = 0.5f;

        // private static readonly Color DefaultDrawColor = new Color(137, 0, 255, 255); // purple
        private static readonly Color DefaultDrawColor = Color.white;

        private Color drawColor = DefaultDrawColor;

        // TODO: Remove this
        public Color DrawColor
        {
            set => drawColor = value;
            get => drawColor;
        }

        [SerializeField]
        [HideInInspector]
        private PathfindingGrid grid;

        // _Exclusively_ for equalizing the 'Draw Height Above Floor' gizmo drawing.
        public float creationHeightAboveFloor;

        private void OnDrawGizmos()
        {
            if (grid.alwaysDrawRays) DrawTransparentRays();

            if (grid.alwaysDrawNodes) DrawCubeGizmoOnPosition();

            if (grid.alwaysDrawCollisionArea) DrawCollisionArea();
        }

        private void DrawCollisionArea()
        {
            Vector3 markerPos = transform.position;
            var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

            Gizmos.DrawWireCube(newPos, grid.minimumOpenAreaAroundMarkers);
        }

        private void OnDrawGizmosSelected()
        {
            if (Selection.activeGameObject != gameObject) return;
            grid.ResetMarkerGizmos();
            DrawSingleSelectedMarker();

            // TODO: If multiples are selected, instead check if there is an available path between the two. This requires a rework of the adjacency system.
        }

        private void DrawSingleSelectedMarker()
        {
            drawColor = Color.black;

            if (grid.selectionDrawRays) DrawTransparentRays();

            if (!grid.AffectNodes) return;

            scale = grid.selectionNodeScale;

            if (connectedMarkers == null) return;

            foreach (PathfindingMarker marker in connectedMarkers)
            {
                // PathfindingMarker marker = markerStat.marker;

                if (marker == null) continue;

                if (grid.selectionChangeNodeColors) marker.drawColor = Color.magenta;

                marker.scale = grid.selectionChangeScale ? grid.selectionNodeScale : grid.nodeScale;
            }
        }

        private bool CheckIfConnected(PathfindingMarker marker)
        {
            return connectedMarkers != null && connectedMarkers.Any(connMarker => connMarker == marker);
        }

        private void DrawTransparentRays()
        {
            if (connectedMarkers == null) return;
            const float rayTransparency = 0.6f;
            var transparentDrawColor = new Color(drawColor.a, drawColor.g, drawColor.b, drawColor.a * rayTransparency);

            Gizmos.color = transparentDrawColor;
            DrawRayGizmosToConnectedMarkers();
        }

        private void PickAColor()
        {
            const int fewAdjacentNodesAmount = 3;
            const int someAdjacentNodesAmount = 6;

            drawColor = DefaultDrawColor;

            int connMarkersAmount = ConnMarkerCount;

            if (ConnMarkerCount == -1) return;

            if (connMarkersAmount == 0)
            {
                drawColor = new Color(137, 0, 255, 255); // purple
            }
            else if (connMarkersAmount > 0 && connMarkersAmount < fewAdjacentNodesAmount)
            {
                drawColor = Color.green;
            }
            else if (connMarkersAmount >= fewAdjacentNodesAmount && connMarkersAmount < someAdjacentNodesAmount)
            {
                drawColor = Color.blue;
            }
            else
            {
                drawColor = Color.yellow;
            }
        }

        private void DrawCubeGizmoOnPosition()
        {
            Gizmos.color = drawColor;

            Vector3 position = transform.position;
            position.y += (grid.drawHeightAboveFloor - creationHeightAboveFloor) + (scale/2);

            Gizmos.DrawCube(position, Vector3.one * scale);
        }


        private void DrawRayGizmosToConnectedMarkers()
        {
            // if (adjacentMarkers == null || adjacentMarkers.Count == 0) return;

            foreach (PathfindingMarker marker in connectedMarkers)
            {
                // PathfindingMarker marker = markerStat.marker;
                if (marker == null)
                {
                    Logging.Log("Marker in list does not exist for some reason..");
                    return;
                }

                Vector3 markerPos = marker.transform.position;
                Vector3 position = transform.position;

                Vector3 vectorToChild = markerPos - position;
                Gizmos.DrawRay(position, vectorToChild);
            }
        }

        public void ResetGizmo()
        {
            scale = grid.nodeScale;
            PickAColor();
        }

        public static PathfindingMarker CreateInstance(string markerName, PathfindingGrid grid, Vector3 markerPos, Transform parent, float creationYPosAboveFloor)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();
            // newMarker.connectedMarkers = new List<MarkerStats>();

            newMarker.transform.position = markerPos;
            newMarker.transform.SetParent(parent);
            
            newMarker.creationHeightAboveFloor = creationYPosAboveFloor;

            newMarker.grid = grid;

            grid.markers.Add(newMarker);
            return newMarker;
        }

        public static PathfindingMarker CreateInstance(string markerName, ref PathfindingGrid grid)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();
            // newMarker.connectedMarkers = new List<MarkerStats>();

            newMarker.grid = grid;
            
            grid.markers.Add(newMarker);

            return newMarker;
        }
    }
}
