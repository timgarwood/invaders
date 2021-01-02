using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Game1.GameObjects
{
    public class GameObjectDefinitionFactory
    {
        private Dictionary<string, object> _gameObjectDefinitions;


        public void Load<T>(Stream stream)
        {
            using(stream)
            {
                using(var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var ts = JsonConvert.DeserializeObject<T[]>(json);
                    foreach(var t in ts)
                    {
                        
                    }
                }
            }
        }

        public T GetDefinition<T>(string name)
        {
            if(_gameObjectDefinitions.ContainsKey(name))
            {
                return (T)_gameObjectDefinitions[name];
            }

            return default(T);
        }
    }
}
