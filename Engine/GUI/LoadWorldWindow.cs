﻿using Engine.Saves;
using System;
using System.Windows.Forms;

namespace Engine.GUI
{
    public partial class LoadWorldWindow : Form
    {
        public LoadWorldWindow(SaveManager manager, IInit init)
        {
            InitializeComponent();
            Manager = manager;
            Init = init;
        }

        public SaveManager Manager { get; }
        public IInit Init { get; }

        private void button1_Click(object sender, EventArgs e)
        {
            var save = Manager.LoadFromFile(txtBase.Text);
            Manager.LoadSave(save);
            Init.IsWorldGenerated = true;
            Close();
        }
    }
}