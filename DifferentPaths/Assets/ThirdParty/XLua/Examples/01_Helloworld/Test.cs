using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		GameObject go = GameObject.FindGameObjectWithTag("MainCamera");
		var helloWorld = go.GetComponent<XLuaTest.Helloworld>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
