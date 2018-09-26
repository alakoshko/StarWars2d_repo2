using System;
using System.Drawing;
using System.Windows.Forms;

namespace StarWars
{
    /// <summary>Класс игровой логики</summary>
    internal static class Game
    {
        /// <summary>Конекст буфера отрисовки графики</summary>
        private static BufferedGraphicsContext __Context;

        /// <summary>Таймер обновления игрового интерфейса</summary>
        private static readonly Timer __Timer = new Timer { Interval = 100 };

        /// <summary>Массив графических игровых объекотв</summary>
        private static GameObject[] __GameObjects;

        private static Asteroid[] __Asteroids;

        private static Bullet __Bullet;

        
        /// <summary>Буфер, в который будем проводить отрисовку графики очередного кадра</summary>
        public static BufferedGraphics Buffer { get; private set; }

        /// <summary>Ширина игрового поля</summary>
        public static int Width { get; private set; }
        /// <summary>Высота игрового поля</summary>
        public static int Height { get; private set; }

        


        /*static Button btnNew;
        static Button btnRecords;
        static Button btnExit;*/

        /// <summary>Загрузка данных игровой логики</summary>
        public static void Load(StarWarsForm form)
        {
            Width = form.Width;
            Height = form.Height;

            __GameObjects = new GameObject[30];

            
            var rnd = new Random();

            //Звезды
            for (var i = 0; i < __GameObjects.Length; i++)
            {
                int r = rnd.Next(5, 50);
                var size = rnd.Next(10, 30);
                __GameObjects[i] = new Star(
                    new BaseObjectParams
                    {
                        Position = new Point(Width, (rnd.Next(0, Height) ) ),
                        Speed = new Point(rnd.Next(0, i), 0),
                        Size = new Size(size, size)
                    },
                    rnd.Next(0,5));
            }

            //Астероиды
            const int asteroids_count = 10;
            __Asteroids = new Asteroid[asteroids_count];

            for (var i = 0; i < asteroids_count; i++)
            {
                var speed = rnd.Next(3, 10);

                var Power = rnd.Next(Asteroid.powerMin, Asteroid.powerMax);
                
                __Asteroids[i] = new Asteroid(
                    new BaseObjectParams
                    {
                        Position = new Point(rnd.Next(0, Width), rnd.Next(0, Height)),
                        Speed = new Point(speed, (rnd.Next(1, speed)%2 == 0) ? -rnd.Next(1, speed): rnd.Next(1, speed)),
                        //параметр зависит от Power
                        Size = new Size(0, 0)
                    },
                    Power
                );
            }


            //Пули
            const int BulletPower = 1;

            __Bullet = new Bullet(
                new BaseObjectParams
                {
                    Position = new Point(0, 200),
                    Speed = new Point(3, 0),
                    Size = new Size(4, 1)
                },
                BulletPower);
        }

        /// <summary>Инициализация игровой логики</summary>
        /// <param name="form">Игровая форма</param>
        public static void Init(StarWarsForm form)
        {
            Width = form.Width;
            Height = form.Height;

            __Context = BufferedGraphicsManager.Current;

            var graphics = form.CreateGraphics();
            Buffer = __Context.Allocate(graphics, new Rectangle(0, 0, Width, Height));

            __Timer.Tick += OnTimerTick;
            __Timer.Enabled = true;

        }

        /// <summary>Метод, вызываемвый таймером всякий раз при истечении указанного интервала времени</summary>
        private static void OnTimerTick(object Sender, EventArgs e)
        {
            Update();
            Draw();
        }

        /// <summary>Метод отрисовки очередного кадра игры</summary>
        public static void Draw()
        {
            var g = Buffer.Graphics; // Извлекаем графический контекст для рисования
            g.Clear(Color.Black);    // Заливаем всю поверхность одним цветом (чёрным)

            #region Пример рисования примитивов для проверки процесса создания игровой сцены
            //g.DrawRectangle(Pens.White, 100, 100, 200, 200);  // Рисуем прямоугольник
            //g.FillEllipse(Brushes.Red, 100, 100, 200, 200);   // Заливаем эллипс
            #endregion

            // Пробегаемся по всем графическим объектам и вызываем у каждого метод отрисовки
            foreach (var game_object in __GameObjects)
                game_object.Draw();

            //рисуем астероиды
            foreach (var asteroid_obj in __Asteroids)
                asteroid_obj.Draw();

            __Bullet.Draw();

            Buffer.Render(); // Переносим содержимое буфера на экран
        }

        /// <summary>Метод обновления состояния игры между кадрами</summary>
        private static void Update()
        {
            // Пробегаемся по всем игровым объектам
            foreach (var game_object in __GameObjects)
                game_object.Update(); // И вызываем у каждого метод обновления состояния

            //Запустили пулю
            __Bullet.Update();

            Random rnd = new Random();

            //Двигаем астероиды
            for (var i = 0; i < __Asteroids.Length; i++)
            {
                if (__Asteroids[i].Collision(__Bullet))
                {
                    __Asteroids[i].Damage += __Bullet.Power();
                    //__Bullet = null;
                }

                if (__Asteroids[i].Power <= 0)
                {
                    var speed = rnd.Next(3, 10);
                    var Power = rnd.Next(Asteroid.powerMin, Asteroid.powerMax);

                    __Asteroids[i] = new Asteroid(
                        new BaseObjectParams
                        {
                            Position = new Point(rnd.Next(0, Width), rnd.Next(0, Height)),
                            Speed = new Point(speed, (rnd.Next(1, speed) % 2 == 0) ? -rnd.Next(1, speed) : rnd.Next(1, speed)),
                            //параметр зависит от Power
                            Size = new Size(0, 0)
                        },
                        Power
                    );
                }
                __Asteroids[i].Update();
            }
            
        }
    }
}
