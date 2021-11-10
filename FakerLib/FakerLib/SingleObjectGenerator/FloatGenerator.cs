using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    class FloatGenerator: IGenerator
    {
        private Random rand = new Random(DateTime.Now.Millisecond);
        private Type resultType = typeof(float);

        public Type ResultType
        {
            get
            {
                return resultType;
            }

        }

        public object Generate()
        {
            float result = (float)(rand.NextDouble() * rand.Next());

            if (rand.Next() % 2 == 0)
                result = -result;

            return result;
        }


    }
}
