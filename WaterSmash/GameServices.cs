using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water
{
    public static class GameServices
    {
        private static GameServiceContainer container;
        public static GameServiceContainer Instance
        {
            get
            {
                if (container == null)
                {
                    container = new GameServiceContainer();
                }
                return container;
            }
        }

        public static T GetService<T>()
        {
            return (T)Instance.GetService(typeof(T));
        }

        public static void AddService<T>(T service)
        {
            Instance.AddService(typeof(T), service);
        }

        public static void RemoveService<T>()
        {
            Instance.RemoveService(typeof(T));
        }
    }
}