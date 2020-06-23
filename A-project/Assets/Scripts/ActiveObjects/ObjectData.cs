using UnityEngine;
using System.Collections;

// Создаём перечисление всех типов предметов вне класса чтобы он был виден другим скриптам
public enum Type : byte {undefined, Weapon, Armor, Food, Magic, Construction, Other}

public class ObjectData : MonoBehaviour 
{
	public float WorldOffsetUp = 0.5f;	// Переменная для указания смещения текста вверх на определённое расстояние по глобальной оси вверх
	public float LocalOffsetZ;			// Переменная для указания смещения по локальной осиZ
	public float LocalOffsetX;			// Переменная для указания смещения по локальной осиX
	public string ObjectName;			// Игровое название объекта
	public string Description;			// Описание Объекта
	public Texture ObjectTexture;		// Текстура объекта разрешением 148x148
	public Type ObjectType;				// Категория предмета "All", "Weapon", "Armor", "Food", "Magic", "Construction", "Other"
	public int Queue;					// Цифра для предмета своей категории означающая после какого предмета он будет стоять в очереди
	public string Path;					// Путь к префабу этого объекта
	public bool StackbleObject;			// Говорит ложиться ли данный объект в кучу с такими же объектами
	public int StackCount;				// Эта переменная указывает максимальное количество объектов в куче для данного префаба
	public bool DestroyableObject;		// Говорит разрушаемый ли этот объект или нет
	public float StateObject;			// Говорит в каком состоянии находиться объект, если он не разрушаемый то его состояние всегда 0%

	
	void Start()
	{
		if(!GetComponent<Rigidbody>())
		{
			Debug.LogError("У " + ObjectName + " отсутствует компонент rigitbody, добавьте его.");
		}
		if(!ObjectTexture)
		{
			Debug.LogError("У " + ObjectName + " отсутствует 2D текстура для инвентаря добавьте её.");
		}
	}
}



