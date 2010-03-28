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
        public const String FONT = "font";

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

        public const String FLOOR_TEXTURE = "floortexture";

        public const String WALL_TEXTURE = "walltexture";

        public const String CEILING_TEXTURE = "ceilingtexture";

        public const String JAIL_BARS = "jailbars";

        public const String PLAYER_DEFAULT_GUN = "playerdefgun";

        public const String PLAYER_GUN_SND = "playergunsound";

        public const String PLAYER_WALKING_SND = "playerwalkingsound";

        public const String OPERA_THEME_SONG = "operatheme";

        public const String GUNFIRE = "gunfire";

        public const String TERRORIST = "terrorist";

        public const String TERRORIST_WEAPON = "terweapon";

        public const String BULLET_BALL = "bulletball";

        private static Hashtable resHashTable = new Hashtable();

        public static void LoadTexture2D(ContentManager content, String fileName, String resName)
        {
            Texture2D texture = content.Load<Texture2D>(@"Images/"+fileName);
            RegisterResource(resName, texture);
        }

        public static void RegisterResource(String resName, object res)
        {
            if (IsResourceRegistered(resName)) 
                return;
            resHashTable.Add(resName, res);
        }

        public static bool IsResourceRegistered(String resName)
        {
            return (resHashTable.ContainsKey(resName));
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