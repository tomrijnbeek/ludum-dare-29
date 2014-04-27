using System.Collections.Generic;

namespace LudumDare29.Levels
{
    enum LayerType
    {
        TileLayer,
        ObjectGroup
    };

    class LayerRepresentation
    {
        public LayerType Type;
        public int[] Data;
        public string Name;
        public ObjectRepresentation[] Objects;
        public Dictionary<string, bool> Properties;
    }
}