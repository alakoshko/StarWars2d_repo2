using System;
using System.Windows.Forms;

namespace StarWars
{
    /// <summary>Класс программы</summary>
    internal static class Program
    {
        /// <summary>Точка входа в программу</summary>
        [STAThread]
        private static void Main()
        {
            #region Активация стилей оформления пользовательского интерфейса для приложения Win-Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            #endregion


            #region Запуск игры
            //Application.Run(new SplashScreen());

            //var login_window = new LoginWindow();
            //Application.Run(login_window);
            //var user_name = login_window.UserNameTextBox.Text;

            #endregion


            #region Игровая логика
            // Создаём главную форму
            StarWarsForm game_form = new StarWarsForm
            //Form game_form = new Form
            {
                Width = 1024,
                //Width = ,
                Height = 768,
                FormBorderStyle = FormBorderStyle.FixedSingle // Запрещаем ей менять свои размеры
            };

            Game.Init(game_form);  // Инициализация игровой логики
            Game.Load();  // Загрузка данных игровой логики
                          
        

            game_form.Show();      // Показываем форму на экране

            //Game.Draw();         // Отрисовываем кадр

            Application.Run(game_form); // Запускаем процесс обработки очереди сообщений Windows
            #endregion

        }
    }
}
