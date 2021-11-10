using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    public interface IGenerator
    {
        object Generate();
        Type ResultType { get; }
    }
}
