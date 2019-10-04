using System;
namespace ItemObserveApp.Models.Validator
{
    public interface IValidate<T> where T : class
    {
        string Validate(T target);
    }
}
