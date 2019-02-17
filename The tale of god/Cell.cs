using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;
using TheTaleOfGod.enemies;

namespace TheTaleOfGod
{
    [ProtoContract]
    public class Cell
    {
        public static Cell[] CellsOnScreen { get; set; }

        public static Texture2D CellSprite { get; set; }
        public static Vector2 SpriteOrigin { get; set; }

        [ProtoMember(1)]
        public int x;
        [ProtoMember(2)]
        public int y;

        /// <summary>
        /// enemies contained in this cell
        /// </summary>
        public List<Enemy> enemies = new List<Enemy>(); // look over if this is using an unreasonable amount of ram
        public List<SceneObject> objects = new List<SceneObject>();
        public List<Collider> colliders = new List<Collider>();

        public static Point startingPoint;

        public static Cell[,] grid;
        public static readonly int cellWidth = 32, cellHeight = 32;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Cell()
        {

        }

        public void Draw(SpriteBatch batch, Color color)
        {
            batch.Draw(CellSprite, ToVector2(), null, color, 0f, SpriteOrigin, 1f, SpriteEffects.None, 0.99f);
        }

        #region gridfunctions
        public static void CreateGrid(Point startingPoint, int rows, int columns)
        {
            Cell.startingPoint = startingPoint;
            grid = new Cell[rows, columns];
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    grid[x, y] = new Cell(x * cellWidth + startingPoint.X, y * cellHeight + startingPoint.Y);
                }
            }
            CellSprite = DebugTextures.GenerateHollowRectangele(cellWidth, cellHeight, 1, Color.White);
            SpriteOrigin = new Vector2(cellWidth/2f, cellHeight/2f);
        }

        public static Vector2 SnapToGrid(Vector2 position)
        {
            float x = (float)Math.Round(position.X / (float)cellWidth) * cellWidth;
            float y = (float)Math.Round(position.Y / (float)cellHeight) * cellHeight;

            return new Vector2(x, y);
        }
        public static Vector2 SnapToGridNoRound(Vector2 position)
        {
            float x = position.X % cellWidth;
            float y = position.Y % cellHeight;

            Vector2 pos = position;
            pos.X -= x;
            pos.Y -= y;
            return pos;
        }
        public static Cell GetCell(Vector2 position)
        {
            position = SnapToGrid(position);

            return grid[((int)position.X - startingPoint.X) / cellWidth, ((int)position.Y - startingPoint.Y) / cellHeight];
        }
        public static Cell GetCell(int x, int y)
        {
            return GetCell(new Vector2(x, y));
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }

        #region Cells Nearby

        public static Cell GetCell(Cell cell, int offsetX, int offsetY)
        {
            return grid[(cell.x - startingPoint.X) / cellWidth + offsetX, (cell.y - startingPoint.Y) / cellHeight + offsetY];
        }

        public static void SetGridCell(Cell cell) // do i have to clone it??
        {
            grid[(cell.x - startingPoint.X) / cellWidth, (cell.y - startingPoint.Y) / cellHeight] = cell;
        }

        public static Cell[] GetSurroundingCells(Cell cell)
        {
            return new Cell[] { GetCell(cell, 1, 0), GetCell(cell, -1, 0), GetCell(cell, 0, 1), GetCell(cell, 0, -1), GetCell(cell, 1, 1), GetCell(cell, -1, 1), GetCell(cell, 1, -1), GetCell(cell, -1, -1) };
        }
        public static Cell[] GetSurroundingCells(Cell topLeftcell, int width, int height)
        {
            List<Cell> cells = new List<Cell>();

            for (int y = 0; y < height; y++)
            {
                Cell cell01 = GetCell(topLeftcell, width, y);
                cells.Add(cell01);
                Cell cell02 = GetCell(topLeftcell, -1, y);

                cells.Add(cell02);
            }
            for (int x = 0; x < width; x++)
            {
                Cell cell01 = GetCell(topLeftcell, x, height);
                cells.Add(cell01);
                Cell cell02 = GetCell(topLeftcell, x, -1);
                if (cell02 != null)
                {
                    cells.Add(cell02);
                }
            }
            Cell[] cel;
            cel = cells.Where(c => c != null).ToArray();
            return cel;
        }
        public static Cell[] GetAreaOfCells(Cell origin, int width, int height)
        {
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            Cell startPoint = GetCell(origin, -halfWidth, -halfHeight);
            Cell[] cells = new Cell[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[y * width + x] = GetCell(startPoint.x + (x * cellWidth), startPoint.y + (y * cellHeight));
                }
            }
            return cells;
        }
        public static Cell[] GetAreaOfCellsTopLeft(Cell topLeft, int width, int height)
        {
            Cell[] cells = new Cell[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[y * width + x] = GetCell(topLeft.x + (x * cellWidth), topLeft.y + (y * cellHeight));
                }
            }
            return cells;
        }
        #endregion
        public static Cell[] GetCellsOnScreen(Vector2 cameraPos, int screenWidth, int screenHeight)
        {
            int shellWidth = 2;
            int shellHeight = 2;
            Vector2 camCornerPos = new Vector2(cameraPos.X - (screenWidth / 2f) - cellWidth, cameraPos.Y - (screenHeight / 2f) - ((shellHeight / 2) * cellHeight));
            Vector2 camCellPos = SnapToGrid(camCornerPos);
            Cell camCell = GetCell(camCellPos);

            if (camCell == null)
            {
                return null;
            }

            float widthRest = screenWidth % cellWidth;

            int width = screenWidth + (cellWidth - (int)widthRest);
            int xCells = (width / cellWidth) + shellWidth;

            float heightRest = screenHeight % cellHeight;

            int height = screenHeight + (cellHeight - (int)heightRest);
            int yCells = (height / cellHeight) + shellHeight;

            Cell[] cells = new Cell[xCells * yCells];

            for (int y = 0; y < yCells; y++)
            {
                for (int x = 0; x < xCells; x++)
                {
                    int arrayIndex = xCells * y + x;

                    cells[arrayIndex] = grid[(camCell.x - startingPoint.X) / cellWidth + x, (camCell.y - startingPoint.Y) / cellWidth + y];
                }
            }
            return cells;
        }
        public static void UpdateCellsOnScreen(Vector2 cameraPos, int screenWidth, int screenHeight)
        {
            CellsOnScreen = GetCellsOnScreen(cameraPos, screenWidth, screenHeight);
        }
        public static Collider[] GetNearbyColliders(Vector2 origin, int width, int height)
        {
            Cell[] nearbyCells = GetAreaOfCells(GetCell(origin), width, height);
            List<Collider> colls = new List<Collider>();
            foreach (var c in nearbyCells)
            {
                if (c.colliders.Count > 0)
                {
                    foreach (var col in c.colliders)
                    {
                        colls.Add(col);
                    }
                }
            }
            return colls.ToArray();
        }
        public static void DrawGrid(SpriteBatch batch)
        {
            foreach (var cell in CellsOnScreen)
            {
                batch.Draw(CellSprite, cell.ToVector2(), null, Color.IndianRed, 0f, SpriteOrigin, 1f, SpriteEffects.None, 0.9f);
            }
        }
        #endregion
    }
}