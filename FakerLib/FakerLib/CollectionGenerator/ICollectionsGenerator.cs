using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    interface ICollectionGenerator
    {
        object Generate<T>( Faker faker);
        Type[] CollectionType { get; }
    }
}
