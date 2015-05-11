/* 
* This code has been designed and developed by Gray Lake Studios.
* You may only use this code if you’ve acquired the appropriate license.
* To acquire such licenses you may visit www.graylakestudios.com and/or Unity Asset Store
* For all inquiries you may contact contact@graylakestudios.com
* Copyright © 2012 Gray Lake Studios
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProD
{
	public static class Generator_Dungeon
	{
		#region PROD DEFINED DEFAULT CONTENT
		private static Map map;
		private static void PrepareMap()
		{
			map = new Map(map_Size_X, map_Size_Y);
			map.theme = theme;
		}
		public static void SetGenericProperties(int map_Size_X_, int map_Size_Y_, string theme_)
		{
			map_Size_X = map_Size_X_;
			map_Size_Y = map_Size_Y_;
			theme = theme_;
		}
		#endregion PROD DEFINED DEFAULT CONTENT

		#region USER DEFINED DEFAULT CONTENT
		//You should set all these variables to your liking.
		private static int map_Size_X = 29;
		private static int map_Size_Y = 29;

		//private static string theme = "Stone Dungeon Theme";
		private static string theme = "Terminal Theme";
		public static void SetTheme(string t)
		{
			theme = t;
		}


		//These variables are here for runtime alteration, mainly for debug testing.
		private static string type_Abyss = "Abyss";
		private static string type_Room = "Room";
		private static string type_StartRoom = "StartRoom";
		private static string type_EndRoom = "EndRoom";
		private static string type_Corridor = "Path";
		private static string type_Wall = "Wall";
		private static int room_Min_X = 3;
		private static int room_Max_X = 11;
		private static int room_Min_Y = 3;
		private static int room_Max_Y = 11;
		private static int room_Freq = 10;
		private static int room_Retry = 6;
		private static int doorsPerRoom = 1;
		private static int repeat = 12;
		private static bool frameMap = false;


		//This method is only here for runtime alteration, mainly for debug testing.
		public static void SetSpecificProperties(string type_Abyss_, string type_Room_, string type_StartRoom_, string type_EndRoom_, string type_Corridor_, string type_Wall_,
			int room_Min_X_, int room_Max_X_, int room_Min_Y_, int room_Max_Y_, int room_Freq_, int room_Retry_,
			int doorsPerRoom_, int repeat_, bool frameMap_)
		{
			type_Abyss = type_Abyss_;
			type_Room = type_Room_;
			type_StartRoom = type_StartRoom_;
			type_EndRoom = type_EndRoom_;
			type_Corridor = type_Corridor_;
			type_Wall = type_Wall_;
			room_Min_X = room_Min_X_;
			room_Max_X = room_Max_X_;
			room_Min_Y = room_Min_Y_;
			room_Max_Y = room_Max_Y_;
			room_Freq = room_Freq_;
			room_Retry = room_Retry_;
			doorsPerRoom = doorsPerRoom_;
			repeat = repeat_;
			frameMap = frameMap_;
		}

		//Generate() has the actual methods that generate the map data. 
		//You may customize this method with MethodLibrary methods.
		public static Map Generate()
		{
			List<string> walkableTypes = new List<string>();
			walkableTypes.Add(type_Room);
			walkableTypes.Add("Door");

			PrepareMap();

			MethodLibrary.CreateRooms(map, type_Wall, type_Room, room_Min_X, room_Max_X, room_Min_Y, room_Max_Y,
				room_Freq, room_Retry, doorsPerRoom);

			MethodLibrary.CreateMaze(map, type_Corridor, type_Abyss);

			MethodLibrary.PlaceStairs(map, 1, type_StartRoom, "Entrance");
			MethodLibrary.PlaceStairs(map, 1, type_EndRoom, "Exit", "Entrance", map.size_X * map_Size_Y / 20, walkableTypes);

			MethodLibrary.SetCellsOfTypeAToB(map, type_Abyss, type_Wall);

			MethodLibrary.CloseDeadEndCells(map, type_Wall, type_Corridor);
			MethodLibrary.ReduceUCorridors(map, type_Wall, type_Corridor, repeat);
			MethodLibrary.ConvertUnreachableCells(map, type_Wall, type_Abyss);

			if (frameMap)
				MethodLibrary.FrameMap(map, type_Wall, 1);


			return map;
		}
		#endregion USER DEFINED DEFAULT CONTENT
	}
}