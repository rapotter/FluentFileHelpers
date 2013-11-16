using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluentFileHelpers
{
    public abstract class ClassMapBase<T> where T : new()
    {
        protected IList<ClassProperty<T>> Fields = new List<ClassProperty<T>>();

        public ClassMapBase() { }

        public ClassProperty<T> Map(Expression<Func<T, object>> MapExpression, int Offset, int Length)
        {
            ClassProperty<T> Field = new ClassProperty<T>(MapExpression, Offset, Length);
            Fields.Add(Field);
            return Field;
        }

        public T Assign(string s)
        {
            return Assign(new T(), s);
        }

        public T Assign(T target, string s)
        {
            foreach (ClassProperty<T> Field in Fields)
            {
                Field.Assign(target, s);
            }

            return target;
        }

        public string ToString(T target)
        {
            string s = string.Empty;
            foreach (ClassProperty<T> Field in Fields)
            {
                s += Field.ToString(target);
            }
            return s;
        }

    }
}
