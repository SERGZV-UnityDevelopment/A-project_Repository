// Этот скрипт вешаем на Charecter basis он будет уничтожать предмет и ложить информацию о нём в выбраную им же ячейку в инвентаре. // Также если инвентарь полон и в него пытаються положить ещё этот скрипт отрисовывает на экране надпись "Инвентарь полон". // В одной ячейке склада может лежать несколько объектов но на самом деле в ячейке будет лишь один объект, просто мы прибавим // Для ячеёки ещё одну переменную которая отражает количество элементов максимальное значение которой равно переменной в скрипте этого предмета // Как только количество предметовв ячейке равно или больше этого числа предметы начинают ложиться в следующуу ячейку. // В этом скрипте я использовал метафоры чтобы лучше понимать процесс который тут происходит  // Место на "Складе" это номер массива с информацией о подобранном объекте - объектах, // Coupons - очередь с людьми у которых есть купоны это список с переменными int // Ну и ячейки в инвентаре это ячейки в инвентаре для текущей категории которые ссылаются на купона которые ссылаются на ячейки на складе :-) using UnityEngine;  public class PickUp : MonoBehaviour  { 	public Inventory Inv;			// Сдесь лежит скрипт Инвентраь 	public ObjectRegisrator OR;		// Сдесь лежит CharecterBasis пустышка на которой вести скрипт ObjectRegistrator 	ObjectData OD;                  // Сдесь находиться скрипт ObjectData того объекта с которым мы в данный момент работаем     StoreObjectData SOD;            // Это объект для хранения данных на складе 	int time = 0;					// Эта переменная указывает сколько времени будет показываться окошко "Инвентарь пуст" 	 	void Update() 	{ 		// Если нажата левая кнопка мыши и инвентарь выключен и в скрипте ObjectRegistrator переменная FocusObject не равана нулю 		if(Input.GetMouseButtonDown(0) & Inv.InventoryOn == false & OR.FocusObject != null)		 		{ 			if(Inv.Coupons.Count < 40)	    // Если максимальное количество слотов не достигнуто 			{ 				PickUpObject();             // Вызываем метод подбирания объекта
                Inv.FillCategory((Type)Inv.ActiveCategory); // Call the update method of the current category
            } 			else 						    // Иначе если максимальное количество слотов достигнуто 			{ 				time = 100;				    // Ставим счётчик времени time на 100; 			} 		} 		if(time > 0)	// Если переменная time больше ноля 			time --;	// То отнимаем у неё еденицу 	}  	 	void PickUpObject() 	{         OD = OR.FocusObject.GetComponent<ObjectData>();     // We put the script of the stored object into the variable "OD"          SOD = new StoreObjectData()
        {
            // Copy all the data from the selected object into it.
            WorldOffsetUp = OD.WorldOffsetUp,             LocalOffsetZ = OD.LocalOffsetZ,             LocalOffsetX = OD.LocalOffsetX,             ObjectName = OD.ObjectName,             Description = OD.Description,             ObjectTexture = OD.ObjectTexture,             ObjectType = OD.ObjectType,             Queue = OD.Queue,             Path = OD.Path,             StackbleObject = OD.StackbleObject,             StackCount = OD.StackCount,             DestroyableObject = OD.DestroyableObject,             StateObject = OD.StateObject,
        };          FindASuitablePlace();							// Вызываем метод FindASuitablePlace 		Destroy(OR.FocusObject);                        // Удаляем объект в фокусе
    }   	// Этот метод находит подходящее место в "Очереди" и вставляет туда объект 	void FindASuitablePlace()	 	{ 		if(OD.StackbleObject == true)   // Если объект в фокусе стакуемый то мы стараемся найти ячейку с таким же объектом и прибавить к ней +1 		{
            // Проводим итерацию по очереди людей с 0 до конца, спрашиваем у каждого талон, идём по нему на склад и там смотрим на тип и номер
            if(Inv.Coupons.Count > 0 && Inv.Coupons.Count < 40)     // Если в очереди уже есть купоны
            {
                for (byte a = 0; a < Inv.Coupons.Count; a++)        // Продолжаем цикл пока не переберём всех купонов в очереди
                {                     if (OD.ObjectType == Inv.ObjectsInStore[Inv.Coupons[a]].ObjectType) // Если тип объекта в фокусе равен типу объекта "человека в очереди"
                    {
                        // Если Queue объекта в фокусе равен Queue "человека в очереди" то есть мы нашли точно такой же объект
                        if (OD.Queue == Inv.ObjectsInStore[Inv.Coupons[a]].Queue)
                        {
                            // И если у объекта в фокусе макс. число стака больше чем лежит в ячейке на складе того "Человека"
                            if (OD.StackCount > Inv.ObjectsInStore[Inv.Coupons[a]].StackValue)
                            {                                 Inv.ObjectsInStore[Inv.Coupons[a]].StackValue++;                  // Plus the variable responsible for the number of items in the cell
                                break; // И прерываем цикл
                            }                             else // Otherwise, if this cell is already filled with this kind of objects
                            {
                                InsertNewCoupon(a);                                   // Вызываем метод "Вставить нового купона"                                 Inv.ObjectsInStore[Inv.Coupons[a]].StackValue++;      // And plus the variable responsible for the number of items in the cell 						        break;  // И прерываем цикл
                            }
                        }
                        // Иначе если Queue объекта в фокусе меньше Queue "человека в очереди" то есть мы наткнулись на объект старше по очереди
                        else if (OD.Queue < Inv.ObjectsInStore[Inv.Coupons[a]].Queue)
                        {
                            InsertNewCoupon(a);                                   // Вызываем метод "Вставить нового купона"                             Inv.ObjectsInStore[Inv.Coupons[a]].StackValue++;      // And plus the variable responsible for the number of items in the cell
                            break;  // И прерываем цикл
                        }
                    }                     else if (OD.ObjectType < Inv.ObjectsInStore[Inv.Coupons[a]].ObjectType)
                    {
                        InsertNewCoupon(a); // Вызываем метод "Вставить нового купона"                         Inv.ObjectsInStore[Inv.Coupons[a]].StackValue++; // And plus the variable responsible for the number of items in the cell 					    break;      // И прерываем цикл
                    }
                    else if (OD.ObjectType > Inv.ObjectsInStore[Inv.Coupons[a]].ObjectType)
                    {
                        if ((a + 1) == Inv.Coupons.Count)                       // If in the list of coupons this last
                        {
                            InsertNewCoupon(a +1);                             // Insert the object in focus at the end of the inventory.
                            Inv.ObjectsInStore[Inv.Coupons[a +1]].StackValue++;    // And plus the variable responsible for the number of items in the cell
                            break;                                              // And break the loop
                        }
                    }
                }             } 			else if(Inv.Coupons.Count == 0)         // Иначе если это первый талон и талонов ещё нет 			{ 				InsertNewCoupon(0);                 // Вызываем метод "Вставить нового купона"                 Inv.ObjectsInStore[Inv.Coupons[0]].StackValue ++; // То мы плюсуем к переменной отвечающей за количество предметов в ячейке 			}             else if(Inv.Coupons.Count == 40)        // Иначе если инвентарь полон
            {
                Debug.LogError("Инвентарь полон");
            }
        } 		else if(OD.StackbleObject == false) // Иначе если объект в фокусе нестакуемый то мы ищем свободное место  		{   // Проводим итерацию по очереди людей с 0 до конца, спрашиваем у каждого талон, идём по нему на склад и там смотрим на тип и номер
            if (Inv.Coupons.Count > 0 && Inv.Coupons.Count < 40)     // Если в очереди уже есть купоны
            {
                for (byte a = 0; a < Inv.Coupons.Count; a++)        // Продолжаем цикл пока не переберём всех купонов в очереди
                {
                    if (OD.ObjectType == Inv.ObjectsInStore[Inv.Coupons[a]].ObjectType) // Если тип объекта в фокусе равен типу объекта "человека в очереди"
                    {
                        if (OD.Queue == Inv.ObjectsInStore[Inv.Coupons[a]].Queue)
                        {
                            InsertNewCoupon(a);             // Call the method "InsertNewCoupon"
                            break;                          // And break the loop
                        }
                        else if (OD.Queue < Inv.ObjectsInStore[Inv.Coupons[a]].Queue)   // Если Queue объекта в фокусе меньше "Queue" "человека в очереди"
                        {
                            InsertNewCoupon(a);             // Call the method "InsertNewCoupon"
                            break;                          // And break the loop
                        }
                        else if (OD.Queue > Inv.ObjectsInStore[Inv.Coupons[a]].Queue)    // Если Queue объекта в фокусе больше "Queue" "человека в очереди"
                        {
                            if ((a + 1) == Inv.Coupons.Count)               // If in the list of coupons this last
                            {
                                InsertNewCoupon(a +1);                      // Insert the object in focus at the end of the inventory.
                                break;                                      // And break the loop
                            }
                        }
                    }
                    else if (OD.ObjectType < Inv.ObjectsInStore[Inv.Coupons[a]].ObjectType)  // Если тип объекта в фокусе меньше типа объекта "человека в очереди"
                    {
                        InsertNewCoupon(a);                               // Вызываем метод "Вставить нового купона"
                        break; // То прерываем цикл
                    }
                    else if (OD.ObjectType > Inv.ObjectsInStore[Inv.Coupons[a]].ObjectType) // Если тип объекта в фокусе больше типа объекта "человека в очереди"
                    {
                        if ((a + 1) == Inv.Coupons.Count)               // If in the list of coupons this last
                        {
                            InsertNewCoupon(a +1);                      // Insert the object in focus at the end of the inventory.
                            break;                                      // And break the loop
                        }
                    }
                }
            }             else if(Inv.Coupons.Count == 0)         // Иначе если это первый талон и талонов ещё нет 			{ 				InsertNewCoupon(0);                 // Вызываем метод "Вставить нового купона" 			}             else if(Inv.Coupons.Count == 40)        // Иначе если инвентарь полон
            {
                Debug.LogError("Инвентарь полон");
            } 		} 	}   	// Этот метод вставляет нового человека в вычесленное место в очереди и присваевает ему место на складе куда и ложит все его данные 	void InsertNewCoupon(int PlaceInLine) 	{ 		int WarehousePlace = FindAndfillWarPlace();		                // Вызываем метод FindWarehousePlace и присваиваем переменной WarehousePlace EmptyPlace 		Inv.Coupons.Insert(PlaceInLine, WarehousePlace); 	            // Вставляем в очередь нового человека с номером места на "Складе" 	} 	  	// "FindAndfillWarehousePlace" Найти и заполнить. Этот метод ищет по именам объектов в массиве NamesObject первое пусто место на "Складе", 	// помещает туда всю информацию об объекте и возвращает в переменную WarehousePlace номер этого места на складе 	int FindAndfillWarPlace() 	{ 		int EmptyPlace = 0; // Эта переменная символизирует пустое место на складе          for (int a = 0; Inv.ObjectsInStore[EmptyPlace] != null ; a++) // Закончим итерацию когда наткнёмся на пустое имя на "Складе"         {             EmptyPlace++;         }          Inv.ObjectsInStore[EmptyPlace] = SOD;           // Ложим на пустое место на "Складе" копию подобранного объекта 		return EmptyPlace;							    // Возвращаем пустое место 	} } 