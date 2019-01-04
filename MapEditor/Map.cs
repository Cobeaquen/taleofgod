using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTaleOfGod;
using ProtoBuf;
using System.IO;

namespace MapEditor
{
    [ProtoContract]
    public class Map
    {
        [ProtoMember(1)]
        public NPC[] npcs;

        [ProtoMember(2)]
        public SceneObject[] objects;

        [ProtoMember(3)]
        public Collider[] colliders;

        public Map()
        {

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