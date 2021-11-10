using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    class LongGenerator:IGenerator
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private Type resultType = typeof(long);

        public Type ResultType
        {
            get
            {
                return resultType;
            }

        }

        public object Generate()
        {
            long result = rnd.Next() * rnd.Next();
            if (rnd.Next() % 2 == 0)
                result = -result;
            return result;
        }
    }
}
