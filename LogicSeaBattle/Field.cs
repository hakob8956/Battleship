﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GameCore
{
   
    public class Field
    {
        public static int Size { get; } = 10;
        private int[,] plans = new int[Size, Size];
        public struct TestPoint
        {
            public int x { get; set; }
            public int y { get; set; }

        }
        public struct ArroundCords
        {
            public int[] xx { get; set; }
            public int[] yy { get; set; }
            public ArroundCords(int x, int y)
            {
                int[] xx = new int[8], yy = new int[8];
                xx[0] = x + 1; yy[0] = y + 1;
                xx[1] = x - 1; yy[1] = y + 1;
                xx[2] = x + 1; yy[2] = y - 1;
                xx[3] = x - 1; yy[3] = y - 1;
                xx[4] = x; yy[4] = y + 1;
                xx[5] = x + 1; yy[5] = y;
                xx[6] = x - 1; yy[6] = y;
                xx[7] = x; yy[7] = y - 1;
                this.xx = xx;
                this.yy = yy;
            }
        }
        public int[,] Plans
        {
            get { return plans; }
            set { plans = value; }
        }

        public Field()
        {
            plans = new int[Size, Size];
            ClearField();
        }
        public Field(int[,] _plans)
        {
            if (_plans.Length == plans.Length)
            {
                plans = _plans;
            }
            ClearField();

        }

        public void SetRandomShips()
        {
            //1->4
            //2->3     //4+6+6+4=20-->Count Ships
            //3->2
            //4->1
            int[] ships = { 1, 2, 3, 4 };
            Random random = new Random();

            for (int i = 0; i < ships.Length; i++)
            {
                while (ships[i] != 0)
                {
                    int x = random.Next(0, 10);
                    int y = random.Next(0, 10);
                    int dir = random.Next(0, 2);//vertical(0) or horizontal(1);
                    if (SetShip(ships.Length - i, dir, x, y))
                    {
                        ships[i]--;
                    }
                }
            }
        }
        
        public static bool CheckLocation(int x, int y)
        {
            if (!(x >= 0 && y >= 0 && x < Size && y < Size))
                return false;

            else
                return true;

        }
        TestPoint[] TestSetShip(int shipType, int orentation, int x, int y)
        {
            if (TestCanSetShip(x, y))
            {
                TestPoint[] testPoint = new TestPoint[shipType];
                switch (orentation)
                {
                    case (int)Orentation.Vertical:
                        testPoint = TestSetShipDir(shipType, 1, 0, x, y);
                        if (testPoint == null) testPoint = TestSetShipDir(shipType, -1, 0, x, y);
                        break;
                    case (int)Orentation.Horizontal:
                        testPoint = TestSetShipDir(shipType, 0, 1, x, y);
                        if (testPoint == null) testPoint = TestSetShipDir(shipType, 0, -1, x, y);
                        break;
                }
                return testPoint;

            }
            return null;
        }
        public bool SetShip(int shipType, int orentation, int x, int y)
        {
            TestPoint[] points = TestSetShip(shipType, orentation, x, y);
            if (points != null)
            {
                foreach (var point in points)
                {
                    plans[point.x, point.y] = (int)FieldType.Used;
                }
                return true;
            }
            return false;
        }
        public TestPoint[] TestSetShipDir(int shipType, int XD, int YD, int x, int y)
        {
            TestPoint[] testPoint = new TestPoint[shipType];
            for (int i = 0; i < shipType; i++)
            {
                if (TestCanSetShip(x, y))
                {
                    testPoint[i].x = x;
                    testPoint[i].y = y;

                }
                else
                    return null;
                x += XD;
                y += YD;
            }
            return testPoint;
        }
        public bool TestCanSetShip(int x, int y)
        {
            if (CheckLocation(x, y))
            {
                ArroundCords arround = new ArroundCords(x, y);
                if (plans[y, x] != (int)FieldType.Empty) return false;
                for (int i = 0; i < 8; i++)
                {
                    if (CheckLocation(arround.xx[i], arround.yy[i]))
                    {
                        if (plans[arround.xx[i], arround.yy[i]] != (int)FieldType.Empty)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public void ClearField()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    plans[i, j] = 0;
                }
            }
        }
        public void Display()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(plans[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

    }
}