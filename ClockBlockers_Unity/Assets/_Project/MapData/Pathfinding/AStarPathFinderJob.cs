// using Unity.Collections;
// using Unity.Jobs;
//
//
// namespace ClockBlockers.MapData.Pathfinding
// {
// 	internal struct AStarPathFinderJob : IJobParallelFor
// 	{
// 		public NativeArray<AStarPathFinder> pathFindersArray;
//
// 		public void Execute(int index)
// 		{
// 			AStarPathFinder pathFinder = pathFindersArray[index];
// 			pathFinder.FindPath();
// 			pathFindersArray[index] = pathFinder;
// 		}
// 	}
// }