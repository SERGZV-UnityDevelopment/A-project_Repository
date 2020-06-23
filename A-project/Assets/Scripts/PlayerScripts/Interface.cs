using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour 
{
	// Текстура Жизни, Выносливости , Маны и три рамки куда это всё вставляеться
	public Texture Health, Endurance, Mana, FrameLife, FrameEndurance, FrameMana; 
	int MaxHealth = 50;		   // Максимальное здоровье игрока
	int CurrentHealth = 50;	   // Текущее здоровье игрока
	int MaxEndurance = 50;	   // Максимальная выносливость игрока
	int CurrentEndurance = 50; // Текущая выносливость игрока
	int	MaxMana = 10;		   // Максимальная мана игрока
	int CurrentMana = 10;	   // Текущая мана игрока


	void Update()
	{
		//CurrentHealth -= 1;

	}


	void OnGUI()
	{
		GUI.DrawTexture(new Rect(10, 824, 182, 22), FrameLife);			// Рисуем первую рамку
		GUI.DrawTexture(new Rect(549, 824, 182, 22), FrameEndurance);	// Рисуем вторую рамку
		GUI.DrawTexture(new Rect(1088, 824, 182, 22), FrameMana);		// Рисуем третюю рамку
		GUI.DrawTexture(new Rect(12, 826, 178*CurrentHealth/MaxHealth, 18), Health);		    // Отрисовываем полоску жизни
		GUI.DrawTexture(new Rect(551, 826, 178*CurrentEndurance/MaxEndurance, 18), Endurance);  // Отрисовываем Выносливость
		GUI.DrawTexture(new Rect(1090, 826, 178*CurrentMana/MaxMana, 18), Mana); 				// Отрисовываем Ману
	}
}
