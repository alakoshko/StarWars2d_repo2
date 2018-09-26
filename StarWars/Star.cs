using System.Drawing;

namespace StarWars
{
    /// <summary>Игровой объект - звезда</summary>
    class Star : GameObject
    {
        /// <summary>Инициализация новой звезды</summary>
        /// <param name="Position">ПОложение на игровой сцене</param>
        /// <param name="Speed">Скорость перемещения между кадрами</param>
        /// <param name="Size">Размер на игровой сцене</param>
        public Star(Point Position, Point Speed, Size Size)
            :base(Position, Speed, Size) // Передача параметров в конструктор предка
        {
             // Конструктор звезды ничего больше не делает
        }

        /// <summary>Переорпделяем метод рисования</summary>
        public override void Draw()
        {
            var g = Game.Buffer.Graphics;
            g.DrawLine(Pens.White, 
                _Position.X, _Position.Y, 
                _Position.X + _Size.Width, _Position.Y + _Size.Height);
            g.DrawLine(Pens.White, 
                _Position.X + _Size.Width, _Position.Y,
                _Position.X, _Position.Y + _Size.Height);
        }

        /// <summary>Переопределяем метод обновления состояния</summary>
        public override void Update()
        {
            _Position.X -= _Speed.X;
            if (_Position.X < 0)
                _Position.X = Game.Width + _Size.Width;
        }
    }
}