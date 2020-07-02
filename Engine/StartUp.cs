﻿using Engine.Engine;
using Engine.Engine.models;
using Engine.GUI;
using Engine.Saves;
using Logic;
using PixBlocks.PythonIron.Tools.Game;
using PixBlocks.PythonIron.Tools.Integration;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Logic
{
    public class StartUp:IInit
    {
        public bool IsWorldGenerated { get; set; } = false;
        private Engine.Engine engine;
        private Parameters parameters;
        private TileManager tileManager;

        public void Init()
        {
            parameters = new Parameters();
            var drawer = new Drawer(parameters);
            var StatusWindow = new StatusDisplay(parameters);
            var IdProcessor = new BlockIdProcessor();
            tileManager = new TileManager(parameters, drawer, IdProcessor);
            engine = new Engine.Engine(parameters,tileManager,drawer);
            var blockConverter = new BlockConverter(parameters, drawer, IdProcessor);
            var playerstatus = new PlayerStatus(parameters, StatusWindow);
            var game = GameScene.gameSceneStatic;
            var moveDefiner = new PlayerMoveDefiner();
            var SaveManager = new SaveManager(tileManager, playerstatus,blockConverter,engine.Center,engine);
            var pauseMenu = new PauseMenu(parameters,SaveManager);
            var pointer = new Pointer(drawer,parameters);
            var pointerController = new PointerController(playerstatus,pointer, tileManager,moveDefiner,parameters);
            var player = new Player(pauseMenu,parameters,tileManager,engine,pointerController,moveDefiner,playerstatus);

            var MainMenu = new Main_Menu(this,parameters,SaveManager);
            MainMenu.ShowDialog();
            if (!IsWorldGenerated) return;
            engine.Add(pointer);
            game.background = new Color(102, 51, 204);
            game.add(pointerController);
            game.add(pointer.Sprite);
            game.add(player);
            game.start();
        }

        public void GenerateWorld(int seed, int size, ProgressBar progress)
        {
            IsWorldGenerated = true;
            var oreTable = new OreTable();

            var generator = new Generator(seed, tileManager, parameters, oreTable,size);
            ExecuteGeneration(generator,progress);

        }
        private void ExecuteGeneration(Generator generator,ProgressBar progress)
        {
            generator.GenerateTerrian();
            progress.Value = 25;
            generator.CreateUnderGround();
            progress.Value = 50;
            generator.GenerateTrees();
            progress.Value = 75;
            generator.GenerateOres(BlockType.CoalOre);
            generator.GenerateOres(BlockType.IronOre);
            generator.GenerateOres(BlockType.GoldOre);
            generator.GenerateOres(BlockType.DiamondOre);
            progress.Value = 100;
        }
    }
}
