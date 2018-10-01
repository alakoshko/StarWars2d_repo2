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
        public static readonly Timer __Timer = new Timer { Interval = 100 };

        /// <summary>Массив графических игровых объекотв</summary>
        private static GameObject[] __GameObjects;

        private static Asteroid[] __Asteroids;

        //private static Bullet __BulletCenter, __BulletLeft, __BulletRight;
        private static Bullet[] __Bullets = new Bullet[3];

        private static Ship __Ship;

        /// <summary>Буфер, в который будем проводить отрисовку графики очередного кадра</summary>
        public static BufferedGraphics Buffer { get; private set; }

        /// <summary>Ширина игрового поля</summary>
        public static int Width { get; private set; }
        /// <summary>Высота игрового поля</summary>
        public static int Height { get; private set; }

        private const int BulletPower = 1;

        /// <summary>Загрузка данных игровой логики</summary>
        public static void Load()
        {
            __GameObjects = new GameObject[30];

            var rnd = new Random();

            //Звезды
            #region
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
            #endregion

            //Астероиды
            #region
            const int asteroids_count = 10;
            __Asteroids = new Asteroid[asteroids_count];

            for (var i = 0; i < asteroids_count; i++)
            {
                var speed = rnd.Next(3, 10);

                var Power = rnd.Next(Asteroid.powerMin, Asteroid.powerMax);
                
                __Asteroids[i] = new Asteroid(
                    new BaseObjectParams
                    {
                        Position = new Point(rnd.Next(Width/2, Width), rnd.Next(0, Height)),
                        Speed = new Point(speed, (rnd.Next(1, speed)%2 == 0) ? -rnd.Next(1, speed): rnd.Next(1, speed))
                    },
                    Power
                );
            }
            #endregion

            

            //Космический корабль
            #region
            __Ship = new Ship(
                new BaseObjectParams
                {
                    Position = new Point(10, 400),
                    Speed = new Point(5, 5),
                    Size = new Size(150, 150)
                } );

            __Ship.ShipDie += OnShipDie;
            #endregion
        }

        private static void OnShipDie()
        {
            __Timer.Enabled = false;
            __Ship = null;
            var g = Buffer.Graphics;
            g.Clear(Color.Black);
            g.DrawString("Вы проиграли!", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }

        /// <summary>Инициализация игровой логики</summary>
        /// <param name="form">Игровая форма</param>
        //public static void Init(StarWarsForm form)
        public static void Init(StarWarsForm form)
        {
            Width = form.Width;
            Height = form.Height;
            form.KeyDown += OnGameFormKeyPress;

            __Context = BufferedGraphicsManager.Current;

            //var graphics = form.CreateGraphics();
            var graphics = form.CreateGameGraphicObject();
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

        public static void OnGameFormKeyPress(object Sender, KeyEventArgs args)
        {
            if (__Timer.Enabled)
            {
                switch (args.KeyCode)
                {
                    case Keys.ControlKey:
                        //var ship_location = __Ship.Rect.Location;
                        __Bullets[0] = new Bullet( new BaseObjectParams { Position = new Point(__Ship.CenterWeapon.X, __Ship.CenterWeapon.Y) }, BulletPower);
                        __Bullets[1] = new Bullet(new BaseObjectParams { Position = new Point(__Ship.LeftWeapon.X, __Ship.LeftWeapon.Y) }, BulletPower);
                        __Bullets[2] = new Bullet(new BaseObjectParams { Position = new Point(__Ship.RightWeapon.X, __Ship.RightWeapon.Y) }, BulletPower);
                        break;
                    case Keys.Up:
                        __Ship?.Up();
                        break;
                    case Keys.Down:
                        __Ship?.Down();
                        break;
                    case Keys.W:
                        __Ship?.Die();
                        break;
                }
            }
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
            
            //рисуем пули
            for (int iBullet = 0; iBullet < __Bullets.Length; iBullet++)
                __Bullets[iBullet]?.Draw();

            //корабль
            __Ship?.Draw(); // if(__Ship != null) __Ship.Draw();

            Buffer.Render(); // Переносим содержимое буфера на экран
        }

        /// <summary>Метод обновления состояния игры между кадрами</summary>
        private static void Update()
        {
            // Пробегаемся по всем игровым объектам
            foreach (var game_object in __GameObjects)
                game_object.Update(); // И вызываем у каждого метод обновления состояния

            //Запустили пули
            for (int iBullet = 0; iBullet < __Bullets.Length; iBullet++)
                __Bullets[iBullet]?.Update();
           

                Random rnd = new Random();

            //Двигаем астероиды
            for (var i = 0; i < __Asteroids.Length; i++)
            {
                for (int iBullet = 0; iBullet < __Bullets.Length; iBullet++)
                {
                    if (__Bullets[iBullet] != null && __Asteroids[i].Collision(__Bullets[iBullet]))
                    {
                        __Asteroids[i].Damage += __Bullets[iBullet].Power();
                        __Bullets[iBullet] = null;
                    }
                }

                if (__Asteroids[i].Power <= 0)
                {
                    //var speed = rnd.Next(3, 10);
                    //var Power = rnd.Next(Asteroid.powerMin, Asteroid.powerMax);

                    //__Asteroids[i] = new Asteroid(
                    //    new BaseObjectParams
                    //    {
                    //        Position = new Point(rnd.Next(0, Width), rnd.Next(0, Height)),
                    //        Speed = new Point(speed, (rnd.Next(1, speed) % 2 == 0) ? -rnd.Next(1, speed) : rnd.Next(1, speed))
                    //    },
                    //    Power
                    //);
                    __Asteroids[i] = null;
                }
                __Asteroids[i]?.Update();

                //Корабль
                if (__Ship != null && __Ship.Collision(__Asteroids[i]))
                {
                    if(__Asteroids[i] != null)
                        __Ship.Energy -= __Asteroids[i].Power;
                    if (__Ship.Energy <= 0)
                        __Ship.Die();
                }
            }
            
        }
    }
}
