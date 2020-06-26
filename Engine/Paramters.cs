﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Paramters
    {
        public (int Up,int Left,int Right,int Down) border = (100,100,100,100);
        public (int Up,int Left,int Right,int Down) hitboxArea = (20,20,20,20);
        public int Delay = 0;
        //Generator Parameters
        public int BlockSize = 20;
        public int sizeOfStoneCollumn = 10;

        public int TreeSpread = 3;
        public int minimumFillarHeightForTree = 3;
        public int treeChance = 5;
    }

}
