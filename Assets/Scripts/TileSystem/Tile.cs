using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Mirror.Examples.Basic
{
	public class Tile : NetworkBehaviour
	{
		//Editor variables		
		public class SyncDictString : SyncDictionary<string, string> {}	//See Mirror.SyncDictionary documentation
		public SyncDictString layers = new SyncDictString();	//Values for each layer
		//e.g. layers["Wall"] = "MetalWall", layers["Pipe1"] = "Manifold"
		
		//Non-editor variables
		
		//Shorthands
		TileManager tm;
		MeshFilter mf;
		
		Dictionary<string, Mesh> meshesToRender = new Dictionary<string, Mesh>();	//Meshes for each layer
		
		public void init(TileManager tileManager, Vector2 pos, bool addToDictionary = true) {
			//Initialization
			tm = tileManager;
			mf = GetComponent<MeshFilter>();
			
			//Initialize arrays, dictionaries
			for (int i = 0; i < tm.layerCount; i++) {
				layers.Add(tm.layerNames[i], "");
				meshesToRender.Add(tm.layerNames[i], new Mesh());
			}
			
			//This is a Mirror thing
			layers.Callback += onLayerSync;
			
			//Each tile is a child of the tile manager
			transform.parent = tm.transform;
			
			//Useful because of TileManager.emptyTile, which is not added to TileManager.tiles
			if (addToDictionary) {
				tm.tiles.Add(pos, this);
			}
		}
		
		public void setLayerVal(string layer, string layerVal) {
			//Set the value of a layer
			if (layers.ContainsKey(layer)) {
				layers[layer] = layerVal;
				if (true) {	//--------------CHANGE TO if (isClient) WHEN DONE TESTING
					updateModels(layer);
					updateAdjacentModels(layer);
				}
			} else {
				Debug.LogWarning("There is no layer with the name '" + layer + "'!");
			}
		}
		
		public string getLayerVal(string layer) {
			//Get the value of a layer
			return layers[layer];
		}
		
		Dictionary<string, bool> whichLayersToRender() {
			//Decide which layers to render
			
			//	-For example, a floor obstructs everything below it
			//	-Be careful for adjacent tiles
			//	-Everything in this method might be moved to individual tileParts
			
			//TEMPORARY SOLUTION FOR TESTING
			return new Dictionary<string, bool> {
				{"Wall", true}
			};
		}
		
		public string getAdjacentLayerVals(string layer) {
			//Get the value of a given layer for the surrounding 4 tiles.
			//Returned as a string of 0s and 1s (Booleans, in essence) for convenience with updateModels().
			Vector2 newPos = tm.closestTo(transform.position);
			
			Vector2[] offsets = {
				new Vector2(0, 1),
				new Vector2(1, 0),
				new Vector2(0, -1),
				new Vector2(-1, 0)
			};
			
			string result = "";
			
			for (int i = 0; i <= 3; i++) {
				//Cycle through offsets and append to the result
				Tile workingTile = tm.getTile(newPos + offsets[i]);
				
				if (workingTile.layers[layer] == "") {
					result += "0";
				} else {
					result += "1";
				}
			}
			return result;
		}
		
		public void updateModels(string layer) {
			//Retrieve and render the appropriate meshes for this tile
			if (isServer) {
				print("Warning: call to Tile.updateModels(), which is a clientside function, on serverside");
			}
			
			if (layers[layer] == "") {
				meshesToRender[layer] = new Mesh();	//this bitch empty
			} else {
				string layerVal = layers[layer];
				if (tm.tileParts.ContainsKey(layerVal)) {
					meshesToRender[layer] = tm.tileParts[layerVal].getMesh(this);
				} else {
					Debug.LogWarning("Could not find tilePart for the layer value '" + layerVal + "'! Double check your spelling?");
				}
			}
		}
		
		public void updateAdjacentModels(string layer) {
			//Update models for adjacent tiles
			Vector2 pos = tm.closestTo(transform.position);
			
			Vector2[] offsets = {
				new Vector2(0, 1),
				new Vector2(1, 0),
				new Vector2(0, -1),
				new Vector2(-1, 0)
			};
			
			for (int i = 0; i <= 3; i++) {
				//Cycle through offsets and update models
				Tile workingTile = tm.getTile(pos + offsets[i]);
				workingTile.updateModels(layer);
			}
		}
		
		void onLayerSync(SyncDictString.Operation op, string index, string val) {
			if (true) {	//--------------CHANGE TO if (isClient) WHEN DONE TESTING
				updateModels(index);
				updateAdjacentModels(index);
			}
		}
	}
}