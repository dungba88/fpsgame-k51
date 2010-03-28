using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FPSGame.Core.AI
{
    public class StateSessionStorage
    {
        private static Hashtable hashtable = new Hashtable();

        public static void StoreVar(String var, object value)
        {
            if (IsVarRegistered(var)) 
                return;
            hashtable.Add(var, value);
        }

        public static bool IsVarRegistered(String var)
        {
            return hashtable.Contains(var);
        }

        public static T LoadVar<T>(String var)
        {
            IDictionaryEnumerator enums = hashtable.GetEnumerator();
            while (enums.MoveNext())
            {
                String key = (String)enums.Key;
                if (key == var)
                    return (T)enums.Value;
            }
            return default(T);
        }
    }
}
