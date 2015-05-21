using UnityEngine;
using System.Collections.Generic;

namespace ProD
{
	public class SpawnHealthMovement : Movement
	{
		public override void Setup(WorldMap newWorld)
		{
			Setup(newWorld, new Address(0, 0), null);
		}

		public override void Setup(WorldMap newWorld, Address mapAdress, Address spawnPoint)
		{


			if (newWorld.maps == null || newWorld.size_X <= 0 || newWorld.size_Y <= 0)
				return;

			currentWorld = newWorld;

			//Resize player to cell size
			//if (ProDManager.Instance.topREPLACEDown)
			//{
			//    transform.localScale = new Vector3(ProDManager.Instance.tileSpacingX, 1, ProDManager.Instance.tileSpacingY); 
			//}
			//else
			//{
			//    transform.localScale = new Vector3(ProDManager.Instance.tileSpacingX, ProDManager.Instance.tileSpacingY, 1);  
			//}

			if (currentWorld == null || mapAdress.x < 0 || mapAdress.x >= currentWorld.size_X ||
				mapAdress.y < 0 || mapAdress.y >= currentWorld.size_Y)
				return;


			currentMap = currentWorld.maps[mapAdress.x, mapAdress.y];
			if (spawnPoint == null)
			{
				List<Cell> placementList = new List<Cell>();
				placementList.AddRange(MethodLibrary.GetListOfCellType("Corridor", currentMap));
				MoveToCell(placementList[Random.Range(0, placementList.Count - 1)]);
			}
			else
				MoveToCell(currentMap.GetCell(spawnPoint.x, spawnPoint.y));
		}
	}
}
