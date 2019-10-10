using Common.Interfaces;
using System.Collections.Generic;

namespace Common.Models
{
    public class ModelEqualityComparer<TModel> : IEqualityComparer<TModel>
        where TModel : IModel
    {
        public bool Equals(TModel x, TModel y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(TModel obj)
        {
            return obj.Id;
        }
    }
}
