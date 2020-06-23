using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SkinnedMeshTools
{
	// Объявляем метод AddSkinMeshTo а тип его возвращаемого значения List в котором содержаться GameObjects
	// Возвращаем в лист все новые skinned mesh renderer добавленные к root.
	// Указываем рекурсивно объекты как неактивные.
	public static List<GameObject> AddSkinnedMeshTo( GameObject obj, Transform root )
	{ 
		return AddSkinnedMeshTo(obj, root, true); 
	}

	// Объявляем воторой метод
	// Возвращаем в лист все новые skinned mesh renderer added to root. Set recursively obj as inactive if hideFromObj is true.
	public static List<GameObject> AddSkinnedMeshTo( GameObject obj, Transform root, bool hideFromObj )
	{
		List<GameObject> result = new List<GameObject>();	// Создаём list с именем результат
		
		// Здесь, Объект кость должен быть Помещён на сцену и активен (По край не й мере один с рендером),
		// или ещё GetComponentsInChildren не будет работать.
		SkinnedMeshRenderer[] BonedObjects = obj.GetComponentsInChildren<SkinnedMeshRenderer>();

		// для каждого (Обявляем переменную скинмешрендер под именем smr)
		// В переменную цикла smr из массива boneObjects по очереди помещаем объекты а именно скинмешрендеры
		foreach( SkinnedMeshRenderer smr in BonedObjects )
			// Каждый цикл мы добавляем в лист "результат", результат действия метода ProcessBonedObject
			result.Add( ProcessBonedObject( smr, root )); 
		
		if(hideFromObj)				// Если значение hideFromObj true это третий параметр данного метода
			obj.SetActive( false ); // То мы устанавливаем в неактивное состояние obj это первый параметр данного метода
		
		return result;		// Возвращаем результат в list результат
	}

	// Объявляем третий метод ProcessBoneObject с возвращаемым значением GameObject, и его параметрами SkinedMeshRender с именем ThisRender
	// и transform с именем root
	private static GameObject ProcessBonedObject(SkinnedMeshRenderer ThisRenderer, Transform root)
	{		
		// Создать субобъект
		// Создаём переменную GameObject с именем newObject и присваиваем ей новый объект с именем объекта на котором весит этот скинмешрендер
		GameObject newObject = new GameObject( ThisRenderer.gameObject.name );	
		newObject.transform.parent = root;    // Присваиваем этому объекту родителя root второй параметр этого метода
		
		// Добавляем рендер
		// Создаём новый МешРендер и присваиваем ему скинмешрендер.
		SkinnedMeshRenderer NewRenderer = newObject.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
		
		// Собираем структуру кости
		// Создаём массив Transform с именем (MyBones)- Мои кости с количеством элементов равных количеству костей в ThisRenderer первом параметре
		// этого метода
		Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
		
		// Как и клипы, использующие кости своими именами, мы находим их таким образом
		// Продолжаем цикл до тех пор пока i меньше количества костей ThisRenderer
		for( int i = 0; i < ThisRenderer.bones.Length; i++ )
			// Заполняем массив MyBones костями из ThisRendered
			MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, root);
		
		// Собираем рендер	
		NewRenderer.bones = MyBones;							// Скинмешрендеру присваиваем кости из массива MyBones
		NewRenderer.sharedMesh = ThisRenderer.sharedMesh;		// Скинмешрендеру присваиваем шейдер от ThisRenderer
		NewRenderer.materials = ThisRenderer.materials;			// Скинмешрендеру присваиваем шейдер от ThisRenderer
		
		return newObject;										// Возвращаем newObject в вызвающую часть программы
	}
	
	// Recursive search of the child by name.
	private static Transform FindChildByName( string ThisName, Transform ThisGObj )	
	{	
		Transform ReturnObj;
		
		// If the name match, we're return it
		if( ThisGObj.name == ThisName )	
			return ThisGObj.transform;
		
		// Else, we go continue the search horizontaly and verticaly
		foreach( Transform child in ThisGObj )	
		{	
			ReturnObj = FindChildByName( ThisName, child );
			
			if( ReturnObj != null )	
				return ReturnObj;	
		}
		
		return null;	
	}
}
