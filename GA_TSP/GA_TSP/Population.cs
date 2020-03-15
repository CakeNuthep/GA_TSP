using System;
using System.Collections.Generic;
using System.Linq;

namespace GA_TSP
{

    public class Population
    {

        // Member variables

        //เป็นตัวแปรที่เก็บ Poppulation ซึ่ง Population ดังกล่าวจะมีสมาชิกที่เป็น Tour อยู่
        public List<Tour> p { get; private set; }

        //เป็นตัวแปรที่เก็บผลคำตอบในการเดินทางของ Tour ที่ดีที่สุด
        public double maxFit { get; private set; }

        
        public Population(List<Tour> l)
        {
            this.p = l;
            this.maxFit = this.calcMaxFit();
        }

        /// <summary>
        /// เป็น method ในการ random ประชากรในที่นี้คือ tour มาทั้งหมด n tour
        /// </summary>
        /// <param name="t">คือ Tour เพื่อบอกว่าต้องการไป City ใหนบ้าง</param>
        /// <param name="n">คือจำนวนประชากรที่ต้องการสร้าง</param>
        /// <returns></returns>
        public static Population randomized(Tour t, int n)
        {
            List<Tour> tmp = new List<Tour>();

            for (int i = 0; i < n; ++i)
                tmp.Add( t.shuffle() );

            return new Population(tmp);
        }

        /// <summary>
        /// เป็น method ในการคำนวณค่าผลคำตอบในการเดินทางของ Tour ที่ดีที่สุด
        /// </summary>
        /// <returns></returns>
        private double calcMaxFit()
        {
            return this.p.Max( t => t.fitness );
        }

        /// <summary>
        /// เป็น Method สำหรับการสุ่มเลือก Tour มาหนึ่ง Tour
        /// </summary>
        /// <returns></returns>
        public Tour select()
        {
            while (true)
            {
                int i = Program.r.Next(0, Env.popSize);

                if (Program.r.NextDouble() < this.p[i].fitness / this.maxFit)
                    return new Tour(this.p[i].t);
            }
        }

        /// <summary>
        /// เป็น Method สำหรับการสร้าง new generation ขึ้นมาใหม่ n จำนวนซึ่งจะใช้วิธีการ crossover และ mutate
        /// </summary>
        /// <param name="n">คือ จำนวน generation ใหม่ที่ต้องการสร้าง</param>
        /// <returns></returns>
        public Population genNewPop(int n)
        {
            List<Tour> p = new List<Tour>();

            for (int i = 0; i < n; ++i)
            {
                Tour t = this.select().crossover( this.select() );

                foreach (City c in t.t)
                    t = t.mutate();

                p.Add(t);
            }

            return new Population(p);
        }

        /// <summary>
        /// เป็น method สำหรับเลือกประชากรที่มีผลคำตอบที่ดีที่สุดมา
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Population elite(int n)
        {
            List<Tour> best = new List<Tour>();
            Population tmp = new Population(p);

            for (int i = 0; i < n; ++i)
            {
                best.Add( tmp.findBest() );
                tmp = new Population( tmp.p.Except(best).ToList() );
            }

            return new Population(best);
        }

        /// <summary>
        /// เป็น method ในการหา Tour ที่ให้ผลคำตอบที่ดีที่สุด
        /// </summary>
        /// <returns></returns>
        public Tour findBest()
        {
            foreach (Tour t in this.p)
            {
                if (t.fitness == this.maxFit)
                    return t;
            }

            // Should never happen, it's here to shut up the compiler
            return null;
        }

        /// <summary>
        /// เป็น method สำหรับในการสร้าง generation รุ่นต่อไปตามหลัก genetic algorithm
        /// </summary>
        /// <returns></returns>
        public Population evolve()
        {
            //เป็นการเลือกประชากรที่มีผลคำตอบที่ดีที่สุดมา Env.elitism จำนวน
            Population best = this.elite(Env.elitism);

            //เป็นการสร้าง new generation ขึ้นมาใหม่มา Env.popSize - Env.elitism จำนวน
            Population np = this.genNewPop(Env.popSize - Env.elitism);

            //return ประชากรที่เกิดจาก เป็นการเลือกประชากรที่มีผลคำตอบที่ดีที่สุด กับ ที่สร้าง genration ใหม่
            return new Population( best.p.Concat(np.p).ToList() );
        }
    }
}

