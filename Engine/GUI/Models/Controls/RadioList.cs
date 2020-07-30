﻿using PixBlocks.PythonIron.Tools.Integration;
using System.Collections.Generic;
using System.Linq;

namespace Engine.GUI.Models
{
    class RadioList:PixControl
    {
        public List<IndexedButton> radios { get; }
        public int Selection { get; set; }
        public RadioList(Vector vector)
        {
            position = vector;
            size = 0;
            radios = new List<IndexedButton>();
            
        }
        public void Initalize(IList<RadioTemplate> controls)
        {
            radios.Clear();
            var Ypos = position.y;
            var Xpos = position.x;
            for (int i = 0; i < controls.Count(); i++)
            {
                if (Ypos < -70) 
                { 
                    Xpos -= 40;
                    Ypos = position.y;
                }
                var radio = new IndexedButton(new Vector(position.x, Ypos), controls[i].Text,30, changeSelection, i);
                radios.Add(radio);
                Ypos -= 30;
            }
            if (radios.Count == 0) return;
            
        }
        private void changeSelection(PixControl obj)
        {
            radios.ForEach(s => s.Active = false);
            var radio = obj as IndexedButton;
            radio.Active = true;
            Selection = radio.Index;
        }
        public override void Show()
        {
            radios.ForEach(s => s.Show());
        }
        public override void Hide()
        {
            radios.ForEach(s => s.Hide());
        }
    }

    public struct RadioTemplate
    {
        public RadioTemplate(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}