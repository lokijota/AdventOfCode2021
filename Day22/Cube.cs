using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Day22
{
    public class Cube
    {
        static Regex _regex = new Regex(@"^(on|off)...(-?\d+)..(-?\d+)...(-?\d+)..(-?\d+)...(-?\d+)..(-?\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // on x = -26071..-15399, y = -54535..-33151, z = 64373..77023

        public Cube(bool On, int xStart, int xEnd, int yStart, int yEnd, int zStart, int zEnd, int cubeNumber)
        {
            this.On = On;
            this.xStart = xStart;
            this.xEnd = xEnd;
            this.yStart = yStart;
            this.yEnd = yEnd;
            this.zStart = zStart;
            this.zEnd = zEnd;
            this.CubeNumber = cubeNumber;
        }

        public Cube(string s, int cubeNumber)
        {
            // Find matches.
            MatchCollection matches = _regex.Matches(s);
            GroupCollection groups = matches[0].Groups;

            if (groups[1].Value == "on")
                On = true;
            else
                On = false;

            int value = Int32.Parse(groups[2].Value);
            xStart = value;

            value = Int32.Parse(groups[3].Value);
            xEnd= value;

            value = Int32.Parse(groups[4].Value);
            yStart = value;

            value = Int32.Parse(groups[5].Value);
            yEnd = value;

            value = Int32.Parse(groups[6].Value);
            zStart = value;

            value = Int32.Parse(groups[7].Value);
            zEnd = value;

            CubeNumber = cubeNumber;
        }

        public bool IsIn(int x, int y, int z)
        {
            if (x >= xStart && x <= xEnd && y >= yStart && y <= yEnd && z >= zStart && z <= zEnd)
                return true;
            else
                return false;
        }

        public bool Intersects(Cube b)
        {
            // https://gamedev.stackexchange.com/questions/130408/what-is-the-fastest-algorithm-to-check-if-two-cubes-intersect-where-the-cubes-a
            // https://stackoverflow.com/questions/5009526/overlapping-cubes

            if (this.xEnd >= b.xStart && this.xStart <= b.xEnd && 
                this.yEnd >= b.yStart && this.yStart <= b.yEnd && 
                this.zEnd >= b.zStart && this.zStart <= b.zEnd)
                return true;

            return false;
        }

        public decimal Volume()
        {
            return (decimal) Math.Abs((decimal)xEnd - xStart + 1) * Math.Abs((decimal)yEnd - yStart + 1) * Math.Abs((decimal)zEnd - zStart + 1);
        }

        public List<Cube> FilterIntersectingCubes(List<Cube> cubes)
        {
            List<Cube> intersections = new List<Cube>();

            foreach (Cube c in cubes)
            {
                if (Intersects(c) && this != c)
                {
                    // check this for any doubt: https://stackoverflow.com/questions/5556170/finding-shared-volume-of-two-overlapping-cuboids#5556796

                    Cube overlap = new Cube(c.On, 0, 0, 0, 0, 0, 0, 0);

                    // NOTAJOTA -- N PRECISO DE METER TAMBÉM O SINAL?

                    overlap.On = c.On;

                    overlap.xStart = Math.Max(xStart, c.xStart);
                    overlap.xEnd = Math.Min(xEnd, c.xEnd);

                    overlap.yStart = Math.Max(yStart, c.yStart);
                    overlap.yEnd = Math.Min(yEnd, c.yEnd);

                    overlap.zStart = Math.Max(zStart, c.zStart);
                    overlap.zEnd = Math.Min(zEnd, c.zEnd);

                    overlap.CubeNumber = c.CubeNumber;

                    intersections.Add(overlap);
                }
            }

            return intersections;
        }

        public bool On { get; set; }
        public int xStart { get; set; }
        public int xEnd{ get; set; }
        public int yStart { get; set; }
        public int yEnd { get; set; }
        public int zStart { get; set; }
        public int zEnd { get; set; }
        public int CubeNumber { get; set; }

        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Cube))
            {
                return false;
            }
            return //(this.On == ((Cube)obj).On) &&
                (this.xStart == ((Cube)obj).xStart)
                && (this.xEnd == ((Cube)obj).xEnd)
                && (this.yStart == ((Cube)obj).yStart)
                && (this.yEnd == ((Cube)obj).yEnd)
                && (this.zStart == ((Cube)obj).zStart)
                && (this.zEnd == ((Cube)obj).zEnd);
        }

        public override int GetHashCode()
        {
            return /*On.GetHashCode() ^*/ xStart.GetHashCode() ^ xEnd.GetHashCode() ^ yStart.GetHashCode() ^ yEnd.GetHashCode() ^ zStart.GetHashCode() ^ zEnd.GetHashCode();
        }
    }
}
