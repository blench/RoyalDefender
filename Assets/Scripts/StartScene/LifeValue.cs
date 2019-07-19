using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeValue : MonoBehaviour {

	public int lifeValue { set; get;}
	public Text LifeValueText;
	// Use this for initialization
	void Start () {
		lifeValue = 100;
	}
	
	// Update is called once per frame
	void Update () {
		LifeValueText.text = this.lifeValue + "";
	}
}
