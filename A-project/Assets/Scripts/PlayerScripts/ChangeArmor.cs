using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeArmor : MonoBehaviour 
{

	public GameObject Preefab;  		 	// Переменная для префаба одежды
	public GameObject ChildPreefab;			// Сдесь мы храним меш, первый ребёнок префаба по иерархии
	public Transform RB;				 	// Сдесь мы храним RootBone
	
	void Start () 
	{	
		Preefab = Instantiate(Preefab,transform.position,Quaternion.identity)as GameObject; // Инстантируем префаб
		ChildPreefab = GameObject.Find("Mike");												// Находим и помещаем майку в переменную ChildPreefab
	//	List<GameObject> gos = SkinnedMeshTools.AddSkinnedMeshTo(ChildPreefab, RB, true);	// Вызываем скрипт SkinnedMeshTools
		SkinnedMeshTools.AddSkinnedMeshTo(ChildPreefab, RB, true);	// Вызываем скрипт SkinnedMeshTools
		Destroy(Preefab);																	// уничтожаем префаб
	}
}
