// Этот скрипт вешаеться на CharecterBasis и выполняет движения повороты и анимацию персонажа
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float test;							// Временная тестовая переменная для отслеживания скорости мыши по оси x
	public float MouseXspeed = 2;				// Скорость вращения персонажа мышкой
	public float RotHorSpeed = 2;				// Скорость вращения персонажа кнопками вертикали
	public float TransformRotHorizontal;   		// Накапливаемое вращение игрока по оси x
	
	public bool DownRotHorizontal = false;		// Зажата ли кнопка вращения по Горизонтали (Q-E)
	public bool DownHorizontal = false;         // Зажата ли кнопка движения по Горизонтали (W-S)
	public bool CapslockActivate = false;		// Активированна ли клавиша CapsLock		
	public bool ShiftDown = false;				// Зажата ли клавиша Shift	
	public bool Run = false;					// Игрок бежит?								
	public Animator __animator;					// Сюда ложим аниматор при старте
	public float __Walk_FB;						// Скорость движения вперёд     
	private float __WalkSide;					// Скорость движения в стороны
	public float SpeedRun = 0;					// Меняющаяся скорость бега для плавного перехода между шагом и бегом
													
	public GameObject PlayerCam;				// Сдесь лежит игровой объект PlayerCamera
	public Inventory Inv;						// Сдесь лежит скрипт Инвентарь

	
	void Start () 
	{
		__animator = GetComponent<Animator>(); 	// Поместить в переменную аниматор компонент аниматор.
		Inv = PlayerCam.GetComponent<Inventory>();	// Ложим скрипт в переменную InvScr
	}


	void Update () 
	{
		#region переключение состояний анимаций шаг, бег, красться, ползти.

		if(Input.GetKeyDown(KeyCode.CapsLock))	// Если была нажата кнопка CapsLock
			// И если CaplockActive ложь то делаем его правда, а если СaplockActive правда то делаем его ложь
			CapslockActivate = CapslockActivate == false ? CapslockActivate = true : CapslockActivate = false;
								
		// Пока зажата кнопка левый шифт ShiftDown равна правда, а если кнопка левый шифт отпущенна то ShiftDown равно ложь
		ShiftDown = Input.GetKey(KeyCode.LeftShift) ? ShiftDown = true : ShiftDown = false;

		if(CapslockActivate == false)								// Если CaplockActive равна ложь тогда мы спрашиваем
			Run = ShiftDown == false ? Run = false : Run = true;	// ShiftDown ложь ? то бег ложь, а если ShiftDown правда то и бег правда
		else 														// Однако если CaplockActive равно правда тогда мы спрашиваем
			Run = ShiftDown == false ? Run = true : Run = false;	// ShiftDown равна ложь ? то бег правда, а если ShiftDown ложь то и бег правда

		#endregion

		#region Ограничиваем проигрыш определённых анимаций в определённых условиях

		// Когда будет готова анимация в бока сделать ограничения и для неё

		// Если зажата кнопка горизонтали (A-D) то(DownHorizontal) равно правда, иначе (DownHorizontal) равно ложь.
		DownHorizontal = Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D)? DownHorizontal = true : DownHorizontal = false;

		// Если (A-D)зажаты одновременно то не проигрывать анимации в бока(Нельзя возвращать значение для __WalkSide),
		// а если зажата только одна, то можно проигрывать анимации в бок(То можно возвращать значение в __WalkSide).
		__WalkSide = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) ? __WalkSide = 0 : __WalkSide = Input.GetAxis("Horizontal");

		if(Inv.InventoryOn == false)// Если инвентарь выключен то мы разрешаем выполнять код ниже
		{
			// Если кнопки горизонтали не нажаты То можно проигрывать анимации вперёд и назад, а если нажаты то проигрывать их нельзя
			// А также сдесь мы возвращаем значение в переменную __Walk_FB
		__Walk_FB = DownHorizontal == false ? __Walk_FB = Input.GetAxis("Vertical") : __Walk_FB = 0;
		}
		else // Но если инвентарь включен
		{
			SpeedRun = 0;
			__Walk_FB = SmoothDampingAnimation(__Walk_FB);	 // Посылаем скорость __Walk_FB в метод занимающийся сглаживанием скорости
			__WalkSide = SmoothDampingAnimation(__WalkSide); // Посылаем скорость __WalkSide в метод занимающийся сглаживанием скорости
		}

		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))	// Если обе кнопки по вертикали зажаты
			__Walk_FB = 0;										// То проигрывать анимации по вертикали нельзя

		#endregion

		#region Указываем при каких условиях какую анимацию будем проигрывать
		if(Inv.InventoryOn == false)
		{
		//Указываем какую скорость анимации нам установить по вертикали и горизонтали
		//Если __Walk_FB больше 0 то прибавляем SpeedRun, а если меньше 0 то вычитаем SpeedRun иначе скорость равна 0.
		__Walk_FB += __Walk_FB > 0 ? SpeedRun : __Walk_FB < 0 ? - SpeedRun : 0;
		//Если __WalkSide больше 0 то прибавляем SpeedRun, а если меньше то отнимаем SpeedRun иначе скорость равна 0.
		__WalkSide += __WalkSide > 0 ? SpeedRun : __WalkSide < 0 ? - SpeedRun : 0;
		}
		#endregion

		#region Физичесское вращение персонажа
	}


	void FixedUpdate()
	{
		// Если зажата кнопка вращения горизонтали (Q или E) ,то переменная DownRotHorizontal = правда,  Если же не зажата то = ложь
		DownRotHorizontal = Input.GetKey(KeyCode.Q) | Input.GetKey(KeyCode.E) ? DownRotHorizontal = true : DownRotHorizontal = false;
		
		// Если DownRotHorizontal равно ложь и InterfaceScr.InventoryOn равно ложь
		if(DownRotHorizontal == false & Inv.InventoryOn == false)
			// Переменная TransformRotHorizontal накапливает отклонение мыши по оси x умноженное на MouseXspeed в постоянное число
			TransformRotHorizontal += (Input.GetAxis("Mouse X") * MouseXspeed);

		if(Input.GetKey(KeyCode.E))      			// Если кнопка E зажата 
			TransformRotHorizontal += RotHorSpeed ; // То Переменная TransformRotationHorizontal прибавляет RotHorSpeed
		
		if(Input.GetKey(KeyCode.Q))					// Если кнопка Q зажата 
			TransformRotHorizontal -= RotHorSpeed ;	// То Переменная TransformRotationHorizontal отнимает RotHorSpeed

		
		// Тут мы не даём переменной TransformRotHorizontal быть выше 360 градусов и меньше 0 градусов
		TransformRotHorizontal = Mathf.LerpAngle(0,TransformRotHorizontal,360);

		if(Inv.InventoryOn == false)	// Только если инвентарь выключен то мы разрешаем персонажу вращаться, иначе нет
		// И тут мы приравниваем обработанный угл переменной TransformRotationHorizontal игроку по оси y делая возможным его поворот мышью
		transform.rotation = Quaternion.AngleAxis(TransformRotHorizontal, Vector3.up);

		#endregion

		#region Плавный переход от шага к бегу и от бега к шагу
		// (а когда персонаж идёт скорость бега равна 0)
		if(Inv.InventoryOn == false)	// Если выключен инвентарь
		{
			if (Run == true & SpeedRun < 1)				// Если активирован режим бега неважно шифтом или CapsLock'ом а SpeedRun при этом меньше 1,
					SpeedRun += Time.deltaTime * 3;		// Тогда мы делаем плавный переход от ходьбы к бегу
			if (Run == false & SpeedRun > 0)			// Если режим бега не активирован а SpeedRun больше 0
					SpeedRun -= Time.deltaTime * 3;		// То мы делаем плавный переход от бега к ходьбе
			SpeedRun = Mathf.Clamp01(SpeedRun);			// Сдесь мы зажимаем значение SpeedRun чтобы оно было не больше 1 и не меньше 0
		}

		// Присваиваем переменным в аниматоре обработанные значения
		__animator.SetFloat("Walk_FB", __Walk_FB);
		__animator.SetFloat("WalkSide", __WalkSide);
		#endregion
	}

	#region сглаженная остановка анимации при включении меню или инвентаря
	// Это метод который сглаживает остановку персонажа если инвентарь включен
	 float SmoothDampingAnimation(float I)
	{
		if(I > 0 )								// Если число пришедшее сюда больше 0			
		{
			I -= Time.deltaTime * 4;			// Тогда мы отнимаем у него чучуть
			if(I < 0)							// Если мы отняли и вышли в минус ноль
				I = 0;							// То мы ставим переменной ноль
			return I;							// И возвращаем это значение обратно в шапку метода откуда его забирает вызывающая строка
		}
		if(I < 0)								// Если число пришедшее сюда меньше 0
		{
			I += Time.deltaTime * 4;			// Тогда мы прибавляем к нему чучуть
			if(I > 0)							// Если мы отняли и вышли в плюс ноль
				I = 0;							// Тогда назначаем переменной ноль
			return I;							// И возвращаем это значение обратно в шапку метода откуда его забирает вызывающая строка
		}
		else 									// Если число пришедшее сюда равно нулю тогда
		{
			return I;							// Возвращаем его не обработав
		}
	}
	#endregion
}

