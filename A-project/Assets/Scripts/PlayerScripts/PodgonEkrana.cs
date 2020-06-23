using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PodgonEkrana : MonoBehaviour
{
	private float posX = 0;                   // Переменная позиция гуи текстуры X равна 0.
	private float posY = 0;                   // Переменная позиция гуи текстуры Y равна 0.
	private float shirina = 0;                // Ширина гуи текстуры на который вешаеться скрипт
	private float vysota = 0;                 // Высота гуи текстуры на который вешаеться скрипт
	private float scrinWidth = 0;             // Переменная ширина экрана
	private float scrinHeight = 0;            // Переменная высота экрана
	private float scrinBalansWidth = 0;       // Переменная баланс экрана по ширине (БЭШ)
	private float scrinBalansHeight = 0;      // Переменная баланс экрана по высоте (БЭВ)
	
	void Start()
	{
//      posX = GetComponent<GUITexture>().pixelInset.x;          // Ложим в переменную posX
//		posY = GetComponent<GUITexture>().pixelInset.y;          // Выравниваем гуи текстуру по позиции переменной Posy равной нулю.
//		shirina = GetComponent<GUITexture>().pixelInset.width;   // Выравниваем ширину гуи текстуры по ширине экрана
//		vysota = GetComponent<GUITexture>().pixelInset.height;   // Выравниваем высоту гуи текстуры по высоте экрана
		scrinWidth = Screen.width;               // Переменной ширина экрана присваиваем ширину экрана
		scrinHeight = Screen.height;             // Переменной высота экрана присваиваем высоту экрана
		scrinBalansWidth = 1452 / scrinWidth;    // Переменной (БЭШ) присваиваем разрешение 1452 делённое на ширину экрана
		scrinBalansHeight = 910 / scrinHeight;   // Переменной (БЕВ) присваиваем разрешение 910 делённое на выстоу экрана
		Balans ();
	}
	void Balans () 
	{
		// Рисуем новый прямоугольник в котором Переменную позиции гуи текстуры X делим на (БЕШ), Переменную позиции гуи текстуры Y делим на (БЕВ),
		// Ширину гуи текстуры делим на (БЕШ), Высоту гуи текстуры делим на (БЕВ).
		Rect newRect = new Rect(posX / scrinBalansWidth, posY / scrinBalansHeight, shirina / scrinBalansWidth, vysota / scrinBalansHeight);
		// выравниваем гуи текстуру по только что нарисованному прямоугольнику.
//		GetComponent<GUITexture>().pixelInset = newRect ;
	}
	
}