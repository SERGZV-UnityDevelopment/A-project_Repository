using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreObjectData
{
    public float WorldOffsetUp = 0.5f;  // Переменная для указания смещения текста вверх на определённое расстояние по глобальной оси вверх
    public float LocalOffsetZ;          // Переменная для указания смещения по локальной осиZ
    public float LocalOffsetX;          // Переменная для указания смещения по локальной осиX
    public string ObjectName;           // Игровое название объекта
    public string Description;          // Описание Объекта
    public Sprite ObjectTexture;        // Текстура объекта разрешением 148x148
    public Type ObjectType;             // Категория предмета "All", "Weapon", "Armor", "Food", "Magic", "Construction", "Other"
    public int Queue;                   // Цифра для предмета своей категории означающая после какого предмета он будет стоять в очереди
    public string Path;                 // Путь к префабу этого объекта
    public bool StackbleObject;         // Говорит ложиться ли данный объект в кучу с такими же объектами
    public int StackCount;              // Эта переменная указывает максимальное количество объектов в куче для данного префаба
    public bool DestroyableObject;      // Говорит разрушаемый ли этот объект или нет
    public float StateObject;			// Говорит в каком состоянии находиться объект, если он не разрушаемый то его состояние всегда 0%
}
