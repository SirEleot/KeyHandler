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