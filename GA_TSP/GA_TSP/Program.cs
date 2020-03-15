using GA_TSP;
using System;

public class Program
{
    public static Random r { get; private set; }

    public static void Main()
    {
        r = new Random();
        
        // ทำการสร้าง random ของ tour ขึ้นมา 1 tour โดยใน Tore นั้นจะประกอบไปด้วยหลายเมืองเป็นจำนวนเมืองทั้งหมด Env.numCities เมือง
        Tour dest = Tour.random(Env.numCities);

        //ทำการสร้างประชากรที่ประกอบไปด้วยหลาย tour ซึ่งแต่ละ toure เกิดจากการ random shuffle ก็คือทำการสลับลำดับของเมืองแบบสุ่มนั่นเอง ซึ่งจะมี toure ทั้งหมด Env.popSize tour
        Population p = Population.randomized(dest, Env.popSize);

        //ประกาศตัวแปรที่ชื่อว่า gen ไว้สำหรับเป็นตัวระบุว่า ได้ทำการประมวลผลของ genetic algorithm ถึง generation ที่เท่าไหร่แล้ว
        int gen = 0;

        //เป็นการบอกว่า generation ปัจจุบันให้ผลคำตอบในการเดินทางดีกว่า generation ก่อนหน้าหรือไม่ ถ้าดีกว่าค่า better จะเป็น true ถ้าไม่ดีค่า better จะเป็น false
        bool better = true;

        //ให้ทำการวนลูปไม่มีที่สิ้นสุด
        while (true /*gen < 100 */ )
        {
            //เป็นการบอกว่า generation ปัจจุบันให้ผลคำตอบในการเดินทางดีกว่า generation ก่อน
            if (better)
                display(p, gen);  //ทำการแสดงผลออกทางหน้าจอ

            better = false;
            //เก็บค่าผลคำตอบในการเดินทางที่ดีที่สุดก่อนหน้ามาใส่ในตัวแปรที่ชื่อว่า oldFit
            double oldFit = p.maxFit;

            //ทำการสร้าง geeration รุ่นต่อไปด้วยวิธิการทาง genetic algorithm
            p = p.evolve();

            //ถ้า generation ปัจจุบันให้ผลคำตอบในการเดินทางดีกว่า generation ก่อนหน้า
            if (p.maxFit > oldFit)
                better = true;  //ให้ทำการเก็บค่า better เป็นค่า true

            //ทำการนับว่าถึง generation ใหนแล้ว
            gen++;
        }
    }

    public static void display(Population p, int gen)
    {
        Tour best = p.findBest();
        System.Console.WriteLine("Generation {0}\n" +
            "Best fitness:  {1}\n" +
            "Best distance: {2}\n", gen, best.fitness, best.distance);
    }
}

public static class Env
{
    public const double mutRate = 0.03;
    public const int elitism = 6;
    public const int popSize = 60;
    public const int numCities = 40;
}