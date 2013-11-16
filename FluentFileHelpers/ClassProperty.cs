using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FluentFileHelpers.Enum;

namespace FluentFileHelpers
{
    public class ClassProperty<T>
    {
        private PropertyInfo Info;

        public Expression<Func<T, object>> MapExpression { get; private set; }
        public Action<T, object> MapAction { get; private set; }
        public Func<T, object> MapFunc { get; set; }
        public int Offset { get; private set; }
        public int Length { get; private set; }
        public bool TrimString { get; set; }
        public char? Pad { get; set; }
        public Alignment Alignment { get; set; }
        public string Format { get; set; }

        public ClassProperty()
        {
            TrimString = false;
        }

        public ClassProperty(Expression<Func<T, object>> MapExpression, int Offset, int Length)
            : this()
        {
            this.MapExpression = MapExpression;
            this.Offset = Offset;
            this.Length = Length;

            switch (MapExpression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    Info = (MapExpression.Body as MemberExpression).Member as PropertyInfo;
                    break;
                case ExpressionType.Convert:
                    Info = ((MapExpression.Body as UnaryExpression).Operand as MemberExpression).Member as PropertyInfo;
                    break;
            }

            ParameterExpression e1 = Expression.Parameter(Info.DeclaringType);
            ParameterExpression e2 = Expression.Parameter(typeof(object));

            MapAction = Expression
                            .Lambda<Action<T, object>>(
                                Expression.Assign(
                                    Expression.Property(e1, Info),
                                    Expression.Convert(e2, Info.PropertyType)
                                ),
                                e1,
                                e2
                            )
                            .Compile();

            MapFunc = MapExpression.Compile();
        }

        public ClassProperty<T> Trim()
        {
            TrimString = true;
            return this;
        }

        public ClassProperty<T> SetOffset(int Offset)
        {
            this.Offset = Offset;
            return this;
        }

        public ClassProperty<T> SetLength(int Length)
        {
            this.Length = Length;
            return this;
        }

        public ClassProperty<T> Align(Alignment Alignment)
        {
            this.Alignment = Alignment;
            return this;
        }

        public ClassProperty<T> Padding(char? PaddingCharacter)
        {
            this.Pad = PaddingCharacter;
            return this;
        }

        public ClassProperty<T> Formatter(string Format)
        {
            this.Format = Format;
            return this;
        }

        public string PadAndAlign(string s)
        {
            return (Alignment == Alignment.Right) ? s.PadLeft(Length, Pad.HasValue ? Pad.Value : ' ') : s.PadRight(Length, Pad.HasValue ? Pad.Value : ' ');
        }

        public string ToString(T target)
        {
            object o = MapFunc(target);
            if (new Type[] { typeof(decimal), typeof(Int16), typeof(Int32), typeof(Int64), typeof(double), typeof(float) }.Contains(o.GetType()))
                return string.IsNullOrWhiteSpace(Format) ? PadAndAlign(((dynamic)MapFunc(target)).ToString()) : PadAndAlign(((dynamic)MapFunc(target)).ToString(Format));
            return PadAndAlign(Convert.ToString(o));
        }

        public void Assign(T Target, string s)
        {
            string t = s.Substring(Offset, Length);
            t = TrimString ? t.Trim() : t;

            if (Info.PropertyType == typeof(string))
            {
                MapAction(Target, t);
                return;
            }

            if (Info.PropertyType == typeof(Int32))
            {
                MapAction(Target, Convert.ToInt32(t));
                return;
            }

            if (Info.PropertyType == typeof(decimal))
            {
                MapAction(Target, Convert.ToDecimal(t));
                return;
            }

        }

    }
}


