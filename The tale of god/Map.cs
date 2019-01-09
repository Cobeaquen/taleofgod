using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTaleOfGod;
using ProtoBuf;
using System.IO;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class Map
    {
        [ProtoMember(1)]
        public List<NPC> npcs;

        [ProtoMember(2)]
        public List<SceneObject> objects;

        [ProtoMember(3)]
        public List<Collider> colliders;

        [ProtoMember(4)]
        public List<Enemy> enemies;

        public Map()
        {
            npcs = new List<NPC>();
            objects = new List<SceneObject>();
            colliders = new List<Collider>();
            enemies = new List<Enemy>();
        }

        public void Save(string mapName)
        {
            using (var file = File.Create(mapName))
            {
                Serializer.Serialize(file, this);
            }
        }
        public static Map Load(string mapName)
        {
            using (var file = File.OpenRead(mapName))
            {
                return Serializer.Deserialize<Map>(file);
            }
        }
    }
}