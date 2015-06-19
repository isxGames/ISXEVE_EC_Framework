using System;
using System.Collections.Generic;
using System.Linq;
using EveCom;

namespace EveComFramework.KanedaToolkit
{

    public class VelocityComparer : Comparer<Entity>
    {
        public override int Compare(Entity x, Entity y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (x == y)
                return 0;

            if (x.Velocity < y.Velocity)
                return -1;
            if (x.Velocity > y.Velocity)
                return 1;
            if (x.Velocity == y.Velocity)
                return 0;
            return 0;
        }
    }

}
