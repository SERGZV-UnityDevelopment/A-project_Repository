// * Этот скрипт вешаеться на CharecterBasis и выполняет роль отрисовки инвентаря игрока // * Coupons "Талоны или купоны" Я назвал этот список так потому что номера в этом списке похожи на людей в очереди с талонами У каждого человека есть "талон" // Номер подбираемого объекта на "складе" инвентаря. Как бы талон на получение контенера с вещами на складе, а номер этого человека в очереди помогает сортировать их по категориям. using UnityEngine; using System.Collections.Generic; using UnityEngine.UI;  public class Inventory : MonoBehaviour {     public Sprite[] Images;                     // Changing button images     public GameObject[] panels;                 // Array of UI Elements     public GameObject InventoryPanel;           // The main window of the inventory is stored here.     public Texture2D line, Backline;            // Текстура полоски состояния предметов в инвентаре и фоновая полоска     const int AS = 40;                          // Amount SlotsМаксимальное количество слотов в инвентаре     public ObjectRegisrator OR;                 // Сдесь лежит скрипт ObjectRegistrator     public PlayerControllerTwo PC;              // Сдесь лежит скрипт PlayerController     public PlayerCamera PlCam;                  // Сдесь лежит скрипт PlayerCamera     public bool InventoryOn = false;            // включен ли инвентарь, true говорит о том что включен     public Transform CharacterBasis;            // Сдесь лежит трансформация пустышки игрока CharacterBasis     public int ActiveCategory = 0;              // Номер категори который сейчас выбран     public List<int> InventoryCells;                                    // По номеру ячейки мы находим номер "Талона в очереди"     public List<int> Coupons;                                           // Объявляем список "Талоны" (По ним мы находим номер объекта на складе инвентаря игрока)     public StoreObjectData[] ObjectsInStore = new StoreObjectData[40];  // Объекты на складе (Заменяет разом все переменные снизу в этой категории) //-----------------------------------------------------------------------------------------------------------------------------------------------      public int ActiveСellNumber = 0;                                    // Active cell number (На замену всем номерам будет один единый номер, категории переключаются номер остаётся)      void Start()     {         Cursor.visible = false;                     // Hide the cursor         Cursor.lockState = CursorLockMode.Locked;   // Lock cursor     }       void Update()     {         if (Input.GetKeyDown(KeyCode.Tab) && InventoryPanel.activeSelf == true)       // If the inventory call button was pressed and the inventory was turned off         {             InventoryPanel.SetActive(false);            // Activate the inventory panel             panels[2].gameObject.SetActive(false);      // Turn off the object information panel             PlCam.enabled = true;                       // Turn on the player’s camera control script             PC.enabled = true;                          // Turn on the player control script             OR.enabled = true;                          // Включаем скрипт регестрации объектов             Cursor.visible = false;                     // Hide the cursor             Cursor.lockState = CursorLockMode.Locked;   // Lock cursor                      }         else if (Input.GetKeyDown(KeyCode.Tab) && InventoryPanel.activeSelf == false) // Otherwise, if the call button for the inventory was pressed and the inventory was turned on         {
            PC.__animator.SetFloat("Walk_FB", 0);       // Set the animation forward/back to 0 (A rough but quick option)
            PC.__animator.SetFloat("WalkSide", 0);      // Set the animation right/left to 0 (A rough but quick option)             InventoryPanel.SetActive(true);             // Deactivate the inventory panel             UpdateInformationWindow();                  // Determine whether to enable the object information window             PlCam.enabled = false;                      // Turn off the player’s camera control script             PC.enabled = false;                         // Turn off the player control script             OR.enabled = false;                         // То мы выключаем скрипт регестрирования объектов             Cursor.visible = true;                      // Make the cursor visible.             Cursor.lockState = CursorLockMode.None;     // Unlock the cursor             OR.FocusObject = null;                      // Resetting the object in focus
        }     }           void ChangeActiveCellNomber(int NewNomber)   // This method changes the selected inventory cell.     {         panels[1].transform.GetChild(ActiveСellNumber).GetComponent<Image>().sprite = Images[2];    // Assign the previous cell a standard texture         panels[1].transform.GetChild(NewNomber).GetComponent<Image>().sprite = Images[3];           // Assign the appropriate texture to the selected cell.         ActiveСellNumber = NewNomber;                                                               // Change the number of the active cell         FillCategory((Type)ActiveCategory);                                                         // Call the update method of the current category     }           void UpdateInformationWindow()                      // This method decides whether to turn on the information window, turn it off, or update it.
    {
        // If inventory mode is enabled and the number of the active cell is less to the length of the list of coupons 
        if (InventoryPanel.activeSelf == true && ActiveСellNumber < InventoryCells.Count)            
        {
            panels[2].gameObject.SetActive(true);       // То выключаем окно информации

            panels[2].transform.GetChild(0).GetComponent<Text>().text = ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].ObjectName;    // Update the object name in the object information window              if (ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].Description != "")                                                     // If the object has a description                 panels[2].transform.GetChild(1).GetChild(0).GetComponent<Text>().text =                     ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].Description;                                                       // We update the description of the object in the information window
            else                                                                                                                        // If the object has no description                 panels[2].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Описание отсутствует";                         // We write that the description is missing              if (ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].ObjectTexture != null) // Если на складе инвентаря( Слот склада равен выбранной ячейки ) есть текстура                 panels[2].transform.GetChild(2).GetComponent<Image>().sprite = ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].ObjectTexture; // Draw a texture from a warehouse cell                
        }         else                                            // Иначе             panels[2].gameObject.SetActive(false);      // Отрисовывем окно информации     }       public void FillCategory(Type type)                             // This method is used to visually update the inventory for the selected category.
    {         InventoryCells.Clear();                                     // We clear the list to fill it correctly again         for (int a = 0; a < Coupons.Count; a++)
        {
            if (ActiveCategory == 0)                                // If we need to fill out the "All" category
            {
                InventoryCells.Add(a);                              // Pass the value of the selected coupon to the next cell
            }
            else                                                    // If we need to fill out some specific category
            {
                if (ObjectsInStore[Coupons[a]].ObjectType == type)  // If the object type of this "coupon" in the "warehouse" is equal to what we need
                {
                    InventoryCells.Add(a);                          // Pass the value of the selected coupon to the next cell
                }
            }
        }          for (int a = 0; a < 40; a++) // Продолжаем цикл до тех пор пока i больше чем длинна списка людей в очереди
        {             if (InventoryCells.Count > a)       // If an object should be drawn in this cell
            {
                panels[1].transform.GetChild(a).transform.GetChild(0).GetComponent<Image>().enabled = true;                                          // Turn on the next cell texture in the loop                 panels[1].transform.GetChild(a).transform.GetChild(0).GetComponent<Image>().sprite =
                    ObjectsInStore[Coupons[InventoryCells[a]]].ObjectTexture;   // Assign the next cell in the loop its corresponding texture

                if (ObjectsInStore[Coupons[InventoryCells[a]]].StackValue > 1)           // If the value of the number of items in the cell in the warehouse is more than one
                {
                    panels[1].transform.GetChild(a).transform.GetChild(1).GetComponent<Text>().enabled = true;                                                  // Enable the text of the number of items
                    panels[1].transform.GetChild(a).transform.GetChild(1).GetComponent<Text>().text = ObjectsInStore[Coupons[InventoryCells[a]]].StackValue.ToString();  // We indicate in the text the number of items
                }
                else // Otherwise, if less than or equal to one
                {
                    panels[1].transform.GetChild(a).transform.GetChild(1).GetComponent<Text>().enabled = false;                                         // Disable the text of the number of items
                }

                // Если объект в ячейке на "Складе" разрушаемый а его значение целостности менее 100%
                if (ObjectsInStore[Coupons[InventoryCells[a]]].DestroyableObject == true && ObjectsInStore[Coupons[InventoryCells[a]]].StateObject < 100)
                {  // То мы отрисовываем полоску состояния предмета для этой ячейки
                    panels[1].transform.GetChild(a).transform.GetChild(2).GetComponent<Image>().enabled = true;     // Activate the item status bar back image
                    panels[1].transform.GetChild(a).transform.GetChild(3).GetComponent<Image>().enabled = true;     // Activate the item status bar image
                    panels[1].transform.GetChild(a).transform.GetChild(3).GetComponent<Image>().color
                        = Color.Lerp(Color.red, Color.green, ObjectsInStore[Coupons[InventoryCells[a]]].StateObject / 100);         // Determine the color of the strip of item status

                    // 0.92 is one division of a whole line 92 units wide
                    // Set the size of the item status line
                    panels[1].transform.GetChild(a).transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(0.92f * ObjectsInStore[Coupons[InventoryCells[a]]].StateObject, 10);
                }
                else // Otherwise, if this object is not destructible, or with full lives
                {
                    panels[1].transform.GetChild(a).transform.GetChild(2).GetComponent<Image>().enabled = false;    // Deactivate the item status bar back image
                    panels[1].transform.GetChild(a).transform.GetChild(3).GetComponent<Image>().enabled = false;    // Deactivate the item status bar image
                }
            }             else                                // Otherwise, if we iterate over empty cells for this category
            {
                panels[1].transform.GetChild(a).transform.GetChild(0).GetComponent<Image>().enabled = false;    // Turn off the next cell texture in the loop
                panels[1].transform.GetChild(a).transform.GetChild(1).GetComponent<Text>().enabled = false;     // Disable the text of the number of items
                panels[1].transform.GetChild(a).transform.GetChild(2).GetComponent<Image>().enabled = false;    // Deactivate the item status bar back image
                panels[1].transform.GetChild(a).transform.GetChild(3).GetComponent<Image>().enabled = false;    // Deactivate the item status bar image
            }         }

        UpdateInformationWindow(); // We call the method that updates the information and the state of the information window.
    }

 
    public void ChageCategoryInventory(int CategoryNumber)  // This method switches categories in inventory
    {         panels[0].transform.GetChild(ActiveCategory).GetComponent<Image>().sprite = Images[0];  // Change the texture to not selected from the previous selected button         panels[0].transform.GetChild(CategoryNumber).GetComponent<Image>().sprite = Images[1];  // At the selected button we put the texture of the selected button         ActiveCategory = CategoryNumber;                                                        // Specify the number of the newly selected category          switch (CategoryNumber)         {             case 0:                     FillCategory(Type.undefined);     // We fill the inventory with items of the "Weapon" category                     break;             case 1:
                    FillCategory(Type.Weapon);       // We fill the inventory with items of the "Weapon" category                     break;             case 2:                                     FillCategory(Type.Armor);        // We fill the inventory with items of the "Armor" category                     break;             case 3:                     FillCategory(Type.Food);         // We fill the inventory with items of the "Food" category                     break;             case 4:                     FillCategory(Type.Magic);        // We fill the inventory with items of the "Magic" category                     break;             case 5:                     FillCategory(Type.Construction); // We fill the inventory with items of the "Construction" category                     break;             case 6:                     FillCategory(Type.Other);        // We fill the inventory with items of the "Other" category                     break;         }     }


    void UseObject()     {      }  
    void DropObject()                   // This method is called when we need to throw the selected object
    {
        if (ActiveСellNumber < InventoryCells.Count)                            // If a non-empty inventory cell is selected
        {
            CreateObject();            // Вызываем функцию создать объект

            if (ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].StackValue > 1)   // If there are several of these objects in the storage cell
                ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].StackValue--;    // Reduce the value of the number of objects in this cell in the warehouse by one
            else                                                                            // Otherwise, if there was only one such object in the warehouse
            {
                ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]] = null;           // Otherwise, delete the object from the warehouse.
                Coupons.RemoveAt(InventoryCells[ActiveСellNumber]);                         // Delete a coupon from the list of coupons
                InventoryCells.RemoveAt(ActiveСellNumber);                                  // Remove the corresponding item from the Inventory Cells list.
            }

            FillCategory((Type)ActiveCategory); // Call the update method of the current category
        }
    }       // Этот метод создаёт объект при выбрасывании. Вызываеться из метода DropObject     // Передаём сюда номер купона у которого мы спрашиваем номер склада откуда возьмём все данные об объекте     void CreateObject()     {          // Create a discarded object in front of the player         GameObject drop = Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath(ObjectsInStore[Coupons[InventoryCells[ActiveСellNumber]]].Path, typeof(GameObject))) as GameObject;         drop.transform.position = CharacterBasis.transform.position + CharacterBasis.transform.forward + CharacterBasis.transform.up; // Set him a position
    }      // -------------------------------------------------------------------------------------------- Button Metods ----------------------------------------------------------------------------------------      public void ChageCategoryInventoryButton(int CategoryNumber)    // Method called by buttons to switch inventory categories     {         ChageCategoryInventory(CategoryNumber);                     // We call the handler method     }       public void ChangeActiveCellNomberButton(int NewNomber)         // This method is called by clicking on one of the cell buttons.     {         ChangeActiveCellNomber(NewNomber);                          // We call the handler method     }       public void UseObjectButton()                                   // This method is called by the "Use Object" button.     {      }       public void DropObjectButton()                                  // This method is called by the "Drop Object" button.     {         DropObject();                                               // We call the method of throwing an object
    } } 