using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
	public class ITilePart : ScriptableObject
	{
		//Interface for tile parts
		public TileManager tm;
		public virtual Mesh getMesh(Tile t) {
			return new Mesh();
		}
		public virtual void init(TileManager tileManager) {
			tm = tileManager;
		}
	}
}