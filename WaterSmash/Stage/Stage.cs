using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Water
{
    [DataContract]
    public class Stage
    {
        Generator generator;
        public String name { get; set; }

        [DataMember]
        public List<String> times { get; private set; }
        public Texture2D stageThumbnail { get; private set; }
        public Texture2D stageBackground { get; set; }

        private ContentManager content = GameServices.GetService<ContentManager>();
        public List<object> GameObjects = new List<object>();//holds all map blocks

        List<AActor> enemies;

        public Stage()
        {
            generator = new Generator();
            GameObjects = generator.generateMap();
        }


        
        //field voor generatemap()

        /// <summary>
        /// methode om een map te genereren.
        /// maakt een vloer waar de AActors op kunnen lopen
        /// </summary>

    }
    
}
