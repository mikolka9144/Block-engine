﻿using Engine.Engine;
using Engine.Engine.models;
using Engine.GUI;
using Engine.Logic.models;
using Engine.Resources;

using PixBlocks.PythonIron.Tools.Game;

using System.Windows.Forms;

namespace Engine.Logic
{
    internal class Player : MovableObject 
    {
        private PauseMenu settingsForm;

        public IMover Mover { get; }

        private Center center;

        public Player(PauseMenu pauseMenu, IActiveElements activeElements, PointerController pointer, IMoveDefiner definer, PlayerStatus status,IDrawer drawer,IMover mover,Center center) : base(activeElements, drawer, definer, pointer, status)
        {
            position = new PixBlocks.PythonIron.Tools.Integration.Vector(0, 0);
            size = 10;
            image = 0;
            status.OnKill = KillPlayer;
            settingsForm = pauseMenu;
            Mover = mover;
            this.center = center;
        }
       

        public override void update()
        {
            base.update();
            if (moveDefiner.key(command.Pause)) Pause();
            if (moveDefiner.key(command.OpenInventory)) status.OpenInventory();
            MoveCamera();
        }

        private void MoveCamera()
        {
            if (X != 0)
            {
                Mover.Move(roation.Left, X);
                X = 0;
            }
            if (Y != 0)
            {
                Mover.Move(roation.Down, Y);
                Y = 0;
            }
        }

        private void Pause()
        {
            settingsForm.ShowDialog();
        }

        private void KillPlayer()
        {
            GameScene.gameSceneStatic.stop();
            MessageBox.Show("You Died.");
        }
    }
}