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