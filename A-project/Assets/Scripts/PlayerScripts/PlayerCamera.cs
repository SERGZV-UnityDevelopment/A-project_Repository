using UnityEngine;

public class PlayerCamera : MonoBehaviour 
{
	public float test;						// Временная тестовая переменная для отслеживания скорости мыши по оси y
	public Transform PlayerMesh;			// Ссылка на коренной объект игрока Его меш								
	public Transform TargetTracking;		// Cсылка на объект слежения пустышку
	public Transform TargetFollow;			// Ссылка на пустышку, объект за которым должна лететь камера 
	public float ScrollSpeed = 2;			// Скорость скрола колёсика мыши
	public float distance = 2f;				// Текущее расстояние камеры
	public float MinDistance = 1f;			// Минимально допустимое расстояние от камеры до игрока
	public float MaxDistance = 5f;			// Максимально допустимое расстояние от камеры до игрока
	public float Xrot = 0f;					// Переменная для отслеживания смещения мыши по оси Y и Превращения во вращение TargetFollow по оси X
	public float YmouseSpeed = 2f;			// Чуствительность мыши по оси Y
	public float Yrot = 0f;					// Переменная для отслеживания вращения камеры по оси Y
	public float SmoothPosCamera = 2;		// Переменная для сглаженного перемещения камеры
	public Inventory Inv;					// Сдесь лежит скрипт Инвентарь


	void Update()
	{
		if(Inv.InventoryOn == false)					// Если инвентарь выключен
		Xrot -= Input.GetAxis("Mouse Y") * YmouseSpeed;			// Накапливаем значение смещения мыши по оси Y умноженную на скорость Yspeed
		Xrot = Mathf.Clamp(Xrot,-88,88);						// Ограничиваем вращение камеры по оси X
		Yrot = PlayerMesh.transform.rotation.eulerAngles.y;		// Вращение переменной Yrot равно вращению игрока
		
		Quaternion TargetFollowRot = Quaternion.Euler(Xrot,Yrot,0);	// Собираем из вышеполученных данных вращение в кватернион TargetFollowRot
		TargetFollow.rotation = TargetFollowRot;					// Присваиваем вращение кватерниона TargetFollowRot вращению TargetFollow
		
		if(Input.GetAxis("Mouse ScrollWheel") !=0 & Inv.InventoryOn == false)	// Если произошёл поворот колеса и интефейс не включен
		{
			// То к переменной дистанции прибавляеться или отнимаеться вращение колеса мыши
			distance -= Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed;
			// Но при этом значение дистанции не может быть больше или меньше прописанных нами параметров
			distance = 	Mathf.Clamp(distance, MinDistance, MaxDistance);	
		}
		// Указываем позицию цели за которой должна будет двигаться камера
		TargetFollow.position = TargetTracking.position - TargetFollow.forward * distance;

		// Тут помещаем камеру на место столкновеня луча с объектом если луч пущенный от цели слежения до 
		// камеры встретит препятствие коллайдер
		RaycastHit hit; // В эту переменную помещаеться информация о столкновении луча с другим объектом
		Vector3 TrueTargetPosition = TargetTracking.position ; // Ложим в TrueTargetPosition текущую позицию цели слежения
		
		// Пускаем луч от камеры до пустышки TargetTracking и возвращаем результат столкновения в переменную hit 
		if (Physics.Linecast(TrueTargetPosition, TargetFollow.position, out hit)) 
		{
			// То мы вычисляем расстояние между Целью слежения и точкой где столкнулься луч и делаем его меньше на 0.80f
			// Чтобы камера была подальше от границы столкновения и не смотрела сквозь полигоны когда ей присвоиться эта позиция
			float tempDistance = Vector3.Distance(TrueTargetPosition, hit.point) -0.80f ;
			
			// Пересчитываем положение TargetFollow используя в качестве расстояния уже не Distance a tempDistance до тех пор пока
			// Происходит столкновение луча о коллайдер
			// Но как только столкновения луча нет то мы снова начинаем брать расстояние от камеры до игрока из переменой distance
			TargetFollow.position = TargetTracking.position -TargetFollow.forward * tempDistance;
		}

	
	}

	
	void FixedUpdate ()
	{
		// Заставляем камеру плавно двигаться за позицией TargetFollow 
		transform.position = Vector3.Lerp(transform.position, TargetFollow.position, SmoothPosCamera * Time.deltaTime);
		// Корректируем вращение камеры чтобы она смотрела на TargetTracking
		transform.LookAt(TargetTracking);
	}


}
