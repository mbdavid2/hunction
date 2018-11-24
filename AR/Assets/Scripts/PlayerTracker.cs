﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerTracker : MonoBehaviour {
    private IEnumerator coroutine;
    private int id = 0;

    public Coordenadas coordenadas = null;
 	public Coordenadas primeras = null;

    public GameObject[] monstruos;

 	public Coordenadas getRelativasPrimeras() {
 		if (primeras != null && coordenadas != null) {
 			return new Coordenadas(
 				coordenadas.lat - primeras.lat,
 				coordenadas.lng - primeras.lng);
 		}
 		return null;
 	}

    void Start() {
        coroutine = WaitAndGet(30.0f);
        StartCoroutine(coroutine);
    }

    // every 2 seconds perform the print()
    private IEnumerator WaitAndGet(float waitTime) {
        while (true) {
            StartCoroutine(Get());
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator Get() {
        UnityWebRequest www = UnityWebRequest.Get(
        	"http://hunction2018.herokuapp.com/clients/94:65:2d:62:72:eb");
    	yield return www.SendWebRequest();

    	if (www.isNetworkError) {
    		Debug.Log("Network error: " + www.error);
    	}
    	if (www.isHttpError) {
    		Debug.Log("Http error: " + www.error);
    	}

    	string sJason = www.downloadHandler.text;
    	coordenadas = JsonUtility.FromJson<Coordenadas>(sJason);
    	if (coordenadas != null) {
        	Debug.Log("New coords: " + coordenadas.lat + ", " + coordenadas.lng);
        	if (primeras == null) {
        		primeras = coordenadas;
        	}
        }
    }

    void setUpMonsters() {
    	Coordenadas monstruo1 = new Coordenadas(
    		60.18499131783053 - primeras.lat,
    		24.83297900744435 - primeras.lng);

    	foreach(var m in monstruos) {
    		m.transform.position = new Vector3((float) monstruo1.lat * 100000, (float) monstruo1.lng * 100000);
    	}
    }
}
