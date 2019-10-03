using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
	public class TileManager : NetworkBehaviour
	{
		//Editor Variables
		public GameObject tileGOPrefab;
		
		//Non-editor variables
		GameObject emptyTileGO;
		Tile emptyTile;	//A tile with all layers empty, used as a function return
		
		[HideInInspector]
		public string[] layerNames;	//Names of all the layers, used a lot for iteration
		
		[HideInInspector]
		public int layerCount;	//Number of layers
		
		public Dictionary<string, ITilePart> tileParts;	//Renderers for each possible layer value (metal wall, green pipe etc)
		
		[HideInInspector]
		public Vector3[] defaultPosOffsets;	//Default position offsets for each layer. Derived from defaultYPosOffsets in Start().
		
		public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();	//Dictionary of every tile
		
		// Start is called before the first frame update
		void Start()
		{
			//Initialization
			
			//-------------------Add to me when inventing a new layer
			layerNames = new string[] {
				"Wall"
			};
			
			layerCount = layerNames.Length;
			
			Dictionary<string, float> defaultYPosOffsets;	//Default Y position offsets for each layer
			
			//-------------------Add to me when inventing a new layer
			defaultYPosOffsets = new Dictionary<string, float>() {
				{"Wall", 0f}
			};
			
			defaultPosOffsets = new Vector3[layerCount];
			for (int i = 0; i < layerCount; i++) {
				defaultPosOffsets[i] = new Vector3(0f, defaultYPosOffsets[layerNames[i]], 0f);
			}
			
			//-------------------Add to me when inventing a new tile part
			tileParts = new Dictionary<string, ITilePart> {
				{"MetalWall", MetalWall.CreateInstance<MetalWall>()}
			};
			
			emptyTileGO = Instantiate(tileGOPrefab);
			emptyTile = emptyTileGO.GetComponent<Tile>();
			emptyTile.init(this, new Vector2(0, 0), false);
			
			//For testing
			if (isServer) {
				//Create a 20 by 20 square tiles. every second one has a metal wall on it
				for (int xi = -10; xi <= 10; xi++) {
					for (int yi = -10; yi <= 10; yi++) {
						Vector2 pos = new Vector2(xi, yi);
						Tile t = createTile(pos);
						if (yi % 2 == 0) {
							t.setLayerVal("Wall", "MetalWall");
						}
					}
				}
			}
		}
		
		public Vector2 closestTo(Vector3 vec) {
			//Get the closest tile position to a 3d coordinate
			return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.z));
		}
		
		public Tile getTile(Vector2 pos) {
			//Get the tile at pos
			if (!tiles.ContainsKey(pos)) {
				return emptyTile;
			} else {
				return tiles[pos];
			}
		}
		
		public Tile createTile(Vector2 pos) {
			//Create a new tile at pos
			GameObject tileGO = Instantiate(tileGOPrefab, new Vector3(pos.x, 0, pos.y), new Quaternion(0, 0, 0, 0));
			Tile t = tileGO.GetComponent<Tile>();
			t.init(this, pos);
			return t;
		}
		
		public Mesh mergeMeshes(Mesh[] meshes, Vector3[] posOffsets, Quaternion[] rotOffsets) {
			//Take in some meshes, with positional and rotational offsets, and merge them to a single mesh
			
			Mesh result = new Mesh();
			
			CombineInstance[] combineInstances = new CombineInstance[meshes.Length];
			
			for (int i = 0; i < meshes.Length; i++) {
				combineInstances[i].mesh = meshes[i];
				combineInstances[i].transform = Matrix4x4.TRS(posOffsets[i], rotOffsets[i], new Vector3(1, 1, 1));
			}
			
			result.CombineMeshes(combineInstances, false);
			return result;
		}
	}
}