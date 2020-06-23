using UnityEngine;

public class TestInventoryTwo : MonoBehaviour 
{
	public GUIStyle MyStyle; 					 // Указываем стиль для ячеек
	int	cellWidth = 64, cellHight = 64;			 // Высота и ширина одной ячейки
	int MenuLeft = 100, MenuTop = 100;			 // Указываем положение первой ячейки
	public Rect[] Cells = new Rect[40]; 		 // Объявляем массив ячеек и его количество
	public int[] NumberOfStack = new int[40];	 // Массив со значениями на каждую клетку
	int NumberOfCellsInString = 5;		 		 // Количество ячеек в строке


	void Start()
	{
		GeneratePosition(MenuLeft, MenuTop, cellWidth, cellHight);
	}


	// Этот метод при старте вычисляет положение и размер всех ячеек в массиве на основе первоначальных указанных данных
	void GeneratePosition(int posLeft, int posTop, int CellWidth, int CellHight)
	{
		int Width = MenuLeft;	// Положение обрабатываемой ячейки по ширине
		int Hight = MenuTop;	// Положение обрабатываемой ячейки по высоте
		int Number = 0;			// Номер обрабатываемой ячейки в строке
	
		// Задаём положение и размер всем ячейкам в массиве Cells
		for(int i=0; i<Cells.Length; i++) // Продолжаем итерацию пока не установим места для всех ячеек в меню
		{
			Cells[i] = new Rect(Width, Hight, CellWidth, CellHight); // Задаём очередной ячейке из массива размер и место

			if(Number < NumberOfCellsInString-1) // Если номер обрабатываемой ячейки в строке меньше Количества ячеек в строке минус один
			{
				Number ++; 				  // То прибавляем к Numbrer еденицу
				Width = Width + CellWidth;// А также прибавляем к Width ширину клетки
			}
			else 						  // Если мы указали положение последней ячейки в строке
			{
				Number = 0; 				// То задаём переменной Number значение 0
				Hight = Hight + CellHight; 	// Прибавляем к Hight высоту ячейки
				Width = MenuLeft;			// А также ставим Width первоначальное значение
			}
		}
	}

	
	void OnGUI()
	{
		for(int i=0; i<Cells.Length; i++) // Продолжаем цикл пока номер цикла меньше количества клеток
			if(NumberOfStack[i] != 0)	  // Если значение количества предметов в ячейке не равно нулю
				GUI.Box(Cells[i], ""+NumberOfStack[i], MyStyle); // Отрисовываем ячейку и заполняем её осмысленным содержанием
			else 								// Иначе если значение равно нулю
				GUI.Box(Cells[i], "", MyStyle); // Создаём ячейку не указывая в ней количество предметов
	}
}
