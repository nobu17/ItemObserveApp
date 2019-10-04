using System;
using Prism.Mvvm;

namespace ItemObserveApp.Common
{
    public class BaseModel : BindableBase
    {
        public BaseModel()
        {
        }

        public virtual string Validate()
        {
            return string.Empty;
        }
    }
}
