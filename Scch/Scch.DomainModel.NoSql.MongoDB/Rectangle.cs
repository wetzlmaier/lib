using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Scch.DomainModel.NoSql.MongoDB
{
    [XmlRoot]
    [Serializable]
    public class Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);
        private System.Drawing.Rectangle _inner;

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public Point Location
        {
            get { return _inner.Location; }
            set { _inner.Location = value; }
        }

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public Size Size
        {
            get { return _inner.Size; }
            set { _inner.Size = value; }
        }

        [Browsable(false)]
        public int X
        {
            get { return _inner.X; }
            set { _inner.X = value; }
        }

        [Browsable(false)]
        public int Y
        {
            get { return _inner.Y; }
            set { _inner.Y = value; }
        }

        [Browsable(false)]
        public int Width
        {
            get { return _inner.Width; }
            set { _inner.Width = value; }
        }

        [Browsable(false)]
        public int Height
        {
            get { return _inner.Height; }
            set { _inner.Height = value; }
        }

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public int Left
        {
            get { return _inner.Left; }
        }

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public int Top
        {
            get { return _inner.Top; }
        }

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public int Right
        {
            get { return _inner.Right; }
        }

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public int Bottom
        {
            get { return _inner.Bottom; }
        }

        [Browsable(false)]
        [BsonIgnore]
        [XmlIgnore]
        public bool IsEmpty
        {
            get { return _inner.IsEmpty; }
        }

        public Rectangle()
            : this(0, 0, 0, 0)
        {
        }

        public Rectangle(int x, int y, int width, int height)
        {
            _inner.X = x;
            _inner.Y = y;
            _inner.Width = width;
            _inner.Height = height;
        }

        public Rectangle(Point location, Size size)
            : this(location.X, location.Y, size.Width, size.Height)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Rectangle))
                return false;

            Rectangle rectangle = (Rectangle)obj;
            return _inner.Equals(rectangle._inner);
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left == right || (left != null && right != null && left._inner == right._inner);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }

        public bool Contains(int x, int y)
        {
            return _inner.Contains(x, y);
        }

        public bool Contains(Point pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(Rectangle rect)
        {
            return rect != null && _inner.Contains(rect._inner);
        }

        public override int GetHashCode()
        {
            return _inner.GetHashCode();
        }

        public void Inflate(int width, int height)
        {
            _inner.Inflate(width, height);
        }

        public void Inflate(Size size)
        {
            _inner.Inflate(size);
        }

        public static Rectangle Inflate(Rectangle rect, int x, int y)
        {
            if (rect == null)
                throw new ArgumentNullException("rect");

            Rectangle result = new Rectangle(rect.Location, rect.Size);
            result.Inflate(x, y);
            return result;
        }

        public void Intersect(Rectangle rect)
        {
            if (rect == null)
                throw new ArgumentNullException("rect");

            Rectangle rectangle = Intersect(rect, this);
            X = rectangle.X;
            Y = rectangle.Y;
            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            if (b == null)
                throw new ArgumentNullException("b");

            return System.Drawing.Rectangle.Intersect(a._inner, b._inner);
        }

        public bool IntersectsWith(Rectangle rect)
        {
            return rect != null && _inner.IntersectsWith(rect._inner);
        }

        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            if (b == null)
                throw new ArgumentNullException("b");

            return System.Drawing.Rectangle.Union(a._inner, b._inner);
        }

        public void Offset(Point pos)
        {
            _inner.Offset(pos);
        }

        public void Offset(int x, int y)
        {
            _inner.Offset(x, y);
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public static implicit operator Rectangle(System.Drawing.Rectangle rhs)
        {
            return new Rectangle(rhs.Location, rhs.Size);
        }

        public static implicit operator System.Drawing.Rectangle(Rectangle rhs)
        {
            return new System.Drawing.Rectangle(rhs.Location, rhs.Size);
        }
    }
}
