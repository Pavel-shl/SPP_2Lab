using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    class StringGenerator : IGenerator
    {

        private Random rnd = new Random(DateTime.Now.Millisecond);
        private static string win1251 = Encoding.GetEncoding(1251).GetString(Enumerable.Range(0, 256).Select(n => (byte)n).ToArray());


        private Type resultType = typeof(string);
        private int minLength = 0;
        private int maxLength = 255;
        public Type ResultType
        {
            get
            {
                return resultType;
            }

        }

        public object Generate()
        {
            string result = "";
            int len = rnd.Next(minLength, maxLength);

            for (int i =0;i<len;i++)
            {
                result = result + win1251[rnd.Next(32,255)];
            }

            return result;
        }
    }
}
