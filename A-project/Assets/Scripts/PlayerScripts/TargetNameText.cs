// Этот скрипт вешаем на камеру он управляет текстом который должен находиться в центре сцены и не быть никому дочерним. Скрипт занимаеться 
// отображением текста над отфильтрованным объектом однако удаление объекта нужно перенести отсюда придумать куда а также в том скрипте 
// разместить его конвертацию из мира в инвентарь.
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetNameText : MonoBehaviour 
{
	public Camera Cam;						// Сдесь лежит сама камера на которой и весит этот скрипт
	public ObjectRegisrator OR;				// Сдесь лежит CharecterBasis пустышка на которой вести скрипт ObjectRegistrator
	public Text TargetNametext;             // This is the text displaying the name of the object in focus.
    public float ClampBorderSize = 0.01f;	// Как далеко нужно будет выдти тексту за экран прежде чем он зажмёться на месте
	public Inventory Inv;					// Сдесь лежит скрипт интерфейс
	ObjectData OD;							// Сдесь лежит скрипт ObjectData (того активного объекта что в фокусе)
	public bool WasZeroed = true;			// Была ли обнулёна переменная FocusObject
	public GameObject FocusObject;			// Это объект который в фокусе камеры
//	public Transform GuiText;				// Сюда ложим трансформацию GUIText,a...


	void Start()
	{
//		GuiText = TargetNametext.transform;	// ложим трансформацию GUIText,a в GuiText
        TargetNametext.enabled = false;     // At start, turn off the display of this text
    }

	void Update () 
	{
		if(Inv.InventoryOn == false)			                    // Если инвентарь выключен
		{
			if(OR.FocusObject != null)			                    // И если в скрипте ObjectRegistrator переменная FocusObject не равана нулю
			{
				ObjectText();					                    // Вызываем метод ObjectText
				WasZeroed = false;                                  // 
			}
			else if(FocusObject == null & WasZeroed == false)
			{
                TargetNametext.enabled = false;                             // Turn off the display of the name of the object in focus
//                GuiText.transform.position = new Vector3(0, -0.5f, 0);    // То мы задаём тексту позицию где он не будет виден
				WasZeroed = true;
			}
			else if(OR.FocusObject == null & FocusObject != null)   // Если FocusObject (в том скрипте) равен нулю и сдешний FocusObject ещё неравен
				TurnOffText();								        // Вызываем метод TurnOffText
		}

		if(Inv.InventoryOn == true & FocusObject != null)	        // Если же инвентарь включен и FocusObject не равна нулю
		{
			TurnOffText();								            // Вызываем метод TurnOffText										
		}
	}
	
	void ObjectText()									// Этот метод вешает текст над объектом и указывает его имя
	{
		FocusObject = OR.FocusObject;					// То мы перемещаем из той переменной FocusObject объект в эту переменную FocusObject
		OD = FocusObject.GetComponent<ObjectData>();	// Находим скриппт ObjectData объекта в фокусе
		TargetNametext.text = OD.ObjectName;			// Присваеваем тексту GUIText,a имя из скрипта ObjectName
		 

		// Берём позицию цели плюсуем к ней offset'ы (Все смещения текста для этого объекта), конвертируем эту позицию в локальные координаты
		// камеры и присваиваем эту позицию тексту
		GuiText.position = Cam.WorldToViewportPoint(FocusObject.transform.position + Vector3.up * OD.WorldOffsetUp
		        + FocusObject.transform.right * OD.LocalOffsetX + FocusObject.transform.forward * OD.LocalOffsetZ);
		// Зажимаем значение позиции этого объекта по "x" в пределах "ClampBorderSize" (предел экрана) и еденицей от которой отняли
		// "ClampBorderSize" (предел экрана) и присваиваем это значение "x" нового вектора. Зажимаем значение позиции этого объекта
		// по "y" в пределах "ClampBorderSize" (предел экрана) и еденицей от которой отняли "ClampBorderSize" (предел экрана) и
		// присваиваем это значение "y" нового вектора. Берём позицию объекта "z" на котором висит скрипт и присваивем её "z" нового
		// вектора. Всё новый вектор собран теперь мы присваиваем значения нового вектора позиции объекта на котором висит скрипт.
		GuiText.position = new Vector3(Mathf.Clamp(GuiText.position.x, ClampBorderSize, 1.0f - ClampBorderSize), Mathf.Clamp(GuiText.position.y,
		                                                               ClampBorderSize, 1.0f - ClampBorderSize), GuiText.position.z);
	}
	
	
	public void TurnOffText() // This method turns off the text displaying the object in focus.
    {
        TargetNametext.enabled = false;                             // Turn off the display of the name of the object in focus
        // GuiText.transform.position = new Vector3(0, -0.5f, 0); 	// То мы задаём тексту позицию где он не будет виден
		FocusObject = null;										// И обнуляем сдешнюю FocusObject
	}
}
