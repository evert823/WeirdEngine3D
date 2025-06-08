using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TheWeirdEngine
{
    public struct cornerInfo
    {
        public location cornercoord;
        public int DistanceToKing;
        public bool BishopCanAttack;
    }
    public class WeirdEngineBareKingMate
    {
        public WeirdEngineMoveFinder MyWeirdEngineMoveFinder;
        public WeirdEngineBareKingMate(WeirdEngineMoveFinder pWeirdEngineMoveFinder)
        {
            this.MyWeirdEngineMoveFinder = pWeirdEngineMoveFinder;
        }
        public int DistanceBetweenSquares(int i1, int j1, int i2, int j2, int z1, int z2)
        {
            return MyWeirdEngineMoveFinder.MyBoardTopology.DistanceBetweenSquares[i1, j1, i2, j2, z1, z2];
        }

        public cornerInfo CheckOneCorner(ref chessposition pposition, int i, int j, int z, location targetkingcoord)
        {
            cornerInfo myresult;
            myresult.cornercoord.x = i;
            myresult.cornercoord.y = j;
            myresult.cornercoord.z = z;
            myresult.DistanceToKing = DistanceBetweenSquares(i, j, z,
                                            targetkingcoord.x, targetkingcoord.y, targetkingcoord.z);
            myresult.BishopCanAttack = false;
            if (pposition.WhiteBareKing == true)
            {
                if (MyWeirdEngineMoveFinder.MyBoardTopology.IsWhiteSquare[i, j, z] == true)
                {
                    if (pposition.BlackBishoponWhite == true) { myresult.BishopCanAttack = true; }
                }
                else
                {
                    if (pposition.BlackBishoponBlack == true) { myresult.BishopCanAttack = true; }
                }
            }
            else
            {
                if (MyWeirdEngineMoveFinder.MyBoardTopology.IsWhiteSquare[i, j, z] == true)
                {
                    if (pposition.WhiteBishoponWhite == true) { myresult.BishopCanAttack = true; }
                }
                else
                {
                    if (pposition.WhiteBishoponBlack == true) { myresult.BishopCanAttack = true; }
                }
            }
            return myresult;
        }
        public cornerInfo PickCorner(ref chessposition pposition, location targetkingcoord)
        {
            //Output: x, y, distance, bishop-aligned

            int bestci;

            cornerInfo myresult;

            cornerInfo[] AllCorners = new cornerInfo[8];
            AllCorners[0] = CheckOneCorner(ref pposition, 0, 0, 0, targetkingcoord);
            AllCorners[1] = CheckOneCorner(ref pposition, pposition.boardwidth - 1, 0, 0, targetkingcoord);
            AllCorners[2] = CheckOneCorner(ref pposition, 0, pposition.boardheight - 1, 0, targetkingcoord);
            AllCorners[3] = CheckOneCorner(ref pposition, pposition.boardwidth - 1, pposition.boardheight - 1, 0, targetkingcoord);
            AllCorners[4] = CheckOneCorner(ref pposition, 0, 0, pposition.depth_3d - 1, targetkingcoord);
            AllCorners[5] = CheckOneCorner(ref pposition, pposition.boardwidth - 1, 0, pposition.depth_3d - 1, targetkingcoord);
            AllCorners[6] = CheckOneCorner(ref pposition, 0, pposition.boardheight - 1, pposition.depth_3d - 1, targetkingcoord);
            AllCorners[7] = CheckOneCorner(ref pposition, pposition.boardwidth - 1, pposition.boardheight - 1, pposition.depth_3d - 1, targetkingcoord);

            bestci = 0;
            for (int ci = 1; ci < 8; ci++)
            {
                if (AllCorners[ci].BishopCanAttack == true & AllCorners[bestci].BishopCanAttack == false)
                {
                    bestci = ci;
                }
                else if (AllCorners[ci].BishopCanAttack == AllCorners[bestci].BishopCanAttack
                       & AllCorners[ci].DistanceToKing < AllCorners[bestci].DistanceToKing)
                {
                    bestci = ci;
                }
            }

            myresult = AllCorners[bestci];
            return myresult;
        }
        public double MateBareKing(ref chessposition pposition)
        {
            //Handle the position where one has bare King and the other has mating material
            //(This is already validated before we enter here)
            location targetkingcoord;
            int sumofsquareddistances;
            int numberofchasingpieces;
            int d;
            double AvgD;
            double AvgD2;

            if (pposition.WhiteBareKing == true)
            {
                targetkingcoord = pposition.whitekingcoord;
            }
            else
            {
                targetkingcoord = pposition.blackkingcoord;
            }

            cornerInfo bestcorner = PickCorner(ref pposition, targetkingcoord);

            sumofsquareddistances = 0;
            numberofchasingpieces = 0;
            for (int z = 0; z < pposition.depth_3d; z++)
            {
                for (int i = 0; i < pposition.boardwidth; i++)
                {
                    for (int j = 0; j < pposition.boardheight; j++)
                    {
                        if (pposition.squares[i, j, z] != 0)
                        {
                            if ((pposition.WhiteBareKing == true & pposition.squares[i, j, z] < 0) ||
                                (pposition.BlackBareKing == true & pposition.squares[i, j, z] > 0))
                            {
                                d = DistanceBetweenSquares(i, j, z, targetkingcoord.x, targetkingcoord.y, targetkingcoord.z);
                                numberofchasingpieces += 1;
                                sumofsquareddistances += (d * d);
                            }
                        }
                    }
                }
            }
            AvgD = sumofsquareddistances / numberofchasingpieces;
            AvgD2 = ((bestcorner.DistanceToKing * bestcorner.DistanceToKing) + AvgD) / 2;
            double MaxAvgD2 = ((double)pposition.boardheight * (double)pposition.boardheight) +
                              ((double)pposition.boardwidth * (double)pposition.boardwidth);

            double score = 95 - (AvgD2 * (15 / MaxAvgD2));
            if (score >= 94.9) { score = 94.9; }
            if (score <= 80.1) { score = 80.1; }

            if (pposition.BlackBareKing == true)
            {
                return score;
            }
            if (pposition.WhiteBareKing == true)
            {
                return -score;
            }
            return 0.0;
        }

    }
}
