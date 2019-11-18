using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
	public static class MeshMerger
	{
		public static Mesh mergeMeshes(Mesh[] meshes, Vector3[] posOffsets, Quaternion[] rotOffsets) {
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
		
		/*
		public static Mesh mergeMeshes(Mesh[] meshes) {
			//Take in some meshes and merge them to a single mesh
			
			Mesh result = new Mesh();
			
			CombineInstance[] combineInstances = new CombineInstance[meshes.Length - 1];
			
			
			for (int i = 1; i < meshes.Length; i++) {
				combineInstances[i] = new CombineInstance();
				combineInstances[i].mesh = meshes[i];
			}
			
			meshes[0].CombineMeshes(combineInstances, false);
			return meshes[0];
		}*/
	}
}