using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Water
{
    [DataContract]
    public class Stage
    { 
        public String name { get; set; }

        [DataMember]
        public List<String> times { get; private set; }
        public Texture2D stageThumbnail { get; private set; }
        public Texture2D stageBackground { get; set; }

        List<AActor> enemies;

        public Stage()
        {

        }

    }
    
}
