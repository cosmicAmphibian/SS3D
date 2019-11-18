using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
	public class TileManager2 : NetworkBehaviour
	{
		
		//Convenient variables
		string[] layerNames;	//Names of all the layers
		int layerCount;	//How many layers there are
		
		//Dictionary of every tile
		Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
		
		void Start() {
			init();
		}
		
		public void init() {
			
			//Add to me when inventing a new layer
			layerNames = new string[] {
				"Lattice",
				"Underfloor",
				"Disposals",
				"Pipe1",
				"Pipe2",
				"Wire1",
				"Floor",
				"Wire2",
				"Pipe3",
				"Pipe4",
				"Pipe5",
				"Heat1",
				"Heat2",
				"Heat3",
				"Wall"
			};
			
			layerCount = layerNames.Length;
		}
		
		public Tile createTile(Vector2 pos) {
			return new Tile(pos, this);
		}
		
		/*
		public bool isTileAt(Vector2 pos) {
			if (tiles.ContainsKey(pos)) {
				return true;
			} else {
				return false;
			}
		}
		*/
		
		public Tile getTile(Vector2 pos) {
			return new Tile(pos, this);
		}
		
		public class Tile {
			
			class SyncDictString : SyncDictionary<string, string> {}	//See Mirror.SyncDictionary documentation
			SyncDictString layers = new SyncDictString();	//Values for each layer
			
			//Constructor
			public Tile(Vector2 position, TileManager2 tm) {
				tm.tiles.Add(position, this);
				for (int i = 0; i < tm.layerCount; i++) {
					layers[tm.layerNames[i]] = "";
				}
			}
			
		}
	}
}