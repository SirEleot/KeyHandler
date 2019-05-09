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