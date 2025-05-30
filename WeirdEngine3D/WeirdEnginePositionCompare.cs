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
    public struct searchresult
    {
        public int naivematch;
        public int prio_ordermatch;
        public int reusematch;
    }
    public struct TransTableItem
    {
        public chessposition t_position;
        public int used_depth;
        public double used_alpha;
        public double used_beta;
        public double calculated_value;
        public int number_of_no_selfcheck_moves;
        public chessmove bestmove;
    }
    public class WeirdEnginePositionCompare
    {
        public WeirdEngineMoveFinder MyWeirdEngineMoveFinder;
        public TransTableItem[] TransTable;
        public int TransTable_no_items_allocated;//memory allocated
        public int TransTable_no_items_available;//functionally available
        public int dumbcursor;
        public int TransTable_no_positions_reused;
        public WeirdEnginePositionCompare(WeirdEngineMoveFinder pWeirdEngineMoveFinder)
        {
            this.MyWeirdEngineMoveFinder = pWeirdEngineMoveFinder;
            TransTable_no_items_allocated = 100000;
            dumbcursor = 0;
        }
        public chessposition New_chessposition(int pboardwidth, int pboardheight, int pdepth_3d)
        {
            chessposition topos = new chessposition();
            topos.boardwidth = pboardwidth;
            topos.boardheight = pboardheight;
            topos.depth_3d = pdepth_3d;
            topos.squares = null;
            topos.squares = new int[pboardwidth, pboardheight, pdepth_3d];
            topos.squareInfo = null;
            topos.movelist_totalfound = 0;
            return topos;
        }
        public void AllocateTransTableItem(ref TransTableItem ttitem, int pboardwidth, int pboardheight, int pdepth_3d)
        {
            ttitem.t_position = New_chessposition(pboardwidth, pboardheight, pdepth_3d);

            ttitem.used_depth = 0;
            ttitem.used_alpha = 0;
            ttitem.used_beta = 0;
            ttitem.calculated_value = 0;
            ttitem.number_of_no_selfcheck_moves = 0;
            MyWeirdEngineMoveFinder.Init_chessmove(ref ttitem.bestmove);
        }
        public void AllocateTransTable()
        {
            TransTable = null;
            TransTable = new TransTableItem[TransTable_no_items_allocated];

            int w = MyWeirdEngineMoveFinder.positionstack[0].boardwidth;
            int h = MyWeirdEngineMoveFinder.positionstack[0].boardheight;
            int z = MyWeirdEngineMoveFinder.positionstack[0].depth_3d;

            for (int p = 0; p < TransTable_no_items_allocated; p++)
            {
                AllocateTransTableItem(ref TransTable[p], w, h, z);
            }
            TransTable_no_items_available = 0;
        }
        public void StorePosition(chessposition frompos, int t_naive_match,
                                                         chessmove mv,
                                                         int used_depth,
                                                         double used_alpha,
                                                         double used_beta,
                                                         double calculated_value,
                                                         int number_of_no_selfcheck_moves)
        {

            if (t_naive_match > -1)
            {
                //causing trouble --> comparing now
                //StoreIntoTransTable(frompos, t_naive_match, mv,
                //                    used_depth, used_alpha, used_beta, calculated_value);
                //return;
            }
            if (TransTable_no_items_available < TransTable_no_items_allocated)
            {
                StoreIntoTransTable(frompos, TransTable_no_items_available, mv,
                                    used_depth, used_alpha, used_beta, calculated_value,
                                    number_of_no_selfcheck_moves);
                TransTable_no_items_available += 1;
                return;
            }
            if (dumbcursor >= TransTable_no_items_allocated)
            {
                dumbcursor = 0;
            }
            StoreIntoTransTable(frompos, dumbcursor, mv,
                                    used_depth, used_alpha, used_beta, calculated_value,
                                    number_of_no_selfcheck_moves);
            dumbcursor += 1;
        }
        public void StoreIntoTransTable(chessposition frompos, int itemidx, chessmove mv,
                                                               int used_depth,
                                                               double used_alpha,
                                                               double used_beta,
                                                               double calculated_value,
                                                               int number_of_no_selfcheck_moves)
        {
            TransTable[itemidx].t_position.boardwidth = frompos.boardwidth;
            TransTable[itemidx].t_position.boardheight = frompos.boardheight;
            TransTable[itemidx].t_position.depth_3d = frompos.depth_3d;
            TransTable[itemidx].t_position.colourtomove = frompos.colourtomove;

            for (int z = 0; z < frompos.depth_3d; z++)
            {
                for (int i = 0; i < frompos.boardwidth; i++)
                {
                    for (int j = 0; j < frompos.boardheight; j++)
                    {
                        TransTable[itemidx].t_position.squares[i, j, z] = frompos.squares[i, j, z];
                    }
                }
            }
            MyWeirdEngineMoveFinder.MyWeirdEngineMoveGenerator.SynchronizeChessmove(mv, ref TransTable[itemidx].bestmove);
            TransTable[itemidx].used_depth = used_depth;
            TransTable[itemidx].used_alpha = used_alpha;
            TransTable[itemidx].used_beta = used_beta;
            TransTable[itemidx].calculated_value = calculated_value;
            TransTable[itemidx].number_of_no_selfcheck_moves = number_of_no_selfcheck_moves;
        }
        public bool alpha_beta_compatible(int p, double current_alpha, double current_beta)
        {
            //Score was fail high or lowerbound
            if (TransTable[p].used_beta < TransTable[p].calculated_value &
                current_beta < TransTable[p].calculated_value)
            {
                return true;
            }
            //Score was fail low or upperbound
            if (TransTable[p].used_alpha > TransTable[p].calculated_value &
                current_alpha > TransTable[p].calculated_value)
            {
                return true;
            }
            //Score was exact
            if (TransTable[p].used_alpha <= TransTable[p].calculated_value &
                current_alpha <= TransTable[p].calculated_value &
                TransTable[p].used_beta >= TransTable[p].calculated_value &
                current_beta >= TransTable[p].calculated_value)
            {
                return true;
            }
            return false;
        }
        public searchresult SearchTransTable(chessposition pposition, int requested_depth,
                                             double current_alpha, double current_beta)
        {
            searchresult myresult = new searchresult();
            myresult.naivematch = -1;
            myresult.prio_ordermatch = -1;
            myresult.reusematch = -1;
            for (int p = 0; p < TransTable_no_items_available; p++)
            {
                if (PositionsAreEqual(pposition, TransTable[p].t_position))
                {
                    myresult.naivematch = p;
                    if (TransTable[p].used_depth > MyWeirdEngineMoveFinder.myenginesettings.presort_using_depth
                        & alpha_beta_compatible(p, current_alpha, current_beta) == true)
                    {
                        if (myresult.prio_ordermatch > -1)
                        {
                            if (TransTable[p].used_depth > TransTable[myresult.prio_ordermatch].used_depth)
                            {
                                myresult.prio_ordermatch = p;
                            }
                        }
                        else { myresult.prio_ordermatch = p; }
                    }
                    if (TransTable[p].used_depth >= requested_depth)
                    {
                        if (alpha_beta_compatible(p, current_alpha, current_beta) == true)
                        {
                            myresult.reusematch = p;
                            return myresult;
                        }
                    }
                }
            }
            return myresult;
        }
        public bool PositionsAreEqual(chessposition posA, chessposition posB)
        {
            //Error margin is accepted here w.r.t. en passant and Time Thief capture
            //Some positions are flagged as NOT equal while they are equal upon closer examination
            //But flagged equal here guarantees that they were really equal
            if (posA.colourtomove != posB.colourtomove) { return false; }
            if (posA.boardwidth != posB.boardwidth) { return false; }
            if (posA.boardheight != posB.boardheight) { return false; }

            for (int z = 0; z < posA.depth_3d; z++)
            {
                for (int i = 0; i < posA.boardwidth; i++)
                {
                    for (int j = 0; j < posA.boardheight; j++)
                    {
                        if (posA.squares[i, j, z] != posB.squares[i, j, z]) { return false; }
                    }
                }
            }

            return true;
        }
        public void InitRepetitionCounter()
        {
            for (int p = 0; p < WeirdEngineMoveFinder.positionstack_size; p++)
            {
                MyWeirdEngineMoveFinder.positionstack[p].RepetitionCounter = 0;
            }
        }
        public void SetRepetitionCounter(int posidx)
        {
            MyWeirdEngineMoveFinder.positionstack[posidx].RepetitionCounter = 0;
            for (int p = 0; p < posidx; p++)
            {
                bool eq = this.PositionsAreEqual(MyWeirdEngineMoveFinder.positionstack[p],
                    MyWeirdEngineMoveFinder.positionstack[posidx]);
                if (eq == true)
                {
                    MyWeirdEngineMoveFinder.positionstack[posidx].RepetitionCounter =
                        MyWeirdEngineMoveFinder.positionstack[p].RepetitionCounter + 1;
                }
            }
            if (MyWeirdEngineMoveFinder.positionstack[posidx].RepetitionCounter == 0)
            {
                MyWeirdEngineMoveFinder.positionstack[posidx].RepetitionCounter = 1;
            }
        }
        public bool MovesAreEqual(chessmove moveA, chessmove moveB)
        {
            if (moveA.MovingPiece != moveB.MovingPiece) { return false; }
            if (moveA.coordinates[0] != moveB.coordinates[0]) { return false; }
            if (moveA.coordinates[1] != moveB.coordinates[1]) { return false; }
            if (moveA.coordinates[2] != moveB.coordinates[2]) { return false; }
            if (moveA.coordinates[3] != moveB.coordinates[3]) { return false; }
            if (moveA.coordinates[4] != moveB.coordinates[4]) { return false; }
            if (moveA.coordinates[5] != moveB.coordinates[5]) { return false; }
            if (moveA.IsCapture != moveB.IsCapture) { return false; }
            if (moveA.PromoteToPiece != moveB.PromoteToPiece) { return false; }
            return true;
        }
    }
}
