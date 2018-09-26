using System;
using System.Drawing;

namespace StarWars
{
    /// <summary>Игровой объект (инкапсулирующий логику риосвания на игровой сцене, перемещение и взаимодействие с другими объектами)</summary>
    internal abstract class GameObject : ICollision
    {
        /// <summary>Положение на экране</summary>
        protected Point _Position;

        /// <summary>Скорость движения между кадрами</summary>
        protected Point _Speed;

        /// <summary>Размер на игровой сцене</summary>
        protected Size _Size;

        public Rectangle Rect => new Rectangle(_Position, _Size);

        /// <summary>Инициализация нового игрового объекта</summary>
        /// <param name="Position">Положение на игровой сцене</param>
        /// <param name="Speed">Скорость перемещения между кадрами</param>
        /// <param name="Size">Размер на игровой сцене</param>
        public GameObject(BaseObjectParams param)
        {
            _Position = param.Position;
            _Speed = param.Speed;
            _Size = param.Size;
        }

        public bool Collision(ICollision obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Rect.IntersectsWith(obj.Rect);
        }

        /// <summary>Метод отрисовки графики объекта на игровой сцене</summary>
        public abstract void Draw();

        /// <summary>Метод обновления состояния объекта при смене кадров</summary>
        public abstract void Update();
        //{
        //    _Position.X += _Speed.X;  // Перемещаем объект на сцене в соответствии с вектором скорости
        //    _Position.Y += _Speed.Y;

        //    // Проверяем граничные условия выхдода объекта за пределы сцены (меняем знак соответствующей составляющей вектора скорости)
        //    if (_Position.X < 0 || _Position.X > Game.Width - _Size.Width)
        //        _Speed.X *= -1;
        //    if (_Position.Y < 0 || _Position.Y > Game.Height - _Size.Height)
        //        _Speed.Y *= -1;
        //}

        public virtual void Zoom(int zoom)
        {
            _Size.Width = _Size.Width * zoom;
            if (_Size.Width >= 15) _Size.Width = 3;

            _Size.Height = _Size.Height * zoom;
            if (_Size.Height >= 15) _Size.Height = 3;
        }
    }
}

