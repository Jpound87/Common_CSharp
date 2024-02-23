using System;
using System.Linq;

namespace Common.Struct
{
    #region IPair Interface
    public interface IPair<T> : IIdentifiable
    {
        #region Accessors
        T P1 { get; set; }
        T P2 { get; set; }
        bool Contains(T value);
        #endregion
    }
    #endregion

    public struct Pair<T> : IPair<T>
    {
        #region Identity
        public const String StructName = nameof(Pair<T>);
        public String Identity
        {
            get
            {
                return StructName;
            }
        }
        #endregion

        #region Readonly
        private readonly T[] values;
        #endregion

        #region Points
        public T P1
        {
            get
            {
                return values[0];
            }
            set
            {
                values[0] = value;
            }
        }
        public T P2
        {
            get
            {
                return values[1];
            }
            set
            {
                values[1] = value;
            }
        }
        #endregion

        #region Constructor
        public Pair(T p1, T p2)
        {
            values = new T[2] { p1, p2 };
        }
        #endregion

        #region Contains
        public bool Contains(T value)
        {
            if (values.Contains(value))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
