using System.Drawing;
using System;
using StarWars.Properties;



namespace StarWars
{
    /// <summary>Игровой объект - звезда</summary>
    class Star : GameObject
    {
        protected const int starImgArrCount = 6;
        protected static Image[] imageStars = new Image[starImgArrCount];
        //protected static Random rndStarsImage = new Random();
        protected int starNumber;
        protected int starMaxSize = 30;

        /// <summary>Инициализация новой звезды</summary>
        /// <param name="Position">ПОложение на игровой сцене</param>
        /// <param name="Speed">Скорость перемещения между кадрами</param>
        /// <param name="Size">Размер на игровой сцене</param>
        public Star(BaseObjectParams param, int StarNumber) : base(param) // Передача параметров в конструктор предка
        {
            #region Exceptions
            //Exception размера звезды
            if (param.Size.Height > starMaxSize || param.Size.Width > starMaxSize) throw new StarWarsExceptions("Превышен максимальный размер звезды");

            //Exception размера массива
            if (StarNumber >= starImgArrCount) throw new StarWarsExceptions("Недопустимый размер массива звезд");
            #endregion

            starNumber = StarNumber;
            imageStars[0] = Resources.star01;
            imageStars[1] = Resources.star02;
            imageStars[2] = Resources.star03;
            imageStars[3] = Resources.star04;
            imageStars[4] = Resources.star05;
            imageStars[5] = Resources.star06;
        }

        
        /// <summary>Переорпделяем метод рисования</summary>
        public override void Draw()
        {
            //var g = Game.Buffer.Graphics;
            //g.DrawLine(Pens.White,
            //    _Position.X, _Position.Y,
            //    _Position.X + _Size.Width, _Position.Y + _Size.Height);
            //g.DrawLine(Pens.White,
            //    _Position.X + _Size.Width, _Position.Y,
            //    _Position.X, _Position.Y + _Size.Height);

            //#region звезды линиями
            //double c = 2.0 / 3.0;
            //Game.Buffer.Graphics.DrawLine(Pens.White,
            //    (float)(_Position.X - _Size.Width / 2 * c),
            //    (float)(_Position.Y - _Size.Height / 2 * c),
            //    (float)(_Position.X + _Size.Width / 2 * c),
            //    (float)(_Position.Y + _Size.Height / 2 * c));
            //Game.Buffer.Graphics.DrawLine(Pens.White,
            //    (float)(_Position.X + _Size.Width / 2 * c),
            //    (float)(_Position.Y - _Size.Height / 2 * c),
            //    (float)(_Position.X - _Size.Width / 2 * c),
            //    (float)(_Position.Y + _Size.Height / 2 * c));
            ////добавлен +
            //Game.Buffer.Graphics.DrawLine(Pens.White, _Position.X, _Position.Y - _Size.Height / 2, _Position.X, _Position.Y + _Size.Height / 2);
            //Game.Buffer.Graphics.DrawLine(Pens.White, _Position.X - _Size.Width / 2, _Position.Y, _Position.X + _Size.Width / 2, _Position.Y);
            //#endregion

            #region звезды картинками
            Game.Buffer.Graphics.DrawImage(imageStars[starNumber], new Rectangle(_Position, _Size));
            #endregion
        }

        /// <summary>Переопределяем метод обновления состояния</summary>
        public override void Update()
        {
            _Position.X -= _Speed.X;
            if (_Position.X < 0)
            {
                _Position.X = Game.Width + _Size.Width;
            }
        }
    }
}