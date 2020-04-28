using System.Collections.Generic;
using System.Linq;

using ClockBlockers.MapData.Grid;
using ClockBlockers.Utility;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData.Marker
{
    [ExecuteInEditMode]
    public class PathfindingMarker : MonoBehaviour
    {
        // DONE: Create a 'pathfinding master' class that stores the transforms of all the 'Pathfinding Marker' objects in the scene

        // Potentially, instead of storing the Marker objects, you could just store the position, and then you could even possibly delete the objects to save on performance(?)

        // TODO: During pathfinding, find the Marker closest to the 'destination', and then recursive find the marker that's closest to you

        // If same subGrid, then there is a path.
        
        [SerializeReference]
        private List<MarkerStats> adjacentMarkers;

        public PathfindingGrid Grid
        {
            get => grid;
            set => grid = value;
        }

        // public PathfindingMarkerRow Row
        // {
            // get => row;
            // set => row = value;
        // }

        private bool tooManyAdjacencies = false;

        public float scale = 0.5f;
        
        private static readonly Color DefaultDrawColor = new Color(137, 0, 255, 255); // purple

        private Color drawColor = DefaultDrawColor;
        [SerializeField][HideInInspector]
        private PathfindingGrid grid;
        
        // [SerializeField][HideInInspector]
        // private PathfindingMarkerRow row;



        private void OnDrawGizmos()
        {
            if (Grid.alwaysDrawRays && !tooManyAdjacencies) DrawTransparentRays();
            
            if (Grid.alwaysDrawNodes) DrawCube();

            if (Grid.alwaysDrawCollisionArea) DrawCollisionArea();

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
            Grid.ResetAllMarkerGizmos();
            DrawSingleSelectedMarker();
            
            // TODO: If multiples are selected, instead check if there is an available path between the two. This requires a rework of the adjacency system.
        }

        private void DrawSingleSelectedMarker()
        {
            drawColor = Color.black;

            if (Grid.selectionDrawRays) DrawTransparentRays();

            if (!Grid.AffectNodes) return;
            
            scale = Grid.selectionNodeScale;
            
            if (adjacentMarkers == null) return;

            foreach (MarkerStats markerStat in adjacentMarkers)
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

        // ReSharper disable once SuggestBaseTypeForParameter
        private bool CheckIfAdjacent(PathfindingMarker marker)
        {
            return adjacentMarkers != null && adjacentMarkers.Any(markerStat => markerStat.marker == marker);
        }

        [ContextMenu("Generate Adjacencies")]
        public void GetAdjacentMarkersFromGrid()
        {
            adjacentMarkers = new List<MarkerStats>();

            Transform cachedTransform = transform;

            Vector3 position = cachedTransform.position;

            foreach (PathfindingMarker checkedMarker in grid.markers)
            {
                if (checkedMarker == this) continue;
                    
                Vector3 checkedMarkerPos = checkedMarker.transform.position;
                Vector3 vectorToChild = checkedMarkerPos - position;
                float distanceToChild = Vector3.Distance(position, checkedMarkerPos);
            
                if (Physics.Raycast(position, vectorToChild, distanceToChild)) continue;
            
                var markerStat = new MarkerStats(checkedMarker, vectorToChild.magnitude);
                adjacentMarkers.Add(markerStat);
            }
            PickAColor();
        }

        private void DrawCube()
        {
            Gizmos.color = drawColor;

            DrawCubeGizmoOnPosition();
        }

        private void DrawTransparentRays()
        {
            if (adjacentMarkers == null) return;
            const float rayTransparency = 0.6f;
            var transparentDrawColor = new Color(drawColor.a, drawColor.g, drawColor.b, drawColor.a * rayTransparency);

            Gizmos.color = transparentDrawColor;
            DrawRayGizmosToAdjacentMarkersFromList();
        }

        public void PickAColor()
        {
            drawColor = DefaultDrawColor;

            if (adjacentMarkers == null) return;
            
            int adjacentNodesAmount = adjacentMarkers.Count;
            
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
            Vector3 position = transform.position;
            position.y += Grid.heightAboveFloor + scale/2;
            
            Gizmos.DrawCube(position, Vector3.one * scale);
        }


        private void DrawRayGizmosToAdjacentMarkersFromList()
        {
            // if (adjacentMarkers == null || adjacentMarkers.Count == 0) return;
            
            foreach (MarkerStats markerStat in adjacentMarkers)
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
        
        private void DrawRayGizmosToAdjacentMarkersFromCollisionCheck()
        {
            Transform cachedTransform = transform;

            Transform parent = cachedTransform.parent;

            // If too many, then the editor slows to a crawl. Caution is advised with this function
            if (parent.childCount > 10) return;
            
            Vector3 position = cachedTransform.position;

            foreach (Transform child in parent)
            {
                Vector3 childPos = child.position;
                Vector3 vectorToChild = childPos - position;
                float distanceToChild = Vector3.Distance(position, childPos);

                if (Physics.Raycast(position, vectorToChild, distanceToChild)) continue;

                Gizmos.DrawRay(position, vectorToChild);
            }
        }
        
        private void GetAdjacentMarkersFromParent()
        {
            adjacentMarkers = new List<MarkerStats>();


            Transform cachedTransform = transform;

            Vector3 position = cachedTransform.position;
            
            
            Transform parent = cachedTransform.parent;
            foreach (Transform child in parent)
            {
                var marker = child.GetComponent<PathfindingMarker>();
                if (marker == null) return;
                
                Vector3 markerPos = child.position;
                Vector3 vectorToChild = markerPos - position;
                float distanceToChild = Vector3.Distance(position, markerPos);
            
                if (Physics.Raycast(position, vectorToChild, distanceToChild)) continue;
            
                var markerStat = new MarkerStats(marker, vectorToChild.magnitude);
                adjacentMarkers.Add(markerStat);
            }
        }

        private void DrawRayGizmoToLastSibling()
        {
            Transform parent = transform.parent;

            int siblingIndex = transform.GetSiblingIndex();

            Vector3 lastSiblingPos = default;

            if (siblingIndex < 1) return;

            lastSiblingPos = parent.GetChild(siblingIndex - 1).position;

            Vector3 position = transform.position;

            Gizmos.DrawRay(position, lastSiblingPos - position);
        }


        public static PathfindingMarker CreateInstance(string markerName, ref Vector3 markerPos, ref PathfindingGrid newGrid, ref Transform rowTransform)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();
            Transform newMarkerTransform = newMarker.transform;
			
            newMarkerTransform.SetParent(rowTransform);
            newMarkerTransform.position = markerPos;
            
            
            newMarker.Grid = newGrid;
            // newMarker.Row = newRow;
            
            return newMarker;
        }

        public void ResetGizmo()
        {
            scale = grid.nodeScale;
            PickAColor();
        }
    }
}
