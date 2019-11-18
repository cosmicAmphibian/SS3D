using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror.Examples.Basic
{
	public class Health : NetworkBehaviour
	{
		float totalBrute = 0;
		float totalBurn = 0;
		float totalToxin = 0;
		
		float suffocation = 0;	//Not applied to specific body parts
		
		float totalHealth = 100;
		
		float maxDamage = 80;	//Absolute maximum damage per body part
		
		class SyncDictFloat : SyncDictionary<string, float> {} //dictionary, but [syncvar]
		
		//Damage amounts
		SyncDictFloat chest;
		SyncDictFloat head;
		SyncDictFloat rArm;
		SyncDictFloat lArm;
		SyncDictFloat rLeg;
		SyncDictFloat lLeg;
		
		Dictionary<string, SyncDictFloat> bodyPartRefs;	//See init. For use in damageMe() etc.
		
		Dictionary<string, int> damageTypeRefs;	//See init. For use in damageMe()
		
		void init() {
			SyncDictFloat u = new SyncDictFloat {
				{"Brute", 0},
				{"Burn", 0},
				{"Toxin", 0}
			};
			chest = u;
			head = u;
			rArm = u;
			lArm = u;
			rLeg = u;
			lLeg = u;
			
			bodyPartRefs = new Dictionary<string, SyncDictFloat> {
				{"Chest", chest},
				{"Head", head},
				{"RArm", rArm},
				{"LArm", lArm},
				{"RLeg", rLeg},
				{"LLeg", lLeg}
			};
		}

		// Start is called before the first frame update
		void Start()
		{	
			init();
		}
		
		public void damageMe(string bodyPart, string damageType, float amount) {
			if (isClient) {
				if (bodyPart == "All") {
					foreach(var v in bodyPartRefs.Values)
					{
						v[damageType] = Mathf.Min(
							v[damageType] + amount,
							maxDamage
						);
					}
				} else {
					bodyPartRefs[bodyPart][damageType] = Mathf.Min(
						bodyPartRefs[bodyPart][damageType] + amount,
						maxDamage
					);
				}
			}
		}
		
		public void healMe(string bodyPart, string damageType, float amount) {
			
		}
		/*
		public float getDamage(string bodyPart, string damageType) {
			return bodyPartRefs[bodyPart][damageType];
		}
		
		public float getDamageByType(string damageType) {
			switch (damageType) {
				case "Brute":
					return totalBrute;
					break;
				case "Burn":
					return totalBurn;
					break;
				case "Toxin":
					return totalToxin;
					break;
				case "Suffocation":
					return suffocation;
					break;
				default:
					Debug.LogWarning(
						"Call to health.getDamageByType(string damageType)"
						+ " with invalid damage type '" + damageType + "'! Check your spelling?"
					);
					return 0;
					break;
			}
		}
		*/
		void updateHealth() {
			totalBrute = 0;
			totalBurn = 0;
			totalToxin = 0;
			foreach(var v in bodyPartRefs.Values) {
				totalBrute += v["Brute"];
				totalBurn += v["Burn"];
				totalToxin += v["Toxin"];
			}
			totalHealth = 100 - totalBrute - totalBurn - totalToxin - suffocation;
		}

		// Update is called once per frame
		void Update()
		{
			
		}
	}
}