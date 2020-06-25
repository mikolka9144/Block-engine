﻿using Engine.Engine.models;
using PixBlocks.PythonIron.Tools.Game;
using PixBlocks.PythonIron.Tools.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Engine.Engine
{
    public class Engine : IDrawer, ITileManager
    {
        public SpriteOverlay Center;

        public Engine(int timeout)
        {
            BlockAddDelay = timeout;
            Center = new Center(this);
        }

        public Pointer GetPointer()
        {
            Pointer pointer = new Pointer(this);
            this.Sprites.Add(pointer);
            return pointer;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000022FC File Offset: 0x000004FC
        public void LoadMap(string MapData)
        {
            List<string> list = MapData.Split(new char[]
            {
                '#'
            }).ToList<string>();
            string[] array = list[0].Split(new char[]
            {
                '*'
            });
            var playerPos = (Convert.ToInt32(array[0]), Convert.ToInt32(array[1]));
            list.RemoveAt(0);
            foreach (string text in list)
            {
                string[] array2 = text.Split(new char[]
                {
                    '*'
                });
                int num = Convert.ToInt32(array2[0]);
                int x = Convert.ToInt32(array2[1]);
                int y = Convert.ToInt32(array2[2]);
                int id = Convert.ToInt32(array2[3]);
                int num2 = Convert.ToInt32(array2[4]);
                bool flag = num <= -1;
                if (flag)
                {
                    this.AddBlockTile(x, y, id, num2, false);
                }
                else
                {
                    this.Add(new SpriteOverlay(new Sprite
                    {
                        image = num,
                        size = num2
                    }, x, y, id, this));
                }
            }
            this.MoveScene(playerPos.Item1, playerPos.Item2);
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002444 File Offset: 0x00000644
        public void CreateGenerator(int seed, int size)
        {
            var generator = new Generator(seed,this);
            generator.GenerateTerrian(size);
            generator.CreateUnderGround(size);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x0000246C File Offset: 0x0000066C
        public void RemoveTile(Block tile)
        {
            GameScene.gameSceneStatic.remove(tile.Sprite);
            GameScene.gameSceneStatic.remove(tile.foliage.Sprite);
            this.Blocks.Remove(tile);
            this.Toppings.Remove(tile.foliage);
        }



        // Token: 0x06000016 RID: 22 RVA: 0x0000258D File Offset: 0x0000078D
        private void MoveScene(int X, int Y)
        {
            this.Move(roation.Right, X);
            this.Move(roation.Down, Y);
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000025BE File Offset: 0x000007BE
        public void Add(SpriteOverlay sprite)
        {
            this.Sprites.Add(sprite);
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000025D0 File Offset: 0x000007D0
        public void AddBlockTile(int X, int Y, int Id, int size, bool SholdDraw)
        {
            Block block = new Block(X, Y, Id, size, this, IdProcessor);
            this.Blocks.Add(block);
            Toppings.Add(block.foliage);
            if (SholdDraw)
            {
                Draw(block);
                this.Draw(block.foliage);
            }
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002628 File Offset: 0x00000828
        public void Draw(SpriteOverlay sprite)
        {
            bool flag = sprite.X > border || sprite.X < -border || sprite.Y > border || sprite.Y < -border;
            if (flag)
            {
                sprite.Sprite.IsVisible = false;
                sprite.IsRendered = false;
            }
            else
            {
                sprite.Sprite.position = new PixBlocks.PythonIron.Tools.Integration.Vector(sprite.X, sprite.Y);
                bool flag2 = !sprite.Sprite.IsVisible;
                if (flag2)
                {
                    this.addSpriteToGame(sprite);
                }
                
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000026D8 File Offset: 0x000008D8
        private void addSpriteToGame(SpriteOverlay sprite)
        {
            if (!sprite.IsRendered)
            {
                Thread.Sleep(BlockAddDelay);
                GameScene.gameSceneStatic.add(sprite.Sprite);
                sprite.IsRendered = true;
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x0000271C File Offset: 0x0000091C
        public void Move(roation roation, int lenght)
        {
            foreach (Block block in this.Blocks)
            {
                block.Move(roation, lenght);
            }
            foreach (SpriteOverlay spriteOverlay in this.Sprites)
            {
                spriteOverlay.Move(roation, lenght);
            }
            foreach (Foliage foliage in this.Toppings)
            {
                foliage.Move(roation, lenght);
            }
            this.Center.Move(roation, lenght);
        }
        private bool IsActiveBlock(SpriteOverlay sprite)
        {
            var isActive = sprite.IsRendered;
            var IsNotInRange = sprite.X > hitboxArea || sprite.X < -hitboxArea || sprite.Y > hitboxArea || sprite.Y < hitboxArea;
            return isActive && IsNotInRange;
        }

        public List<Block> Blocks = new List<Block>();
        public List<Block> ActiveBlocks => Blocks.FindAll(s => IsActiveBlock(s)).ToList();

        public List<Foliage> Toppings = new List<Foliage>();
        public List<Foliage> ActiveToppings => Toppings.FindAll(s => IsActiveBlock(s)).ToList();

        public BlockIdProcessor IdProcessor = new BlockIdProcessor();
        public List<SpriteOverlay> Sprites = new List<SpriteOverlay>();

        private int BlockAddDelay;
        private const int border = 100;
        private const int hitboxArea = 20;
        
    }
}