using System;
using System.Collections.Generic;

namespace Model
{
    public class Game
    {
        private IViewer Viewer {get; set;}
        private Player _topPlayer;
        private Player _bottomPlayer;
        private Player _currentPlayer;
        private Player _otherPlayer;
        private GameState _gameState;
        private readonly Board _board;
        private static readonly object SyncRoot = new object();
        public Game(bool withBot, IViewer viewer)
        {
            Viewer = viewer;
            _board = new Board();
           NewGame(withBot);
        }

        public void NewGame(bool withBot)
        {
            _board.NewBoard();
            var topStartPosition = _board.TopStartPosition();
            _topPlayer = withBot ? new Bot(Color.Green, topStartPosition) : new Player(Color.Green, topStartPosition);
            var bottomStartPosition = _board.BottomStartPosition();
            _bottomPlayer = new Player(Color.Red, bottomStartPosition);
            _currentPlayer = _bottomPlayer;
            _otherPlayer = _topPlayer;
            var topWinningCells = _board.TopWinningCells();
            var bottomWinningCells = _board.BottomWinningCells();
            _gameState = new GameState(topWinningCells, bottomWinningCells);
        }
        private void ChangeCurrentPlayer()
        {
            if(_currentPlayer == _bottomPlayer)
            {
                _otherPlayer = _bottomPlayer;
                _currentPlayer = _topPlayer;
            }
            else
            {
                _otherPlayer = _topPlayer;
                _currentPlayer = _bottomPlayer;
            }
        }
        
        private bool CheckWinning()
        {
            return _currentPlayer == _bottomPlayer ? _gameState.CheckBottomWinning(_bottomPlayer) : 
                _gameState.CheckTopWinning(_topPlayer);
        }

        private void RenderPlayer(int top, int left)
        {
            if (_currentPlayer == _bottomPlayer)
            {
                Viewer.RenderBottomPlayer(top, left);
            }
            else
            {
                Viewer.RenderUpperPlayer(top, left);
            }
        }

        public bool MakeMove(Cell cell)
        {
            if (!MoveValidator.IsValidMove(cell, _currentPlayer, _otherPlayer)) return false;
            _board.MovePlayer(_currentPlayer, cell);
            var playerCoords = _currentPlayer.CurrentCell.Coords;
            RenderPlayer(playerCoords.Top, playerCoords.Left);
            CheckWinning();
            ChangeCurrentPlayer();
            if (!_gameState.InPlay)
            {
                Viewer.RenderEnding(_gameState.Winner.Color);
            }
            return true;
        }

        public bool PlaceWall(Wall wall)
        {
            if (!_board.CanBePlaced(wall) || !_currentPlayer.PlaceWall()) return false;
            _board.PutWall(wall);
            if (MoveValidator.IsThereAWay(_gameState, _topPlayer, _bottomPlayer))
            {
                var wallCoords = wall.Coords;
                Viewer.RenderWall(wallCoords.Top, wallCoords.Left);
                Viewer.RenderRemainingWalls(_topPlayer.WallsCount, _bottomPlayer.WallsCount);
                ChangeCurrentPlayer();
                return true;
            }

            _board.DropWall(wall);
            _currentPlayer.UnPlaceWall();
            

            return false;
        }
        
        
    }
}