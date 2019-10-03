using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
	public class MetalWall : ITilePart
	{
		//Would be editor variables, if the editor supported interfaces better :(
		public Mesh[] listOfMeshes;	//List of meshes to select from in getMesh()
		public Vector3 defaultPosOffset = new Vector3(0, 0, 0);	//Default positional offset for meshes
		public Vector3 defaultRotOffset = new Vector3(0, 0, 0);	//Default rotational offset for meshes
		
		//Non-editor variables
		Mesh[] meshesToMerge = new Mesh[] {};	//Meshes to merge together for returning (used in getMesh())
		
		public override void init(TileManager tileManager) {
			tm = tileManager;
		}
		
		//Shorthand function used in getMesh()
		void m(int index) {
			//Append to meshesToMerge
			meshesToMerge[meshesToMerge.Length] = listOfMeshes[index];	//-------------HERE IS THE CURRENT ERROR.
			
			/*	
				-Accessing listOfMeshes causes an error because it is uninitialized
				-I would have loved to initialize it in the editor, but...
				-MetalWall, being a tile part, inherits from an interface so that it can have a custom
				 getMesh() function
				-Interfaces are not serializable, therefore do not show up in editor
				-It is also impossible to create a monoBehaviour interface
				
				WHAT NEEDS TO BE DONE
				
				-There needs to be an intuitive way to edit tile parts.
				-Each tile part needs to be able to have a custom getMesh() function.
			*/
			
		}
		
		public override Mesh getMesh(Tile t) {
			Mesh result;
			int yRot = 0;	//Rotation about vertical axis, a useful shorthand
			Vector3[] posOffsets = new Vector3[] {};	//Positional offsets for each mesh
			Vector3[] rotOffsets = new Vector3[] {};	//Rotational offsets for each mesh
			
			string adjacentLayerVals = t.getAdjacentLayerVals("Wall");
			
			switch (adjacentLayerVals) {
				case "0000":
					m(0);
					break;
				case "0001":
				case "0100":
				case "0101":
					m(1);
					yRot = 90;
					break;
				case "0010":
				case "1000":
				case "1010":
					m(1);
					break;
				case "0011":
					m(2);
					break;
				case "0110":
					m(2);
					yRot = 90;
					break;
				case "1100":
					m(2);
					yRot = 180;
					break;
				case "1001":
					m(2);
					yRot = 270;
					break;
				case "0111":
					m(3);
					break;
				case "1110":
					m(3);
					yRot = 90;
					break;
				case "1101":
					m(3);
					yRot = 180;
					break;
				case "1011":
					m(3);
					yRot = 270;
					break;
				case "1111":
					m(0);
					break;
			}
			
			Vector3[] finalPosOffsets = new Vector3[] {};
			Quaternion[] finalRotOffsets = new Quaternion[] {};
			
			for (int i = 0; i < meshesToMerge.Length; i++) {
				finalPosOffsets[i] = defaultPosOffset + posOffsets[i];
				finalRotOffsets[i] = Quaternion.Euler(defaultRotOffset.x, defaultRotOffset.y + yRot, defaultRotOffset.z);
			}
			
			
			//Merge meshes together
			result = tm.mergeMeshes(meshesToMerge, finalPosOffsets, finalRotOffsets);
			return result;
		}
	}
}