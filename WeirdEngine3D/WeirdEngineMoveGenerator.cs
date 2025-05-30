using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TheWeirdEngine
{
    public class WeirdEngineMoveGenerator
    {
        public WeirdEngineMoveFinder MyWeirdEngineMoveFinder;
        public WeirdEngineMoveGenerator(WeirdEngineMoveFinder pWeirdEngineMoveFinder)
        {
            this.MyWeirdEngineMoveFinder = pWeirdEngineMoveFinder;
        }
        public bool WhiteKingIsInCheck(ref chessposition pposition)
        {
            if (pposition.colourtomove == 1)
            {
                return PMKingIsInCheck(ref pposition);
            }
            else
            {
                return POKingIsInCheck(ref pposition);
            }
        }
        public bool BlackKingIsInCheck(ref chessposition pposition)
        {
            if (pposition.colourtomove == 1)
            {
                return POKingIsInCheck(ref pposition);
            }
            else
            {
                return PMKingIsInCheck(ref pposition);
            }
        }
        public bool PMKingIsInCheck(ref chessposition pposition)
        {
            if (pposition.colourtomove == 1)
            {
                if (pposition.squareInfo[pposition.whitekingcoord.x, pposition.whitekingcoord.y,
                    pposition.whitekingcoord.z].AttackedByPO > 0)
                {
                    return true;
                }
            }
            else
            {
                if (pposition.squareInfo[pposition.blackkingcoord.x, pposition.blackkingcoord.y,
                    pposition.blackkingcoord.z].AttackedByPO > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool POKingIsInCheck(ref chessposition pposition)
        {
            if (pposition.colourtomove == 1)
            {
                if (pposition.squareInfo[pposition.blackkingcoord.x, pposition.blackkingcoord.y,
                    pposition.blackkingcoord.z].AttackedByPM > 0)
                {
                    return true;
                }
            }
            else
            {
                if (pposition.squareInfo[pposition.whitekingcoord.x, pposition.whitekingcoord.y,
                    pposition.whitekingcoord.z].AttackedByPM > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void MarkAttacked(ref chessposition pposition, int x, int y, int z, int pmovingpiece)
        {
            if (pmovingpiece > 0)
            {
                if (pposition.colourtomove == 1)
                {
                    pposition.squareInfo[x, y, z].AttackedByPM += 1;
                }
                else
                {
                    pposition.squareInfo[x, y, z].AttackedByPO += 1;
                }
            }
            else
            {
                if (pposition.colourtomove == 1)
                {
                    pposition.squareInfo[x, y, z].AttackedByPO += 1;
                }
                else
                {
                    pposition.squareInfo[x, y, z].AttackedByPM += 1;
                }
            }
        }
        public void AssignCapturedValue(chessposition pposition, ref chessmove mv, int i2, int j2, int z2)
        {
            int pti2 = MyWeirdEngineMoveFinder.pieceTypeIndex(pposition.squares[i2, j2, z2]);
            mv.CapturedValue = MyWeirdEngineMoveFinder.piecetypes[pti2].EstimatedValue;
        }
        public void Default_moveprioindex(ref chessposition pposition)
        {
            for (int movei = 0; movei < pposition.movelist_totalfound; movei++)
            {
                pposition.moveprioindex[movei] = movei;
            }
        }
        public void GetAttacksMoves(ref chessposition pposition, int depth, int prevposidx)
        {
            pposition.movelist_totalfound = 0;
            for (int z = 0; z < pposition.depth_3d; z++)
            {
                for (int i = 0; i < pposition.boardwidth; i++)
                {
                    for (int j = 0; j < pposition.boardheight; j++)
                    {
                        if (pposition.squares[i, j, z] != 0)
                        {
                            this.GetStepLeapAttacksMoves(ref pposition, i, j, z, depth);
                            this.GetSlideAttacksMoves(ref pposition, i, j, z, depth);
                        }
                        if (depth > 0)
                        {
                            if ((pposition.squares[i, j, z] > 0 & pposition.colourtomove > 0) ||
                                (pposition.squares[i, j, z] < 0 & pposition.colourtomove < 0))
                            {
                                GetPawn2StepMoves(ref pposition, i, j, z);
                            }
                        }
                    }
                }
            }
            Default_moveprioindex(ref pposition);
        }
        public void GetStepLeapAttacksMovesPerVector(ref chessposition pposition, int i, int j, int z, vector v,
                                                     bool getcaptures, bool getnoncaptures, int depth, int pti,
                                                     int pti_self)
        {
            int i2;
            int j2;
            int z2;
            int movei;
            i2 = i + v.x;
            if (pposition.squares[i, j, z] > 0)
            {
                j2 = j + v.y;
            }
            else
            {
                j2 = j - v.y;
            }
            z2 = z + v.z;
            if (i2 >= 0 & i2 < pposition.boardwidth & j2 >= 0 & j2 < pposition.boardheight
                & z2 >= 0 & z2 < pposition.depth_3d)
            {
                if (getcaptures == true)
                {
                    this.MarkAttacked(ref pposition, i2, j2, z2, pposition.squares[i, j, z]);
                    if (depth > 0)
                    {
                        if ((pposition.squares[i2, j2, z2] > 0 & pposition.squares[i, j, z] < 0 & pposition.colourtomove < 0) ||
                            (pposition.squares[i2, j2, z2] < 0 & pposition.squares[i, j, z] > 0 & pposition.colourtomove > 0))
                        {
                            movei = pposition.movelist_totalfound;
                            InitializeMove(ref pposition, movei, i, j, z, i2, j2, z2);
                            pposition.movelist[movei].MovingPiece = pposition.squares[i, j, z];
                            pposition.movelist[movei].IsCapture = true;
                            AssignCapturedValue(pposition, ref pposition.movelist[movei], i2, j2, z2);
                            GetPromotion(ref pposition, movei, pti, pti_self);
                        }
                    }
                }
                if (getnoncaptures == true & depth > 0)
                {
                    if ((pposition.squares[i2, j2, z2] == 0 & pposition.squares[i, j, z] < 0 & pposition.colourtomove < 0) ||
                        (pposition.squares[i2, j2, z2] == 0 & pposition.squares[i, j, z] > 0 & pposition.colourtomove > 0))
                    {
                        movei = pposition.movelist_totalfound;
                        InitializeMove(ref pposition, movei, i, j, z, i2, j2, z2);
                        pposition.movelist[movei].MovingPiece = pposition.squares[i, j, z];
                        GetPromotion(ref pposition, movei, pti, pti_self);
                    }
                }
            }
        }
        public void GetStepLeapAttacksMoves(ref chessposition pposition, int i, int j, int z, int depth)
        {
            int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(pposition.squares[i, j, z]);
            int pti_self = pti;

            if (MyWeirdEngineMoveFinder.piecetypes[pti].IsDivergent == false)
            {
                foreach (vector v in MyWeirdEngineMoveFinder.piecetypes[pti].stepleapmovevectors)
                {
                    GetStepLeapAttacksMovesPerVector(ref pposition, i, j, z, v, true, true, depth, pti, pti_self);
                }
            }
            else
            {
                foreach (vector v in MyWeirdEngineMoveFinder.piecetypes[pti].stepleapmovevectors)
                {
                    GetStepLeapAttacksMovesPerVector(ref pposition, i, j, z, v, false, true, depth, pti, pti_self);
                }
                foreach (vector v in MyWeirdEngineMoveFinder.piecetypes[pti].stepleapcapturevectors)
                {
                    GetStepLeapAttacksMovesPerVector(ref pposition, i, j, z, v, true, false, depth, pti, pti_self);
                }
            }
        }
        public void GetSlideAttacksMovesPerVector(ref chessposition pposition, int i, int j, int z, vector v,
                                                  bool getcaptures, bool getnoncaptures, int depth, int pti,
                                                  int pti_self)
        {
            int i2;
            int j2;
            int z2;
            int movei;
            bool blocked;

            i2 = i + v.x;
            if (pposition.squares[i, j, z] > 0)
            {
                j2 = j + v.y;
            }
            else
            {
                j2 = j - v.y;
            }
            z2 = z + v.z;

            blocked = false;
            while (i2 >= 0 & i2 < pposition.boardwidth & j2 >= 0 & j2 < pposition.boardheight
                   & z2 >= 0 & z2 < pposition.depth_3d & blocked == false)
            {
                if (getcaptures == true)
                {
                    this.MarkAttacked(ref pposition, i2, j2, z2, pposition.squares[i, j, z]);
                    if (depth > 0)
                    {
                        if ((pposition.squares[i2, j2, z2] > 0 & pposition.squares[i, j, z] < 0 & pposition.colourtomove < 0) ||
                            (pposition.squares[i2, j2, z2] < 0 & pposition.squares[i, j, z] > 0 & pposition.colourtomove > 0))
                        {
                            movei = pposition.movelist_totalfound;
                            InitializeMove(ref pposition, movei, i, j, z, i2, j2, z2);
                            pposition.movelist[movei].MovingPiece = pposition.squares[i, j, z];
                            pposition.movelist[movei].IsCapture = true;
                            AssignCapturedValue(pposition, ref pposition.movelist[movei], i2, j2, z2);
                            GetPromotion(ref pposition, movei, pti, pti_self);
                        }
                    }
                }
                if (getnoncaptures == true & depth > 0)
                {
                    if ((pposition.squares[i2, j2, z2] == 0 & pposition.squares[i, j, z] < 0 & pposition.colourtomove < 0) ||
                             (pposition.squares[i2, j2, z2] == 0 & pposition.squares[i, j, z] > 0 & pposition.colourtomove > 0))
                    {
                        movei = pposition.movelist_totalfound;
                        InitializeMove(ref pposition, movei, i, j, z, i2, j2, z2);
                        pposition.movelist[movei].MovingPiece = pposition.squares[i, j, z];
                        GetPromotion(ref pposition, movei, pti, pti_self);
                    }
                }
                if (pposition.squares[i2, j2, z2] != 0)
                {
                    blocked = true;
                }
                i2 = i2 + v.x;
                if (pposition.squares[i, j, z] > 0)
                {
                    j2 = j2 + v.y;
                }
                else
                {
                    j2 = j2 - v.y;
                }
                z2 = z2 + v.z;
            }
        }
        public void GetSlideAttacksMoves(ref chessposition pposition, int i, int j, int z, int depth)
        {
            int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(pposition.squares[i, j, z]);
            int pti_self = pti;

            if (MyWeirdEngineMoveFinder.piecetypes[pti].IsDivergent == false)
            {
                foreach (vector v in MyWeirdEngineMoveFinder.piecetypes[pti].slidemovevectors)
                {
                    GetSlideAttacksMovesPerVector(ref pposition, i, j, z, v, true, true, depth, pti, pti_self);
                }
            }
            else
            {
                foreach (vector v in MyWeirdEngineMoveFinder.piecetypes[pti].slidemovevectors)
                {
                    GetSlideAttacksMovesPerVector(ref pposition, i, j, z, v, false, true, depth, pti, pti_self);
                }
                foreach (vector v in MyWeirdEngineMoveFinder.piecetypes[pti].slidecapturevectors)
                {
                    GetSlideAttacksMovesPerVector(ref pposition, i, j, z, v, true, false, depth, pti, pti_self);
                }
            }
        }
        public void SynchronizeChessmove(chessmove frommove, ref chessmove tomove)
        {
            tomove.MovingPiece = frommove.MovingPiece;
            tomove.coordinates[0] = frommove.coordinates[0];
            tomove.coordinates[1] = frommove.coordinates[1];
            tomove.coordinates[2] = frommove.coordinates[2];
            tomove.coordinates[3] = frommove.coordinates[3];
            tomove.coordinates[4] = frommove.coordinates[4];
            tomove.coordinates[5] = frommove.coordinates[5];
            tomove.IsCapture = frommove.IsCapture;
            tomove.CapturedValue = frommove.CapturedValue;
            tomove.PromoteToPiece = frommove.PromoteToPiece;
            tomove.calculatedvalue = frommove.calculatedvalue;
            tomove.number_of_no_selfcheck_resp = frommove.number_of_no_selfcheck_resp;
        }
        public void InitializeMove(ref chessposition pposition, int movei, int pi1, int pj1, int pz1,
                                                                int pi2, int pj2, int pz2)
        {
            pposition.movelist[movei].MovingPiece = 0;
            pposition.movelist[movei].coordinates[0] = pi1;
            pposition.movelist[movei].coordinates[1] = pj1;
            pposition.movelist[movei].coordinates[2] = pz1;
            pposition.movelist[movei].coordinates[3] = pi2;
            pposition.movelist[movei].coordinates[4] = pj2;
            pposition.movelist[movei].coordinates[5] = pz2;
            pposition.movelist[movei].IsCapture = false;
            pposition.movelist[movei].CapturedValue = 0;
            pposition.movelist[movei].PromoteToPiece = 0;
        }
        public void DeleteLatestMoveIfDuplicate(ref chessposition pposition, int pti)
        {
            //Only because of duplication of vectors in inefficient piece definitions
            if (MyWeirdEngineMoveFinder.piecetypes[pti].CheckDuplicateMoves == false) { return; }

            bool IsDuplicateMove = false;
            int lmi = pposition.movelist_totalfound - 1;
            for (int movei = lmi - 1; movei >= 0; movei--)
            {
                if (MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.MovesAreEqual(pposition.movelist[movei], pposition.movelist[lmi]) == true)
                {
                    IsDuplicateMove = true;
                    break;
                }
            }
            if (IsDuplicateMove == true)
            {
                MyWeirdEngineMoveFinder.Init_chessmove(ref pposition.movelist[lmi]);
                pposition.movelist_totalfound = lmi;
            }
        }
        public void GetPromotion(ref chessposition pposition, int movei, int pti, int pti_self)
        {
            bool includepromote = false;
            bool includenonpromote = false;

            if (MyWeirdEngineMoveFinder.piecetypes[pti_self].SpecialPiece_ind == SpecialPiece.Pawn)
            {
                if (pposition.movelist[movei].MovingPiece > 0 &
                    pposition.movelist[movei].coordinates[4] == pposition.boardheight - 1)
                {
                    includepromote = true;
                    includenonpromote = false;
                }
                else if (pposition.movelist[movei].MovingPiece < 0 &
                         pposition.movelist[movei].coordinates[4] == 0)
                {
                    includepromote = true;
                    includenonpromote = false;
                }
                else
                {
                    includepromote = false;
                    includenonpromote = true;
                }
            }
            else
            {
                includepromote = false;
                includenonpromote = true;
            }
            if (includenonpromote == true)
            {
                pposition.movelist_totalfound += 1;
                DeleteLatestMoveIfDuplicate(ref pposition, pti);
            }
            if (includepromote == true)
            {
                for (int pi = 0; pi < MyWeirdEngineMoveFinder.piecetypes.Length; pi++)
                {
                    if (pi == pti_self) { }//nothing
                    else if (MyWeirdEngineMoveFinder.piecetypes[pi].SpecialPiece_ind == SpecialPiece.King) { }//nothing
                    else if (MyWeirdEngineMoveFinder.piecetypes[pi].SpecialPiece_ind == SpecialPiece.Amazon) { }//nothing
                    else
                    {
                        int movei2 = pposition.movelist_totalfound;
                        this.SynchronizeChessmove(pposition.movelist[movei], ref pposition.movelist[movei2]);
                        if (pposition.movelist[movei].MovingPiece < 0)
                        {
                            pposition.movelist[movei2].PromoteToPiece = (pi + 1) * -1;
                        }
                        else
                        {
                            pposition.movelist[movei2].PromoteToPiece = pi + 1;
                        }
                        pposition.movelist_totalfound += 1;
                        DeleteLatestMoveIfDuplicate(ref pposition, pti);
                    }
                }
            }
        }
        public void GetPawn2StepMoves(ref chessposition pposition, int i, int j, int z)
        {
            int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(pposition.squares[i, j, z]);

            if (MyWeirdEngineMoveFinder.piecetypes[pti].SpecialPiece_ind != SpecialPiece.Pawn)
            {
                return;
            }
            if (pposition.colourtomove > 0 & j != 1)
            {
                return;
            }
            if (pposition.colourtomove < 0 & j != pposition.boardheight - 2)
            {
                return;
            }
            int i2;
            int i_skip;
            int j2;
            int j_skip;
            int movei;
            i2 = i;
            i_skip = i;
            if (pposition.colourtomove > 0)
            {
                j_skip = j + 1;
                j2 = j + 2;
            }
            else
            {
                j_skip = j - 1;
                j2 = j - 2;
            }
            if (pposition.squares[i_skip, j_skip, z] == 0 & pposition.squares[i2, j2, z] == 0)
            {
                movei = pposition.movelist_totalfound;
                InitializeMove(ref pposition, movei, i, j, z, i2, j2, z);
                pposition.movelist[movei].MovingPiece = pposition.squares[i, j, z];
                pposition.movelist_totalfound += 1;
                //DeleteLatestMoveIfDuplicate(ref pposition, pti);
            }
        }
        public int ExecuteMove(int posidx, chessmove pmove, int prevposidx)
        {
            int newposidx = posidx + 1;
            int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(pmove.MovingPiece);

            MyWeirdEngineMoveFinder.SynchronizePosition(ref MyWeirdEngineMoveFinder.positionstack[posidx], ref MyWeirdEngineMoveFinder.positionstack[newposidx]);

            int i1 = pmove.coordinates[0];
            int j1 = pmove.coordinates[1];
            int z1 = pmove.coordinates[2];
            int i2 = pmove.coordinates[3];
            int j2 = pmove.coordinates[4];
            int z2 = pmove.coordinates[5];

            if (pmove.PromoteToPiece != 0)
            {
                MyWeirdEngineMoveFinder.positionstack[newposidx].squares[i2, j2, z2] = pmove.PromoteToPiece;
            }
            else
            {
                MyWeirdEngineMoveFinder.positionstack[newposidx].squares[i2, j2, z2] = pmove.MovingPiece;
            }
            this.MyWeirdEngineMoveFinder.positionstack[newposidx].squares[i1, j1, z1] = 0;

            if (MyWeirdEngineMoveFinder.positionstack[posidx].colourtomove == 1)
            {
                MyWeirdEngineMoveFinder.positionstack[newposidx].colourtomove = -1;
            }
            else
            {
                MyWeirdEngineMoveFinder.positionstack[newposidx].colourtomove = 1;
            }

            return newposidx;
        }

    }
}
