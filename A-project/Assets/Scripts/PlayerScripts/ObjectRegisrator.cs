// Этот скрипт определяет на какой из активных объектов смотрит игрок и по

// Если будет тормозить то Написать четвёртую версию скрипта с целью ускорить работу скрипта с помощью уменьшения количества циклов в каждом шаге
// а также для укорочения скрипта. Сначала перенесём сюда удаление объекта из 3d мира из скрипта отображающего текст и заставить при этом
// нормально всё функционировать. А затем допишу этот скрипт он также будет при удалении объекта переносить информацию об объекте в инвентарь
// Что мы сделаем в четвёртой версии. Вместо того чтобы в каждой итерации проверять по одному параметру то в идеале я буду проводить лишь одну
// итерацию проверяя сразу проходит объект через стенку и так далее и сразу находить среди них focusObject. А потом сравню третюю версию и
// четвёртую на быстродействие с одновременной фильтрацией большой кучи грибов какой окажеться быстрее в тот и буду помещать добавление объекта
// в инвентарь

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectRegisrator : MonoBehaviour 
{
	public float SphereVisionRadius = 5f; 	// Это радиус сферы
	public GameObject Cam;					// Главная камера игрока 
	public GameObject Jaw;					// Кость челюсти игрока
	public GameObject FocusObject;			// Сюда ложиться один единственный объект который отсеялься после всех проверок
	public List<Collider> Objects;			// Создаём список для частично отсеянныйх объектов
	Collider[] Mass;						// Создаём массив всех коллайдеров в зоне сферы

	
	void Update() 
	{
		// Создаём круг его центром являеться текущая позиция а его радиус из переменной SphereVisionRadius. 
		Mass = Physics.OverlapSphere(transform.position, SphereVisionRadius); // Заполняем массив mass объектами в радиусе сферы
		Objects.Clear();	// Очищаем список от всех объектов
		TheFirstStep();		// Выполняем Первый шаг
		TheSecondStep();	// Выполняем Второй шаг
		TheThirdStep();		// Выполняем третий шаг
		if(FocusObject != null)	// Если в FocusObject находиться объект
		Debug.DrawLine(Jaw.transform.position, FocusObject.transform.position);
	}


	// Первый шаг переносим все объекты с тегом ActiveObject в список Objects
	void TheFirstStep()
	{
		for(int a = 0; a < Mass.Length; a++)
		{
			if(Mass[a].tag == "ActiveObject")
			{
				Objects.Add(Mass[a]);
			}
		}
	}


	// Второй шаг обстреливаем лучами все объекты из списка Objects и те что не видны удаляем из списка
	void TheSecondStep()
	{
		RaycastHit HitInfo; // Создаём переменную куда будет возвращаться информация об объекте куда ударилься луч
		int a = 0; 			// Переменная для подсчёта итераций цикла

		while(a < Objects.Count) // Продолжаем цикл до тех пор пока не кончиться список
		{
			// Пускаем луч и возвращаем то во что он ударилься в переменную "HitInfo" (Информация об ударенном объекте)
			Physics.Linecast(Jaw.transform.position, Objects[a].transform.position, out HitInfo);
			if(HitInfo.collider != Objects[a].GetComponent<Collider>())		// Если луч не дошёл до проверяемого объекта
			{
				// Удаляем объект из списка, объект удаляеться и его место занимает другой в итоге получаем пропуск этого объекта
				Objects.RemoveAt(a);
				a--; 	// Чтобы избежать пропуск проверки этого объекта мы делаем шаг назад
			}
			a++;		// А затем шаг вперёд и в итоге заного проверяем тотже объект
		}
	}

	// Выполняем третий шаг по очереди сравниваем все элементы из списка Objects и тот из них кто ближе к центру экрана ложим в переменную
	// FocusObject что означает объект в фокусе
	void TheThirdStep()
	{
		int a = 0; // Переменная для подсчёта итераций цикла
		Collider TemporaryObject = null; // Временная переменная по мере итерации сюда ложиться объект который ближе к центру экрана.
		Vector3 DirPlayer = Cam.transform.forward;		// Луч от камеры игрока, вперёд
		Vector3 DirTarget;								// Луч от обрабатываемого объекта к камере игрока
		while(a < Objects.Count) // Продолжаем цикл до тех пор пока не закончиться AllVisibleObjects или не закончиться массив
		{
			DirTarget = Objects[a].transform.position - Cam.transform.position; // Присваиваем вектору DirTarget значение
			// Если в TemporaryObject не лежит объект то мы сравниваем текущий обрабатываемый объект дот с 0.2f чтобы исключить попадание
			// объектов которые по краям экрана
			if(TemporaryObject == null)
			{
				if(Vector3.Dot(DirPlayer.normalized, DirTarget.normalized)> 0.82f) // Образно говоря если объект ближе заданных краёв экрана
				{
					TemporaryObject = Objects[a];	// То мы помещаем его в TemporaryObject
				}
			}
			if(TemporaryObject != null)	// Если в TemporaryObject уже лежит объект
			{
				// То ложим в DirTempObj вектор направленный от камеры и во временный объект
				Vector3 DirTempObj = TemporaryObject.transform.position - Cam.transform.position; 
				// и сравниваем его с обрабатываемым елементом массива в цикле и если этот элемент ближе к центру экрана чем в TemporaryObject
				if(Vector3.Dot(DirPlayer.normalized, DirTarget.normalized) > Vector3.Dot(DirPlayer.normalized, DirTempObj.normalized))
				{
					TemporaryObject = Objects[a];	// То мы помещаем в TemporaryObject новый объект ктороый ближе к центру экрана
				}
			}						
			a++;	// Увеличиваем "a" на 1
		}
		// И в конце конов если TemporaryObject есть "отсеянный объект" то ложим его в переменную для отсеянного объекта
		if(TemporaryObject != null) 
			FocusObject = TemporaryObject.gameObject;
		else
			FocusObject = null;
	}		
}

