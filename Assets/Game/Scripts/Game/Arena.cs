using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour {
    [SerializeField] private int requiredTilesInLine = 3;
    private enum tilesCompareState { EMPTY, DIFFERENT, SAME }   //Used to prettier comparison in looking for points
    private ArenaTile[,] _tile;
    private List<ArenaTile> tileList;   //Used to found empty tiles
    private int maxX, maxY;
    private Game game;
    
    
    public ArenaTile[,] tile { get { return _tile; } private set { _tile = value; } }

	void Awake () {
        maxX = transform.GetChild(0).childCount;
        maxY = transform.childCount;
        tile = new ArenaTile[maxX, maxY];
        tileList = new List<ArenaTile>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();

        for(int y = 0; y < maxY; ++y) {
            for(int x = 0; x < maxX; ++x) {
                tile[x, y] = transform.GetChild(y).GetChild(x).GetComponent<ArenaTile>();
                tileList.Add(tile[x, y]);
            }
        }
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) checkPoints();
    }

    public List<ArenaTile> getEmptyTiles() {
        Debug.Log(string.Format("Found {0} empty tiles.", tileList.FindAll(b => b.empty).Count));

        return tileList.FindAll(b => b.empty);
    }

    public void removeAllTiles() {
        Debug.Log("Remove all tiles on arena");

        foreach(var obj in tileList.FindAll(b => !b.empty)) {
            obj.tile.remove();
        }
    }

    public void checkPoints(bool spawnCheck = false) {  //spawnCheck - avoid spawning new tiles without player move (from tile spawner)
        bool pointsRow = checkPointsRow();
        bool pointsCol = checkPointsColumn();
        bool pointsMainDiagonal = checkPointsMainDiagonal();
        bool pointsSubDiagonal = checkPointsSubDiagonal();

        if (!spawnCheck && !pointsRow && !pointsCol && !pointsMainDiagonal && !pointsSubDiagonal) game.spawner.spawn();
    }

    private bool checkPointsRow() {
        bool achievedPoints = false;

        for (int y = 0; y < maxY; ++y) {
            int sameColor = 1, start = 0;
            
            for (int x = 1; x < maxX; ++x) {
                tilesCompareState compState = compareTiles(tile[x, y], tile[x - 1, y]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if(x == maxX - 1 && sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeRow(y, start, x);
                    }
                }
                else {
                    if (sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeRow(y, start, x - 1);
                    }

                    sameColor = 1;
                    start = x;
                }
            }
        }

        return achievedPoints;
    }

    private bool checkPointsColumn() {
        bool achievedPoints = false;

        for (int x = 0; x < maxX; ++x) {
            int sameColor = 1, start = 0;

            for (int y = 1; y < maxY; ++y) {
                tilesCompareState compState = compareTiles(tile[x, y], tile[x, y - 1]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if (y == maxY - 1 && sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeColumn(x, start, y);
                    }
                }
                else {
                    if (sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeColumn(x, start, y - 1);
                    }

                    sameColor = 1;
                    start = y;
                }
            }
        }

        return achievedPoints;
    }

    private bool checkPointsMainDiagonal() {
        bool achievedPoints = false;

        for (int y = 0; y < maxY; ++y) {
            int sameColor = 1, startX = 0, startY = y, flag = y;
            
            for (int x = 1; x < maxX - y; ++x) {
                ++flag;

                if (flag >= maxX) {
                    break;
                }

                tilesCompareState compState = compareTiles(tile[x, flag], tile[x - 1, flag - 1]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if(x == maxX - y - 1 && sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeMainDiagonal(sameColor, startX, startY);
                    }
                }
                else {
                    if (sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeMainDiagonal(sameColor, startX, startY);
                    }

                    sameColor = 1;
                    startX = x;
                    startY = flag;
                }
            }
        }

        for (int x = 1; x < maxX; ++x) {
            int sameColor = 1, startY = 0, startX = x, flag = x;


            for (int y = 1; y < maxY - x; ++y) {
                ++flag;

                if (flag >= maxY) {
                    break;
                }
                tilesCompareState compState = compareTiles(tile[flag, y], tile[flag - 1, y - 1]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if(y == maxY - x - 1 && sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeMainDiagonal(sameColor, startX, startY);
                    }
                }
                else {
                    if (sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeMainDiagonal(sameColor, startX, startY);
                    }

                    sameColor = 1;
                    startX = flag;
                    startY = y;
                }
            }
        }

        return achievedPoints;
    }

    private bool checkPointsSubDiagonal() {
        bool achievedPoints = false;

        for (int y = 0; y < maxY; ++y) {
            int sameColor = 1, startX = 0, startY = y, flag = y;
            
            for (int x = 1; x < y + 1; ++x) {
                --flag;

                if (flag < 0) {
                    break;
                }

                tilesCompareState compState = compareTiles(tile[x, flag], tile[x - 1, flag + 1]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if(x == y  && sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeSubDiagonal(sameColor, startX, startY);
                    }
                }
                else {
                    if (sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeSubDiagonal(sameColor, startX, startY);
                    }

                    sameColor = 1;
                    startX = x;
                    startY = flag;
                }
            }
        }

        for (int x = 1; x < maxX; ++x) {
            int sameColor = 1, startX = x, startY = maxY - 1, flag = x;
            
            for (int y = maxY - 2; y >= x; --y) {
                ++flag;

                if (flag >= maxX) {
                    break;
                }

                tilesCompareState compState = compareTiles(tile[flag, y], tile[flag - 1, y + 1]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if(y == x && sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeSubDiagonal(sameColor, startX, startY);
                    }
                }
                else {
                    if (sameColor >= requiredTilesInLine) {
                        achievedPoints = true;
                        removeSubDiagonal(sameColor, startX, startY);
                    }

                    sameColor = 1;
                    startX = flag;
                    startY = y;
                }
            }
        }

        return achievedPoints;
    }

    private tilesCompareState compareTiles(ArenaTile A, ArenaTile B) {
        if (A.empty || B.empty) return tilesCompareState.EMPTY;
        else if (A.tile.color == B.tile.color) return tilesCompareState.SAME;
        else return tilesCompareState.DIFFERENT;
    }

    private void removeRow(int row, int start, int end) {
        Debug.Log(string.Format("Remove row: {0}, start: {1}, end: {2}, total tiles: {3}", row + 1, start + 1, end + 1, end - start + 1));
        
        game.addPoints(Mathf.Abs(requiredTilesInLine - (end - start + 1)));

        for(int i = start; i <= end; ++i) {
            if (tile[i, row].tile) {
                tile[i, row].tile.remove();
            }
        } 
    }

    private void removeColumn(int col, int start, int end) {
        Debug.Log(string.Format("Remove column: {0}, start: {1}, end: {2}, total tiles: {3}", col + 1, start + 1, end + 1, end - start + 1));

        game.addPoints(Mathf.Abs(requiredTilesInLine - (end - start + 1)));

        for (int i = start; i <= end; ++i) {
            if (tile[col, i].tile) {
                tile[col, i].tile.remove();
            }
        } 
    }

    private void removeMainDiagonal(int count, int startX, int startY) {
        game.addPoints(Mathf.Abs(requiredTilesInLine - count));
        for(int i = 0; i < count; ++i) {
            if (tile[startX + i, startY + i].tile) {
                tile[startX + i, startY + i].tile.remove();
            }
        } 
    }

    private void removeSubDiagonal(int count, int startX, int startY) {
        game.addPoints(Mathf.Abs(requiredTilesInLine - count));
        for(int i = 0; i < count; ++i) {
            if(tile[startX + i, startY - i].tile) {
                tile[startX + i, startY - i].tile.remove();
            }
        } 
    }
}
