using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    class ObjectGenerator: IGenerator
    {
        private Type resultType = typeof(object);

        public Type ResultType
        {
            get
            {
                return resultType;
            }

        }

        public object Generate()
        {
            return new object();
        }

    }
}
