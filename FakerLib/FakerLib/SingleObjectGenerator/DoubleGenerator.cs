using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    class DoubleGenerator : IGenerator
    {
        private Random rand = new Random(DateTime.Now.Millisecond);
        private Type resultType = typeof(double);

        public Type ResultType
        {
            get
            {
                return resultType;
            }

        }

        public object Generate()
        { 
            double result = rand.NextDouble()*rand.Next();

            if (rand.Next() % 2 == 0)
                result = -result;
           
            return result;
        }

    }
}
