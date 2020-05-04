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
        // DONE: Create a 'pathfinding master' class that stores the transforms of all the 'Pathfinding Marker' objects in the scene

        // Potentially, instead of storing the Marker objects, you could just store the position, and then you could even possibly delete the objects to save on performance(?)

        // TODO: During pathfinding, find the Marker closest to the 'destination', and then recursive find the marker that's closest to you


        [SerializeReference]
        public List<MarkerStats> connectedMarkers;

        public PathfindingGrid Grid
        {
            get => grid;
            set => grid = value;
        }

        private bool tooManyAdjacencies = false;

        public float scale = 0.5f;

        private static readonly Color DefaultDrawColor = new Color(137, 0, 255, 255); // purple

        private Color drawColor = DefaultDrawColor;

        [SerializeField]
        [HideInInspector]
        private PathfindingGrid grid;

        // _Exclusively_ for equalizing the 'Draw Height Above Floor' gizmo drawing.
        public float creationHeightAboveFloor;

        private void OnDrawGizmos()
        {
            if (grid.alwaysDrawRays && !tooManyAdjacencies) DrawTransparentRays();

            if (grid.alwaysDrawNodes) DrawCubeGizmoOnPosition();

            if (grid.alwaysDrawCollisionArea) DrawCollisionArea();

        }

        private void DrawCollisionArea()
        {
            Vector3 markerPos = transform.position;
            var newPos = new Vector3(markerPos.x, markerPos.y + Grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

            Gizmos.DrawWireCube(newPos, Grid.minimumOpenAreaAroundMarkers);
        }

        private void OnDrawGizmosSelected()
        {
            if (Selection.activeGameObject != gameObject) return;
            Grid.ResetMarkerGizmos();
            DrawSingleSelectedMarker();

            // TODO: If multiples are selected, instead check if there is an available path between the two. This requires a rework of the adjacency system.
        }

        private void DrawSingleSelectedMarker()
        {
            drawColor = Color.black;

            if (Grid.selectionDrawRays) DrawTransparentRays();

            if (!Grid.AffectNodes) return;

            scale = Grid.selectionNodeScale;

            if (connectedMarkers == null) return;

            foreach (MarkerStats markerStat in connectedMarkers)
            {
                PathfindingMarker marker = markerStat.marker;

                if (marker == null)
                {
                    Logging.Log("Marker is null? Dafuq");
                    return;
                }

                if (Grid.selectionChangeNodeColors) marker.drawColor = Color.magenta;

                marker.scale = Grid.selectionChangeScale ? Grid.selectionNodeScale : Grid.nodeScale;
            }
        }

        private bool CheckIfAdjacent(PathfindingMarker marker)
        {
            return connectedMarkers != null && connectedMarkers.Any(markerStat => markerStat.marker == marker);
        }

        private void DrawTransparentRays()
        {
            if (connectedMarkers == null) return;
            const float rayTransparency = 0.6f;
            var transparentDrawColor = new Color(drawColor.a, drawColor.g, drawColor.b, drawColor.a * rayTransparency);

            Gizmos.color = transparentDrawColor;
            DrawRayGizmosToAdjacentMarkersFromList();
        }

        public void PickAColor()
        {
            drawColor = DefaultDrawColor;

            if (connectedMarkers == null) return;

            int adjacentNodesAmount = connectedMarkers.Count;

            if (adjacentNodesAmount == 0)
            {
                drawColor = Color.white;
            }
            else if (adjacentNodesAmount > 0 && adjacentNodesAmount < Grid.fewAdjacentNodesAmount)
            {
                drawColor = Color.green;
            }
            else if (adjacentNodesAmount >= Grid.fewAdjacentNodesAmount && adjacentNodesAmount < Grid.someAdjacentNodesAmount)
            {
                drawColor = Color.blue;
            }
            else if (adjacentNodesAmount >= Grid.someAdjacentNodesAmount && adjacentNodesAmount < Grid.tooManyAdjacentNodesAmount)
            {
                drawColor = Color.yellow;
            }
            else if (adjacentNodesAmount >= Grid.tooManyAdjacentNodesAmount)
            {
                drawColor = Color.red;
                tooManyAdjacencies = true;
            }
        }

        private void DrawCubeGizmoOnPosition()
        {
            Gizmos.color = drawColor;

            Vector3 position = transform.position;
            position.y += (Grid.drawHeightAboveFloor - creationHeightAboveFloor) + (scale/2);

            Gizmos.DrawCube(position, Vector3.one * scale);
        }


        private void DrawRayGizmosToAdjacentMarkersFromList()
        {
            // if (adjacentMarkers == null || adjacentMarkers.Count == 0) return;

            foreach (MarkerStats markerStat in connectedMarkers)
            {
                PathfindingMarker marker = markerStat.marker;
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

        public static PathfindingMarker CreateInstance(string markerName, ref PathfindingGrid grid, ref Vector3 markerPos, ref Transform parent, ref float creationYPosAboveFloor)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();
            newMarker.connectedMarkers = new List<MarkerStats>();

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
            newMarker.connectedMarkers = new List<MarkerStats>();

            newMarker.grid = grid;
            
            grid.markers.Add(newMarker);

            return newMarker;
        }

        public void SetAdjacentMarkers(List<MarkerStats> connectedMarkers)
        {
            Logging.Log("Hey");
            this.connectedMarkers = connectedMarkers;
        }
    }
}
