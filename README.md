# KeyHandler
Всем привет!

Рассмотрим пример как сделать свой  простенький KeyHandler Для этого давайте создадим папку Inputs и в ней два файла.

Можно скачать по ссылке на Git

Создадим класс для кнопки:

```
namespace YourNamespace.Inputs
{   
  //создадим список enum с кодами кнопок
  enum KeyCodes
    {
        released = -1,
        F1 = 112, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
        Key_0 = 48, Key_1, Key_2, Key_3, Key_4, Key_5, Key_6, Key_7, Key_8, Key_9,
        A = 65, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    } 
  	//объявим делегат для функций обратного вызова
    public delegate void KeyActions();
  	//Сам класс
    class KeyModel
    {
      	//тут будет храниться код кнопки из нашего списка 
        public KeyCodes KeyCode;
      	//тут функция которая будет выполнятся по нажатии кнопки
        public KeyActions OnPress;
      	 //тут функция которая будет выполнятся при отпускании кнопки
        public KeyActions OnRelease;
      	//объявим конструктор класса
        public KeyModel(KeyCodes keyCode, KeyActions onPress, KeyActions onRelease)
        {
            KeyCode = keyCode;
            OnPress = onPress;
            OnRelease = onRelease;
        }  
    }
}
```

И непосредственно сам обработчик событий

```
using RAGE;
using System.Collections.Generic;

namespace YourNamespace.Inputs
{
   	//сам класс унаследованный от RAGE.Events.Script
    class Key: Events.Script
    {
      	//конструктор класса 
        Key()
        {
          	//подпишемся на событие Tick для вызова обработчика кждый кадр
            Events.Tick += Handler;
        }
      	//объявим некоторые переменные 
      	//тут создадим экземпляр ненажатой кнопки
        private static KeyModel Releazed = new KeyModel(KeyCodes.released, null, null);
      	//тут будем хранить текущее состояние кнопок, на начальном этапе присвоим ссылку на Releazed(Ни одна из кнопок не нажата)
        private static KeyModel Pressed = Releazed;
      	//создадим список где будем хранить все наши конфигурации кнопок
        private static List<KeyModel> InputList = new List<KeyModel>();
		
      	//сам метод обработчика, вызывается каждый кадр
        private static void Handler(List<Events.TickNametagData> nametags)
        {
          	//проверяем не занят ли обработчик другим действием, если нет вызываем метод  CheckPressed
          	if (Pressed.KeyCode == KeyCodes.released) CheckPressed();
          	//если он занят в данный момент - ждем когда будет отпущена предыдущая кнопка
            else if (!Input.IsDown((int)Pressed.KeyCode))
            {
              	//проверяем назначен ли метод на событие отпускания кнопки, если есть - выполняем
                if (Pressed.OnRelease != null)  Pressed.OnRelease.Invoke();
              	//сообщаем обработчику что он свободен
                Pressed = Releazed;
            };           
        }
		
      	//метод для проверки нажата ли какая либо клавиша из списка 
        private static void CheckPressed()
        {
          	//проходим по всему списку
            InputList.ForEach(i =>
            {
              	//если нажата какая-то клавиша то
                if (Input.IsDown((int)i.KeyCode))
                {
                  	//назначаем ее обработчику сообщая ему что он занят в данный момент
                    Pressed = i;
                  	//проверяем назначен ли метод на событие нажатия кнопки, если есть - выполняем
                    if (i.OnPress != null) i.OnPress.Invoke();
                  	//выходим из метода
                    return;
                }
            });           
        }
		//метод для добавления кнопки в список принимает теже параметры что и класс KeyModel
        public static void Bind(KeyCodes keyCode, KeyActions onPress, KeyActions onRelease = null) {
          	//проверяем не назначены ли уже события для эой кнопки 
            if (InputList.Exists(i => i.KeyCode == keyCode))
            {
              	//если назначены получаем объект данной кнопки
                KeyModel Input = InputList.Find(i => i.KeyCode == keyCode);
              	//и подписываемся на его события
                Input.OnPress += onPress;
                Input.OnRelease += onRelease;
            }
          	//если нет то просто создаем новый объект
            else InputList.Add(new KeyModel(keyCode, onPress, onRelease));
        }
      	// метод удаления кнопки из списка
        public static void Unbind(KeyCodes keyCode)
        {
           	//тут просто находим объект с данным кодом в списке и удаляем
            if (InputList.Exists(i => i.KeyCode == keyCode))
            {
                KeyModel Input = InputList.Find(i => i.KeyCode == keyCode);
                InputList.Remove(Input);
            }
        }
    }
}
```

Ну и конечно же пример добавляем или удаляем кнопку из списка при помощи методов Key.Bind() и Key.Unbind() предварительно подключив пространство имен YourNamespace.Inputs

```
using RAGE;
using YourNamespace.Inputs;

namespace YourNamespace
{
  	//к примеру при старте клиента
    class ResourceStart: Events.Script
    {
		public ResourceStart(){
            //назначим обработчик для кнопки Е с методами при нажатии (OnKeyPress) и отпуске(OnKeyRelease) кнопки
        	Key.Bind(KeyCodes.E, OnKeyPress, OnKeyRelease);
        }
      
      	//это метод который будет вызван при нажатии
      	public static void OnKeyPress(){
           Chat.Output("check on click");
        }
      	//это метод который будет вызван при отпускании
      	public static void OnKeyRelease(){
          	Chat.Output("check on release");
        }
    }
}
```

Надеюсь кому-то было полезным всем пока
