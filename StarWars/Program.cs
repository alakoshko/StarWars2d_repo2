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

            // Создаём главную форму
            Form game_form = new Form
            {
                Width = 800,
                Height = 600,
                FormBorderStyle = FormBorderStyle.FixedSingle // Запрещаем ей менять свои размеры
            };

            Game.Load();           // Загрузка данных игровой логики
            Game.Init(game_form);  // Инициализация игровой логики

            game_form.Show();      // Показываем форму на экране

            //Game.Draw();         // Отрисовываем кадр

            Application.Run(game_form); // Запускаем процесс обработки очереди сообщений Windows
        }
    }
}
