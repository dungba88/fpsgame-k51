using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FPSGame.Engine
{
    public class ResourceManager
    {
        public const String NEW_GAME_BUTTON = "newgamebtn";

        public const String NEW_GAME_BUTTON_OFF = "newgamebtnoff";

        public const String OPTION_BUTTON = "optbtn";

        public const String OPTION_BUTTON_OFF = "optbtnoff";

        public const String ABOUT_BUTTON = "aboutbtn";

        public const String ABOUT_BUTTON_OFF = "aboutbtnoff";

        public const String EXIT_BUTTON = "exitbtn";

        public const String EXIT_BUTTON_OFF = "exitbtnoff";

        public const String BACK_BUTTON = "backbtn";

        public const String BACK_BUTTON_OFF = "backbtnoff";

        private static Hashtable resHashTable = new Hashtable();

        public static void LoadTexture2D(ContentManager content, String fileName, String resName)
        {
            Texture2D texture = content.Load<Texture2D>(@"Images/"+fileName);
            RegisterResource(resName, texture);
        }

        public static void RegisterResource(String resName, object res)
        {
            if (resHashTable.ContainsKey(resName)) 
                return;
            resHashTable.Add(resName, res);
        }

        public static T GetResource<T>(String resName)
        {
            IDictionaryEnumerator enums = resHashTable.GetEnumerator();
            while (enums.MoveNext())
            {
                String key = (String)enums.Key;
                if (key == resName)
                    return (T)enums.Value;
            }
            return default(T);
        }            
    }
}