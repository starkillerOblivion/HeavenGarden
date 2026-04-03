using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenGarden.DB
{
    public static class Cart
    {
        public static List<RitualGoods> Goods { get; private set; } = new List<RitualGoods>();
        public static List<Services> Services { get; private set; } = new List<Services>();

        public static void AddItem(RitualGoods good) => Goods.Add(good);
        public static void AddService(Services service) => Services.Add(service);
        public static void Clear()
        {
            Goods.Clear();
            Services.Clear();
        }
    }
}
