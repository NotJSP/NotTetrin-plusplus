using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotTetrin.Ingame.Single.Marethon
{
    public class TileHelper {
        public Rect Rect { get; private set; }
        public int DivNumX { get; private set; }
        public int DivNumY { get; private set; }

        public Vector2 Rate { get; private set; }
        public Vector2 Unit { get; private set; }

        public TileHelper Calculate(Rect rect, Vector2 size, int divNumX, int divNumY) {
            this.Rect = rect;
            this.DivNumX = divNumX;
            this.DivNumY = divNumY;

            var distance = new Vector2(Rect.xMax - Rect.xMin, Rect.yMax - Rect.yMin);
            Unit = new Vector2(distance.x / DivNumX, distance.y / DivNumY);
            Rate = new Vector2(Unit.x / size.x, Unit.y / size.y);

            return this;
        }
    }
}
