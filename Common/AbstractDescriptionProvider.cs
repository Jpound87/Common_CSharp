using System;
using System.ComponentModel;

namespace Common
{
    public class AbstractDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        #region Constructor
        public AbstractDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }
        #endregion

        #region Methods
        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType.FullName == typeof(TAbstract).FullName)
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType.FullName == typeof(TAbstract).FullName)
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
        #endregion
    }
}
