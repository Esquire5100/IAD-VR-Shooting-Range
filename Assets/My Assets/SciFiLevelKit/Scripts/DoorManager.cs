using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	public LvlKitDoor door1;
	public LvlKitDoor door2;
	
	
	void OnTriggerEnter(){
		
		if (door1!=null){
			door1.OpenDoor();	

		}
		
		if (door2!=null){
			door2.OpenDoor();	

		}
	}
}
