﻿using PixBlocks.PythonIron.Tools.Integration;
using System;

namespace Engine.GUI.Models
{
    public class IndexedButton : Button
    {

        public IndexedButton(Vector vector, object objectToRepresent, int size, Action<PixControl> taskToRepresent, int index) : base(vector, objectToRepresent.ToString(), size, taskToRepresent)
        {
            ObjectToRepresent = objectToRepresent;
            Index = index;
        }

        public object ObjectToRepresent { get; }
        public int Index { get; }

        private bool active;
        public bool Active 
        { 
            get => active; 
            set 
            { 
                active = value;
                color = value ? new Color(140, 200, 230) : new Color(15, 142, 255);
            } 
        }
    }
}