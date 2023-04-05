// See https://aka.ms/new-console-template for more information
using Day22;
using System.Diagnostics;

Console.WriteLine("Day 22");

string[] rows = File.ReadAllLines("data.txt");

List<Cube> cubes = new List<Cube>();

for (int j = 0; j < rows.Length; j++)
{
    cubes.Add(new Cube(rows[j], j));
}

Console.WriteLine("Read {0} cubes", cubes.Count);

int minx = Int32.MaxValue, maxx = Int32.MinValue, miny = Int32.MaxValue, maxy = Int32.MinValue, minz = Int32.MaxValue, maxz = Int32.MinValue;


// understand the instructions better
int[] countIntersections = new int[rows.Length];
int[] countIntersectionsOn = new int[rows.Length];
int[] countIntersectionsOff = new int[rows.Length];

for (int j = 0; j < rows.Length; j++)
{
    countIntersections[j] = 0;
    countIntersectionsOn[j] = 0;
    countIntersectionsOff[j] = 0;

}

for (int j = 0; j < cubes.Count; j++)
    for (int i = j + 1; i < cubes.Count; i++)
        if (cubes[j].Intersects(cubes[i]))
        {
            //Console.WriteLine("Cube {0} intersects with cube {1}", j, i);
            countIntersections[j]++;
            countIntersections[i]++;

            if (cubes[i].On)
                countIntersectionsOn[j]++;
            else
                countIntersectionsOff[j]++;

            if (cubes[j].On)
                countIntersectionsOn[i]++;
            else
                countIntersectionsOff[i]++;
        }

int totalIntersections = 0;
for (int j = 0; j < cubes.Count(); j++)
{
    //Console.WriteLine("Cube {0} (of type On={1}) has {2} intersections, #On: {3}, #Off: {4}, Volume: {5}", j, cubes[j].On, countIntersections[j], countIntersectionsOn[j], countIntersectionsOff[j], cubes[j].Volume());
    totalIntersections += countIntersections[j];
}

Console.WriteLine("Total unique intersections {0}", totalIntersections / 2);

// ***************************************************************************************************************

ArrayInitializer arrayFactory = new ArrayInitializer();

//BitArray ba = new BitArray(500 * 500 * 500, false);

//Stopwatch sw1 = Stopwatch.StartNew();
//for (int i = 0; i < ba.Length; i++)
//    ba.Set(i, true);
//sw1.Stop();
//Console.WriteLine("BitArray > Time to init: {0}", sw1.ElapsedMilliseconds);

//sw1.Restart();
//decimal count = 1;
//for (int i = 0; i < ba.Length; i++)
//    if (ba.Get(i))
//        count++;
//sw1.Stop();
//Console.WriteLine("BitArray > Time to count: {0}", sw1.ElapsedMilliseconds);

Stopwatch sw = Stopwatch.StartNew();

Console.WriteLine("===> YetAnotherWay: Total on pixels={0}", YetAnotherWay(cubes));
// PART 2 - 1257350313518866

sw.Stop();
Console.WriteLine("===> Elapsed time in ms={0}", sw.ElapsedMilliseconds);

//sw.Restart();
//Console.WriteLine("===> SliceCountMethod: Total on pixels={0}", SliceCountMethod(cubes));
//sw.Stop();
//Console.WriteLine("===> Elapse ms={0}", sw.ElapsedMilliseconds);

Console.ReadLine();

decimal YetAnotherWay(List<Cube> cubes)
{
    List<Cube> universeCubes = new List<Cube>();

    foreach (Cube c in cubes)
    {
        List<Cube> newUniverseCubes = new List<Cube>();
        if (c.On)
            newUniverseCubes.Add(c);

        foreach (Cube universeCube in universeCubes)
        {
            var overlap = c.FilterIntersectingCubes(new List<Cube> { universeCube });

            Debug.Assert(overlap.Count() < 2); // there's only one overlap at most

            if (overlap.Count() > 0)
            {
                // only 2 branches are needed, looking at the universe cube, which is interesting. the cube that's there already doesn't really matter. weird.
                if (c.On && universeCube.On)
                {
                    overlap[0].On = false;
                    newUniverseCubes.Add(overlap[0]);
                }
                else if (c.On && !universeCube.On)
                {
                    overlap[0].On = true;
                    newUniverseCubes.Add(overlap[0]);
                }
                else if (!c.On && universeCube.On)
                {
                    overlap[0].On = false;
                    newUniverseCubes.Add(overlap[0]);
                }
                else if (!c.On && !universeCube.On)
                {
                    overlap[0].On = true;
                    newUniverseCubes.Add(overlap[0]);
                }
            }
        }

        universeCubes.AddRange(newUniverseCubes);
    }

    decimal sum = 0;
    foreach (Cube c in universeCubes)
    {
        if (c.On)
            sum += c.Volume();
        else
            sum -= c.Volume();
    }

    Console.WriteLine("Universe cubes: {0}", universeCubes.Count());

    return sum;
}


// everything below this is useless and doesn't work, it takes too much time

decimal SliceCountMethod(List<Cube> cubes)
{
    decimal volume = 0;

    // calculate map area, the area tha encompases all of the squares
    foreach (Cube cube in cubes)
    {
        if (cube.xStart < minx)
            minx = cube.xStart;
        if (cube.xEnd > maxx)
            maxx = cube.xEnd;

        if (cube.xStart < miny)
            miny = cube.yStart;
        if (cube.yEnd > maxy)
            maxy = cube.yEnd;

        if (cube.zStart < minz)
            minz = cube.zStart;
        if (cube.zEnd > maxz)
            maxz = cube.zEnd;
    }

    int xsize = maxx - minx + 1;
    int ysize = maxy - miny + 1;
    int zsize = maxz - minz + 1;

    // we don't have ram for everything, so let's do it 500x500x500 cube by cube, assuming the max we can do is 500px cubes
    int submegacubex = 0, submegacubey = 0, submegacubez = 0;

    // declare a cube of pixels
    bool[,,] pixelGrid = new bool[500, 500, 500];

    while (submegacubex < xsize)
    {
        while (submegacubey < ysize)
        {
            while (submegacubez < zsize)
            {
                int gridsizex = xsize - submegacubex < 500 ? xsize - submegacubex : 500;
                int gridsizey = ysize - submegacubey < 500 ? ysize - submegacubey : 500;
                int gridsizez = zsize - submegacubez < 500 ? zsize - submegacubez : 500;

                decimal cubeVolume = 0;

                // now go over each of the intersections in order and flip the pixels
                Cube megaCube = new Cube(false, minx + submegacubex, minx + submegacubex + gridsizex,
                                                miny + submegacubey, miny + submegacubey + gridsizey,
                                                minz + submegacubez, minz + submegacubez + gridsizez, -1);

                var submegacubeintersections = megaCube.FilterIntersectingCubes(cubes);

                bool stopProcessingLoop = TryOptimizeIntersections(megaCube, submegacubeintersections, ref cubeVolume);
                if (stopProcessingLoop)
                {
                    volume += cubeVolume;
                    submegacubez += 500;
                    continue;
                }

                Console.WriteLine("After optimizations - Cube xStart {0}, yStart {1}, zStart {2} - Count trimmed intersections {3}", megaCube.xStart, megaCube.yStart, megaCube.zStart, submegacubeintersections.Count());

                if (submegacubeintersections.Count > 0) // always true
                {
                    // initialize as per the cube's lightness
                    // Stopwatch sw = Stopwatch.StartNew();
                    pixelGrid = arrayFactory.CreateArray(megaCube.On); // speed up from 600ms to ~125ms
                    // InitializeCube(pixelGrid, megaCube.On, gridsizex, gridsizey, gridsizez);
                    // sw.Stop();
                    // Console.WriteLine("Time to initialize: {0}ms", sw.ElapsedMilliseconds);

                    foreach (Cube intersection in submegacubeintersections)
                    {
                        // auxiliary variables for clarity only
                        int gridXStart = intersection.xStart - megaCube.xStart;
                        int gridYStart = intersection.yStart - megaCube.yStart;
                        int gridZStart = intersection.zStart - megaCube.zStart;

                        int gridXend = gridXStart + intersection.xEnd - intersection.xStart;
                        int gridYend = gridYStart + intersection.yEnd - intersection.yStart;
                        int gridZend = gridZStart + intersection.zEnd - intersection.zStart;

                        for (int x = gridXStart; x < gridXend; x++)
                            for (int y = gridYStart; y < gridYend; y++)
                                for (int z = gridZStart; z < gridZend; z++)
                                    pixelGrid[x, y, z] = intersection.On;
                    }

                    //Stopwatch sw = Stopwatch.StartNew();
                    cubeVolume = CountOnPixels(pixelGrid, gridsizex, gridsizey, gridsizez);
                    //sw.Stop();
                    //Console.WriteLine("Time to count: {0}ms", sw.ElapsedMilliseconds);
                }
                else
                {
                    if (megaCube.On)
                        cubeVolume = megaCube.Volume();
                }

                volume += cubeVolume;
                // Console.WriteLine("On pixels per megacube={0} of cube #{1}", cubeVolume, megaCube.CubeNumber);

                submegacubez += 500;
            }
            submegacubey += 500;
            submegacubez = 0;
        }
        submegacubex += 500;
        submegacubez = 0;
        submegacubey = 0;
    }

    return volume;
}

bool TryOptimizeIntersections(Cube megaCube, List<Cube> intersections, ref decimal volume)
{
    if (intersections.Count == 0)
    {
        volume = 0;
        return true;
    }

    if (intersections.Count == 1)
    {
        // no need to check if it's the same size as the megacube, as that's always false
        if (intersections[0].On)
        {
            volume = intersections[0].Volume();
            return true;
        }
        else
        {
            volume = 0;
            return true;
        }
    }

    if (intersections.Count > 1)
    {
        // remove adjacent intersections that are equal
        int processedIntersectionPointer = 0;
        while (processedIntersectionPointer < intersections.Count() - 1) // there are still at least one more element on the list to process
        {
            if (intersections[processedIntersectionPointer].Equals(intersections[processedIntersectionPointer + 1]) &&
                intersections[processedIntersectionPointer].On == intersections[processedIntersectionPointer + 1].On)
            {
                intersections.RemoveAt(processedIntersectionPointer + 1);
            }
            else
                processedIntersectionPointer++;
        }

        bool allTrue = true, allFalse = true;
        bool oneIsEqualToCube = false;
        foreach (Cube intersection in intersections)
        {
            allTrue = allTrue && intersection.On;
            allFalse = allFalse && !intersection.On;

            if (megaCube.Equals(intersection))
                oneIsEqualToCube = true;
        }

        if (allTrue && oneIsEqualToCube)
        {
            volume = megaCube.Volume();
            return true; // meaning we can stop processing these intersections
        }
        else if (allTrue && intersections.Count == 2)
        {
            volume += intersections[0].Volume() + intersections[1].Volume() - intersections[0].FilterIntersectingCubes(new List<Cube> { intersections[1] })[0].Volume();
            return true; // meaning we can stop processing these intersections
        }
        else if (allFalse)
        {
            volume = 0;
            return true;
        }
        else if (!allTrue && !allFalse && intersections.Count == 2)
        {
            // one On intersection and one Off intersection
            List<Cube> subIntersections = intersections[0].FilterIntersectingCubes(new List<Cube> { intersections[1] });

            // this will crash eventually -- intersections with length 2 and subintersections with length 1
            if (intersections[0].On)
            {
                volume = subIntersections[0].Volume() - subIntersections[0].Volume();
                return true;
            }
            else
            {
                volume = subIntersections[1].Volume() - subIntersections[0].Volume();
                return true;
            }
        }

        if (!intersections[intersections.Count() - 1].On && megaCube.Equals(intersections[intersections.Count() - 1]))
        {
            // the last region of the intersection is an Off that ocupies all the megacube
            volume = 0;
            return true;
        }

        // there's more that can be done here -- ex, if the # of resulting cubes is low... NOTAJOTA
        if (intersections.Count() == 3 && intersections[0].On && intersections[1].On && !intersections[2].On)
        {
            var intAB = intersections[0].FilterIntersectingCubes(new List<Cube> { intersections[1] });
            var intAC = intersections[0].FilterIntersectingCubes(new List<Cube> { intersections[2] });
            var intBC = intersections[1].FilterIntersectingCubes(new List<Cube> { intersections[2] });

            if (intAB.Count() > 0 && intAC.Count() > 0 && intBC.Count() > 0)
            {
                volume = intersections[0].Volume() + intersections[1].Volume() - intAB[0].Volume() - intBC[0].Volume();
                return true;
            }
        }
        else if (intersections.Count() == 3 && intersections[0].On && !intersections[1].On && !intersections[2].On)
        {
            var intAB = intersections[0].FilterIntersectingCubes(new List<Cube> { intersections[1] });
            var intAC = intersections[0].FilterIntersectingCubes(new List<Cube> { intersections[2] });
            var intBC = intersections[1].FilterIntersectingCubes(new List<Cube> { intersections[2] });

            if (intAB.Count() > 0 && intAC.Count() > 0 && intBC.Count() > 0)
            {
                volume = intersections[0].Volume() - intAB[0].Volume() - intAC[0].Volume() + intBC[0].Volume();
                return true;
            }
        }
    }

    return false;
}


void InitializeCube(bool[,,] pixelGrid, bool initValue, int sizex, int sizey, int sizez)
{
    for (int x = 0; x < sizex; x++)
        for (int y = 0; y < sizey; y++)
            for (int z = 0; z < sizez; z++)
                pixelGrid[x, y, z] = initValue;
}

decimal CountOnPixels(bool[,,] pixelGrid, int sizex, int sizey, int sizez)
{
    decimal total = 0;

    // parallel for increases time to 3.5s to 4s compares to just the looks, 2.5s to 3.5s
    //Parallel.For(0, sizex, i =>
    // {
    //     for (int y = 0; y < sizey; y++)
    //         for (int z = 0; z < sizez; z++)
    //             if (pixelGrid[i, y, z]) total++;
    // });

    // this takes 1.7 to 2.0 seconds in release mode
    for (int x = 0; x < sizex; x++)
        for (int y = 0; y < sizey; y++)
            for (int z = 0; z < sizez; z++)
                if (pixelGrid[x, y, z]) total++;

    return total;
}

// from https://stackoverflow.com/questions/2762424/what-is-the-fastest-way-to-initialize-a-multi-dimensional-array-to-non-default-v
// to faster initialize
public class ArrayInitializer
{
    private bool[,,] _allFalse = null;
    private bool[,,] _allTrue = null;

    public ArrayInitializer()
    {
        _allFalse = new bool[500, 500, 500];
        for (int x = 0; x < 500; x++)
            for (int y = 0; y < 500; y++)
                for (int z = 0; z < 500; z++)
                    _allFalse[x, y, z] = false;

        _allTrue = new bool[500, 500, 500];
        for (int x = 0; x < 500; x++)
            for (int y = 0; y < 500; y++)
                for (int z = 0; z < 500; z++)
                    _allTrue[x, y, z] = true;

    }

    public bool[,,] CreateArray(bool which)
    {
        if (which)
            return (bool[,,])_allTrue.Clone();
        else
            return (bool[,,])_allFalse.Clone();
    }
}