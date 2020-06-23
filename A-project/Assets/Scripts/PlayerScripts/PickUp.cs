﻿// Этот скрипт вешаем на Charecter basis он будет уничтожать предмет и ложить информацию о нём в выбраную им же ячейку в инвентаре.
// Также если инвентарь полон и в него пытаються положить ещё этот скрипт отрисовывает на экране надпись "Инвентарь полон".
// Сделать чтобы такие объекты как гриб складировались по много штук (На самом деле у нас будет в ячейке также лишь один гриб) Просто мы прибавим
// Для ячеёки ещё одну переменную которая отражает количество элементов максимальное значение которой равно переменной в скрипте этого гриба
// Как только количество грибов в ячейке равно или больше этого числа грибы начинают ложиться в следующуу ячейку
// В будующем сделать чтобы для категорий были не массивы а списки тогда она будут сами реорганизовываться при удалении элемента из инвентаря
// А ещё позже чтобы при открытии раздела инвентаря всё чтобы игра не тормозила тоже сделать этот раздел списком и вместо того чтобы заполнять
// этот раздел при его открытии мы как нибудь должны будем просматривать все ячейки на предмет категории на пример оружия потом идёт броняъ
// и если мы только что добавили броню значит и в список всё при подбирании объекта мы будем добавлять его в список всё. А при удалении из 
// какой нибуть категории будем искать в категории всё объект с такими же параметрами и удалять его оттуда тоже.
// В этом скрипте я использовал метафоры чтобы лучше понимать процесс который тут происходит Место на"Складе" это номера массивов с информацией
// об объектах, очередь с людьми у которых есть купоны это список с переменными int, ну и ячейки в инвентаре это ячейки в инвентаре :-)
using UnityEngine;

public class PickUp : MonoBehaviour 
{
	public Inventory Inv;			// Сдесь лежит скрипт Инвентраь
	public ObjectRegisrator OR;		// Сдесь лежит CharecterBasis пустышка на которой вести скрипт ObjectRegistrator
	ObjectData OD;					// Сдесь находиться скрипт ObjectData того объекта с которым мы в данный момент работаем
	int time = 0;					// Эта переменная указывает сколько времени будет показываться окошко "Инвентарь пуст"
	
	// !!!!!!!!!!!!!!! Нужно сделать чтобы когда мы подбираем объект то ячейка категории всё где будет находиться наш объект должна стать
	// выбранной а также ячейка той категории которой был выбран наш объект тоже должна стать выбранной где лежит объект
	
	void Update()
	{
		// Если нажата левая кнопка мыши и инвентарь выключен и в скрипте ObjectRegistrator переменная FocusObject не равана нулю
		if(Input.GetMouseButtonDown(0) & Inv.InventoryOn == false & OR.FocusObject != null)		
		{
			if(Inv.EngagedSlots < 40)	// Если максимальное количество слотов не достигнуто
			{
				PickUpObject();			// Вызываем метод подбирания объекта
			}
			else 						// Иначе если максимальное количество слотов достигнуто
			{
				time = 100;				// Ставим счётчик времени time на 100;
			}
		}
		if(time > 0)	// Если переменная time больше ноля
			time --;	// То отнимаем у неё еденицу
	}

	
	void PickUpObject()
	{
		OD = OR.FocusObject.GetComponent<ObjectData>();	// Ложим скрипт ObjectData из обрабатываемого объекта в переменную OD;
		FindASuitablePlace();							// Вызываем метод FindASuitablePlace
		Destroy(OR.FocusObject);						// Удаляем объект в фокусе
	}


	// Этот метод находит подходящее место в "Очереди" и вставляет туда объект
	void FindASuitablePlace()	
	{
		int PlaceInLine	= 0;			// Вычисляем номер "Нового человека" в очереди
		int WarehousePlace = 0;			// Номер заполняемого слота на складе

		if(OD.StackbleObject == true) // Если объект в фокусе стакуемый то мы стараемся найти ячейку с таким же объектом и прибавить к ней +1
		{
			// Проводим итерацию по очереди людей с 0 до конца, спрашиваем у каждого талон, идём по нему на склад и там смотрим на тип и номер
			while(Inv.Coupons[PlaceInLine] != -1) // Если у "Человека в очереди" номер в талоне равен -1 то прерываем цикл
			{
				if(OD.ObjectType == Inv.TypesObjects[Inv.Coupons[PlaceInLine]]) // Если тип объекта в фокусе равен типу объекта "человека в оче.."
				{
					// Если Queue объекта в фокусе равен Queue "человека в очереди" то есть мы нашли точно такой же объект
					if(OD.Queue == Inv.QueueObject[Inv.Coupons[PlaceInLine]]) 
					{
						// И если у объекта в фокусе макс. число стака больше чем лежит в ячейке на складе того "Человека"
						if(OD.StackCount > Inv.StackValue[Inv.Coupons[PlaceInLine]])
						{
							Inv.StackValue[Inv.Coupons[PlaceInLine]] ++; // То мы плюсуем к переменной отвечающей за количество предметов в ячейке
							break; // И прерываем цикл
						}
					}
					// Иначе если Queue объекта в фокусе меньше Queue "человека в очереди" то есть мы наткнулись на объект старше по очереди
					else if(OD.Queue < Inv.QueueObject[Inv.Coupons[PlaceInLine]]) 
					{
						InsertNewCoupon(PlaceInLine, WarehousePlace); // Вызываем метод "Вставить нового купона"
						Inv.StackValue[Inv.Coupons[PlaceInLine]] ++; // То мы плюсуем к переменной отвечающей за количество предметов в ячейке
						break; // И прерываем цикл
					}

				}
				else if(OD.ObjectType < Inv.TypesObjects[Inv.Coupons[PlaceInLine]])
				{
					InsertNewCoupon(PlaceInLine, WarehousePlace); // Вызываем метод "Вставить нового купона"
					Inv.StackValue[Inv.Coupons[PlaceInLine]] ++; // То мы плюсуем к переменной отвечающей за количество предметов в ячейке
					break; // И прерываем цикл
				}
				PlaceInLine++; // Прибавляем к PlaceInLine чтобы в следующем цикле опрашивать следующего человека в очереди
			}
			if(Inv.Coupons[PlaceInLine] == -1)
			{
				InsertNewCoupon(PlaceInLine, WarehousePlace); // Вызываем метод "Вставить нового купона"
				Inv.StackValue[Inv.Coupons[PlaceInLine]] ++; // То мы плюсуем к переменной отвечающей за количество предметов в ячейке
			}
		}
		else if(OD.StackbleObject == false) // Иначе если объект в фокусе нестакуемый то мы ищем свободное место 
		{
			// Проводим итерацию по очереди людей с 0 до конца, спрашиваем у каждого талон, идём по нему на склад и там смотрим на тип и номер
			while(Inv.Coupons[PlaceInLine] != -1) // Если у "Человека в очереди" номер в талоне равен -1 то прерываем цикл
			{
				if(OD.ObjectType == Inv.TypesObjects[Inv.Coupons[PlaceInLine]]) // Если тип объекта в фокусе равен типу объекта "человека в оче.."
				{
					if(OD.Queue < Inv.QueueObject[Inv.Coupons[PlaceInLine]])	// Если Queue объекта в фокусе меньше Queue "человека в очереди"
					{
						break; // То прерываем цикл
					}
				}
				if(OD.ObjectType < Inv.TypesObjects[Inv.Coupons[PlaceInLine]])  // Если тип объекта в фокусе меньше типа объекта "человека в оче."
				{
					break; // То прерываем цикл
				}
				PlaceInLine++; // Прибавляем к PlaceInLine чтобы в следующем цикле опрашивать следующего человека в очереди
			}
			InsertNewCoupon(PlaceInLine, WarehousePlace); // Вызываем метод "Вставить нового купона"
			Inv.StackValue[Inv.Coupons[PlaceInLine]] ++; // И мы плюсуем к переменной отвечающей за количество предметов в ячейке
		}
	}


	// Этот метод вставляет нового человека в вычесленное место в очереди и присваевает ему место на складе куда и ложит все его данные
	void InsertNewCoupon(int PlaceInLine, int WarehousePlace)
	{
		WarehousePlace = FindAndfillWarPlace();		// Вызываем метод FindWarehousePlace и присваиваем переменной WarehousePlace EmptyPlace
		Inv.Coupons.Insert(PlaceInLine, WarehousePlace); 	// Вставляем в очередь нового человека с номером места на "Складе"
		Inv.Coupons.RemoveAt(40);						 	// Удаляем 41 место в "Очереди"
		CategoryChanged(Inv.TypesObjects[WarehousePlace]); 	// Вызываем метод CategoryChanged и передаём ему тип подобранного объекта
		Inv.EngagedSlots ++;								// Прибавляем к количеству занятых слотов +1
	}
	

	// "FindAndfillWarehousePlace" Найти и заполнить. Этот метод ищет по именам объектов в массиве NamesObject первое пусто место на "Складе",
	// помещает туда всю информацию об объекте и возвращает в переменную WarehousePlace номер этого места на складе
	int FindAndfillWarPlace()
	{
		int EmptyPlace = 0; // Эта переменная символизирует пустое место на складе

		while(Inv.NamesObjects[EmptyPlace] != "")	// Закончим итерацию когда наткнёмся на пустое имя на "Складе"
		{
			EmptyPlace++;
		}

		Inv.NamesObjects[EmptyPlace] = OD.ObjectName;			// Ложим на пустое место на "Складе" имя объекта в фокусе
		Inv.DescriptionsObjects[EmptyPlace] = OD.Description;	// Ложим "рядом" на тоже место на "Складе" описание этого же объекта
		Inv.TexturesObjects[EmptyPlace]	= OD.ObjectTexture;		// Ложим "рядом" на тоже место на "Складе" текстуру этого же объекта
		Inv.TypesObjects[EmptyPlace] = OD.ObjectType;			// Ложим "рядом" на тоже место на "Складе" тип этого же объекта
		Inv.QueueObject[EmptyPlace] = OD.Queue;					// Ложим "рядом" на тоже место на "Складе" порядочный номер объекта
		Inv.PathsObjects[EmptyPlace] = OD.Path;					// Ложим "рядом" на тоже место на "Складе" путь к префабу этого объекта
		Inv.Destroyable[EmptyPlace] = OD.DestroyableObject;		// Ложим "рядом" на тоже место на "Складе" состояние объекта разрушаемый или нет
		Inv.State[EmptyPlace] = OD.StateObject;					// Ложим "рядом" на тоже место на "Складе" насколько объект сломан
		return EmptyPlace;										// Возвращаем пустое место
	}


	// Этот метод опрашивает категорию подобранного предмета и сообщает какая категория инвентаря была изменена чтобы обновить её при открытии
	void CategoryChanged(Type Types)
	{
		Inv.InvChanged = true;					// Изменяем значение переменной "Инвентарь был изменён" на значение правда
		if(OD.ObjectType == Type.Weapon)		// Если тип объекта "Оружие"
			Inv.WeapChanged = true;				// То делаем переменную WeapChanged равной правда
		if(OD.ObjectType == Type.Armor)			// Если тип объекта "Броня"
			Inv.ArmorChanged = true;			// То делаем переменную ArmorChanged равной правда
		if(OD.ObjectType == Type.Food)			// Если тип объекта "Еда"
			Inv.FoodChanged = true;				// То делаем переменную FoodChanged равной правда
		if(OD.ObjectType == Type.Magic)			// Если тип объекта "Магия"
			Inv.MagicChanged = true;			// То делаем переменную MagicChanged равной правда
		if(OD.ObjectType == Type.Construction)	// Если тип объекта "Строительство"
			Inv.ConsChanged = true;				// То делаем переменную ConsChanged равной правда
		if(OD.ObjectType == Type.Other)			// Если тип объекта "Разное"
			Inv.OtherChanged = true;			// То делаем переменную OtherChanged равной правда
	}

	// Этот метот OnGUI в этом скрипте занимаеться только тем что отрисовывает текст "Инвентарь полон" когда TextTrue равна правда
	void OnGUI()
	{
		if(time >0)	// Пока переменная time больше ноля показываем надпись что инвентарь полон
			GUI.Label(new Rect(590, 10, 200, 200), "Инвентарь полон");
	}
}
