# Clock Blockers
 
##For Testers
This game was developed in the 2020.1 version of Unity. There is no reason for this other than the fact that when I started developing the game, I chose that version before realizing there was a version recommendation<br><br>

There's not a lot you can do in this game. However, there are a few things I want you to notice.<br><br>


Enter the 'Arena_1' Scene. Select the 'Pathfinding Grid' GameObject, and enable Gizmos in the Scene View. <br>

At the bottom of the (monolithic...) PathfindingGrid Script component, you see a few buttons,
 primarily 'Generate Markers', 'Clear Markers', and 'Generate Marker Adjacencies'. <br>
 
Start by pressing the 'Generate Markers' button, this will unfortunately freeze the editor, but not for a very long time. <br>

After generating the markers, you can either remove them (By pressing 'Clear Markers'), or map adjacencies onto them by pressing 'Generate Marker Adjacencies'. <br>

Markers are generated concurrently(By help of Coroutines (if in Play Mode), or EditorCoroutines(A Unity-developed Package, if in Edit Mode)),
 so Unity will not freeze. If you hold right-click (A way to force gizmos to constantly refresh - it will show even if not, but only whenever Gizmos would update),
  you can see how far the generation has come (You can also change the speed in the MarkerGenerator script - lower = slower). <br><br>

After generating the adjacencies, you can now click on any one marker and see all the nodes it's adjacent to (Regardless of height, but will not be adjacent if there is a collider in-between). <br>

Clicking on _two_ markers will show a path between the two markers (From the first to the last, based on a "default jump height" set in the PathfindingGrid - the idea here is that someone who can't jump high, shouldn't try to pathfind ontop of a high wall or something)<br>

Clicking on _three or more_ markers will generate a path that goes through all the markers you click on (Due to the way I've implemented it (Simply doing it in the order of 'Selection.' property in the UnityEditor, which I think is based on the order in the Hierarchy ) the path might seem very weird, as it doesn't create the path in the order you clicked it in) <br>

Clicking on _two or more_ markers that are far enough away from eachother not to instantly find a path, there will be a Gizmo that displays the 'OpenList' (Basically, the markers that the Pathfinding algorithm is going to look through next) (Again, holding Right-click is ideal here, due to how Gizmos are updated).<br>
A good way to show this is by reducing the 'DefaultJumpHeight' variable(In the PathfindingGrid component) to 0 , and trying to path from a low elevation to a higher elevation.<br><br>

Now, after showing that, you can build and play. <br><br>

There are four things you can do in this game. <br>
You can move and rotate your character<br>
You can shoot the gun (Only thing in the game with a sound). <br>
You can press R to 'Reset the round', which promptly creates a Clone that retraces your previous path.*<br>
You can right-click on a clone to take control over that clone, and then you can middle-click anywhere to order the clone to move where you clicked. 

\* Currently it follows the entire path, not taking time into consideration. Therefore, if the player goes to A, stands there for 10 seconds before starting to move towards B, the clone will go to A and then immediately go towards B.