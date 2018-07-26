using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silica_Animus.Extension
{
    public static class SemanticExtension
    {

        public static bool In<T>(this T data, params T[] par)
        {
            return par.Contains(data);
        }

        public static (Type objectType, bool foundInList) IsType<T>(this T data, params Type[] par)
        {
            (Type objectType, bool foundInList) returned = (data.GetType(), false);
            foreach (var t in par)
            {
                if (data.GetType() == t)
                {
                    returned = (t, true);
                    break;
                }
            }

            return returned;
        }
    }
}
