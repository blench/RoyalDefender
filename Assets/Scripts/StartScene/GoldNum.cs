using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldNum : MonoBehaviour {

	public int goldNum{ set; get;}
	public Text GoldText;
	// Use this for initialization
	void Start () {
		this.goldNum = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		GoldText.text = this.goldNum + "";
	}
}
