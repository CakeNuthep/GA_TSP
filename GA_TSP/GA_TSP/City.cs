using System;

namespace GA_TSP
{
    public class City
    {

        // Member variables
        public double x { get; private set; }
        public double y { get; private set; }

        
        public City(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// เป็น Method สำหรับคำนวณระยะทางจาก เมือง นึงไป อีก เมือง นึง
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public double distanceTo(City c)
        {
            //คำนวณหาระยะทางจากทฤษฎีพิทาโกรัส
            return Math.Sqrt(Math.Pow((c.x - this.x), 2)
                            + Math.Pow((c.y - this.y), 2));
        }

        /// <summary>
        /// เป็น method สำหรับสร้างเมืองขึ้นมาโดยการสุ่มตำแหน่งของเมือง
        /// </summary>
        /// <returns></returns>
        public static City random()
        {
            return new City( Program.r.NextDouble(), Program.r.NextDouble() );
        }
    }
}
