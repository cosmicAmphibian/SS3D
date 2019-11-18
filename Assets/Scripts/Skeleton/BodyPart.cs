using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Examples.Basic
{
	[System.Serializable]
	public class BodyPart
	{
		public string bodyPartName;
		
		//Arrays, so they can be serialized
		public GameObject[] attachedBones; //Any bones that need to have their rigidbodies + colliders replicated for severing
		public Mesh[] attachedMeshes; //Any meshes that need to be replicated and disabled for severing
		public Material mat;
		
		BodyPartBase bpb;
		
		float maxDamage = 100;	//Max damage of any type
		
		//Whether or not to use certain damage types, for editing convenience
		public bool useBrute = true;
		public bool useBurn = true;
		public bool useToxin = false;
		public bool useSuffocation = false;
		
		//Putting the above in an array for coding convenience
		bool[] useDamage = new bool[4];
		
		float[] damage = new float[4] {0, 0, 0, 0};
		
		public void Initialize(BodyPartBase bpb) {
			Debug.Log("BODYPART INITIALIZED");
			useDamage = new bool[4] {useBrute, useBurn, useToxin, useSuffocation};
			this.bpb = bpb;
			
			severMe();
		}
		
		public void damageMe(int damageType, float amount) {
			//Damage this BodyPart
			float damageToApply = 0;
			if (useDamage[damageType]) {
				damageToApply = Mathf.Min(
					damage[damageType] + amount,
					maxDamage
				);
			}
			damage[damageType] = damageToApply;
		}
		
		//To do: Bake some sort of prefab in memory for severing, instead of calculating upon sever
		//You cannot use Mirror attributes on a non-mirror object. 
		//For this reason, a helper on a mirror NetworkBehaviour is called.
		public void severMe() {
			Debug.Log("SEVER ME");
			bpb.severHelper(this);
		}
	}
}