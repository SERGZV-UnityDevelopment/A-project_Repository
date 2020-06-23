using UnityEngine;
using System.Collections;

public class CalculatePosition
{
	// Этот метод при старте вычисляет положение и размер всех ячеек в массиве на основе первоначальных указанных данных
	// (Позиция по ширине, Позиция по высоте, Ширина клетки, Высота клетки, Массив с координатами клеток, Клеток в одной строке)
	public static void GeneratePosition(int posLeft, int posTop, int CellWidth, int CellHight, Rect[] Cells, int CellsInString)
	{
		int Width = posLeft;	// Положение обрабатываемой ячейки по ширине
		int Hight = posTop;		// Положение обрабатываемой ячейки по высоте
		int Number = 0;			// Номер обрабатываемой ячейки в строке
		
		// Задаём положение и размер всем ячейкам в массиве Cells
		for(int i=0; i<Cells.Length; i++) // Продолжаем итерацию пока не установим места для всех ячеек в меню
		{
			Cells[i] = new Rect(Width, Hight, CellWidth, CellHight); // Задаём очередной ячейке из массива размер и место
			
			if(Number < CellsInString-1) // Если номер обрабатываемой ячейки в строке меньше Количества ячеек в строке минус один
			{
				Number ++; 				  // То прибавляем к Numbrer еденицу
				Width = Width + CellWidth;  // А также прибавляем к Width ширину клетки
			}
			else 						  // Если мы указали положение последней ячейки в строке
			{
				Number = 0; 			  // То задаём переменной Number значение 0
				Hight = Hight + CellHight;   // Прибавляем к Hight высоту ячейки
				Width = posLeft;		  // А также ставим Width первоначальное значение
			}
		}
	}
}
