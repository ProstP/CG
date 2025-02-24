using System.Collections.Generic;

namespace Task3_1.Model
{
    public class ElementsCreator
    {
        Dictionary<KeyValuePair<ElementType, ElementType>, List<ElementType>> _elements =
            new Dictionary<KeyValuePair<ElementType, ElementType>, List<ElementType>>()
            {
                { new KeyValuePair<ElementType, ElementType>(ElementType.Earth, ElementType.Air ),
                    new List<ElementType>() { ElementType.Dust } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Fire),
                    new List<ElementType>() { ElementType.Energy } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Earth, ElementType.Fire),
                    new List<ElementType>() { ElementType.Lava } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Earth),
                    new List<ElementType>() { ElementType.Mud} },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Earth, ElementType.Earth ),
                    new List<ElementType>() { ElementType.Pressure } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Air ),
                    new List<ElementType>() { ElementType.Pressure } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Air),
                    new List<ElementType>() { ElementType.Rain } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Water),
                    new List<ElementType>() { ElementType.Sea } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Fire),
                    new List<ElementType>() { ElementType.Steam } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Energy),
                    new List<ElementType>() { ElementType.Steam } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Steam),
                    new List<ElementType>() { ElementType.Cloud } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Fire, ElementType.Dust),
                    new List<ElementType>() { ElementType.Gunpowder } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Sea),
                    new List<ElementType>() { ElementType.Ocean } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Sea, ElementType.Sea),
                    new List<ElementType>() { ElementType.Ocean } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Earth, ElementType.Rain),
                    new List<ElementType>() { ElementType.Plant } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Sea, ElementType.Fire),
                    new List<ElementType>() { ElementType.Salt} },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Ocean, ElementType.Fire),
                    new List<ElementType>() { ElementType.Salt } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Lava),
                    new List<ElementType>() { ElementType.Stone } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Gunpowder, ElementType.Fire),
                    new List<ElementType>() { ElementType.Explosion, ElementType.Smoke } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Fire, ElementType.Stone),
                    new List<ElementType>() { ElementType.Metal } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Stone, ElementType.Air),
                    new List<ElementType>() { ElementType.Sand } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Cloud, ElementType.Electricity),
                    new List<ElementType>() { ElementType.Storm } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Cloud, ElementType.Energy),
                    new List<ElementType>() { ElementType.Storm } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Sea, ElementType.Wind),
                    new List<ElementType>() { ElementType.Wave } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Ocean, ElementType.Wind),
                    new List<ElementType>() { ElementType.Wave } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Energy, ElementType.Explosion),
                    new List<ElementType>() { ElementType.AtomicBomb } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Sea, ElementType.Sand),
                    new List<ElementType>() { ElementType.Beach } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Ocean, ElementType.Sand),
                    new List<ElementType>() { ElementType.Beach } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Water, ElementType.Sand),
                    new List<ElementType>() { ElementType.Beach } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Sand, ElementType.Sand),
                    new List<ElementType>() { ElementType.Desert } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Metal, ElementType.Energy),
                    new List<ElementType>() { ElementType.Electricity } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Fire, ElementType.Sand),
                    new List<ElementType>() { ElementType.Glass } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Electricity, ElementType.Sand),
                    new List<ElementType>() { ElementType.Glass } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Wave),
                    new List<ElementType>() { ElementType.Sound } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Pressure),
                    new List<ElementType>() { ElementType.Wind } },
                { new KeyValuePair<ElementType, ElementType>(ElementType.Air, ElementType.Energy),
                    new List<ElementType>() { ElementType.Wind } },

            };

        public List<ElementType> CreateNewElement(ElementType first, ElementType second)
        {
            KeyValuePair<ElementType, ElementType> key1 = new KeyValuePair<ElementType, ElementType>(first, second);

            if (_elements.ContainsKey(key1))
            {
                return _elements[key1];
            }
            key1 = new KeyValuePair<ElementType, ElementType>(second, first);
            if (_elements.ContainsKey(key1))
            {
                return _elements[key1];
            }

            return null;
        }
    }
}
