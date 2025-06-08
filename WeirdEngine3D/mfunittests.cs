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
    //Unittests for WeirdEngineMoveFinder
    public class mfunittests
    {
        private bool AllTestsPassed;
        public WeirdEngineMoveFinder MyWeirdEngineMoveFinder;
        public WeirdEngineJson MyWeirdEngineJson;
        public mfunittests(WeirdEngineMoveFinder pWeirdEngineMoveFinder, WeirdEngineJson myWeirdEngineJson)
        {
            this.MyWeirdEngineMoveFinder = pWeirdEngineMoveFinder;
            this.MyWeirdEngineJson = myWeirdEngineJson;
        }


        public void TestMove(string ppath, string ppositionfilename, string expectedmovingpiecename,
                                                                     int pi1, int pj1, int pz1, int pi2, int pj2, int pz2,
                                                                     bool IsExpected)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            bool mymovehappened = false;
            for (int movei = 0; movei < MyWeirdEngineMoveFinder.positionstack[0].movelist_totalfound; movei++)
            {
                int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].MovingPiece);
                if (MyWeirdEngineMoveFinder.piecetypes[pti].name == expectedmovingpiecename)
                {
                    if (MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[0] == pi1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[1] == pj1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[2] == pz1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[3] == pi2 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[4] == pj2 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[5] == pz2)
                    {
                        mymovehappened = true;
                    }
                }
            }
            string givenmvstr;
            givenmvstr = expectedmovingpiecename + "|" + pi1.ToString()
                                                 + "|" + pj1.ToString()
                                                 + "|" + pz1.ToString()
                                                 + "|" + pi2.ToString()
                                                 + "|" + pj2.ToString()
                                                 + "|" + pz2.ToString();
            if (mymovehappened == false & IsExpected == true)
            {
                MessageBox.Show(ppositionfilename + givenmvstr + " did not happen but was expected");
                AllTestsPassed = false;
            }
            if (mymovehappened == true & IsExpected == false)
            {
                MessageBox.Show(ppositionfilename + givenmvstr + " did happen but was not expected");
                AllTestsPassed = false;
            }
        }
        public void TestPawn(string ppath, string ppositionfilename, int pi1, int pj1, int pz1, int pi2, int pj2, int pz2)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            bool mymovehappened = false;
            for (int movei = 0; movei < MyWeirdEngineMoveFinder.positionstack[0].movelist_totalfound; movei++)
            {
                int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].MovingPiece);
                if (MyWeirdEngineMoveFinder.piecetypes[pti].name == "Pawn")
                {
                    if (MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[0] == pi1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[1] == pj1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[2] == pz1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[3] == pi2 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[4] == pj2 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[5] == pz2)
                    {
                        mymovehappened = true;
                    }
                }

            }
            string givenmvstr;
            givenmvstr = "Pawn" + "|" + pi1.ToString()
                                + "|" + pj1.ToString()
                                + "|" + pi2.ToString()
                                + "|" + pj2.ToString();
            if (mymovehappened == false)
            {
                MessageBox.Show(ppositionfilename + givenmvstr + " did not happen but was expected");
                AllTestsPassed = false;
            }
        }
        public void TestPawnPromote(string ppath, string ppositionfilename, int pi1, int pj1, int pz1, int pi2, int pj2, int pz2)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            bool mymovehappened = false;
            for (int movei = 0; movei < MyWeirdEngineMoveFinder.positionstack[0].movelist_totalfound; movei++)
            {
                int pti = MyWeirdEngineMoveFinder.pieceTypeIndex(MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].MovingPiece);
                if (MyWeirdEngineMoveFinder.piecetypes[pti].name == "Pawn")
                {
                    if (MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[0] == pi1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[1] == pj1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[2] == pz1 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[3] == pi2 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[4] == pj2 &
                        MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].coordinates[5] == pz2)
                    {
                        if (MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].PromoteToPiece != 0)
                        {
                            int ptp = MyWeirdEngineMoveFinder.pieceTypeIndex(MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].PromoteToPiece);
                            if (MyWeirdEngineMoveFinder.piecetypes[ptp].name == "Hunter")
                            {
                                mymovehappened = true;
                            }
                        }
                    }
                }
            }
            string givenmvstr;
            givenmvstr = "Pawn" + "|" + pi1.ToString()
                                + "|" + pj1.ToString()
                                + "|" + pi2.ToString()
                                + "|" + pj2.ToString() + " --> promote to hunter ";
            if (mymovehappened == false)
            {
                MessageBox.Show(ppositionfilename + givenmvstr + " did not happen but was expected");
                AllTestsPassed = false;
            }
        }
        public void TestNoPromote(string ppath, string ppositionfilename)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            bool promotehappened = false;
            for (int movei = 0; movei < MyWeirdEngineMoveFinder.positionstack[0].movelist_totalfound; movei++)
            {
                if (MyWeirdEngineMoveFinder.positionstack[0].movelist[movei].PromoteToPiece != 0)
                {
                    promotehappened = true;
                }
            }
            if (promotehappened == true)
            {
                MessageBox.Show(ppositionfilename + " promotion did happen but was not expected");
                AllTestsPassed = false;
            }
        }
        public void TestSelfCheck(string ppath, string ppositionfilename)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(0);

            if (MyWeirdEngineMoveFinder.MyWeirdEngineMoveGenerator.POKingIsInCheck(ref MyWeirdEngineMoveFinder.positionstack[0]) == true)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Check expected but there was no check.");
                AllTestsPassed = false;
            }
        }
        public void TestCheck(string ppath, string ppositionfilename)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            if (MyWeirdEngineMoveFinder.MyWeirdEngineMoveGenerator.PMKingIsInCheck(ref MyWeirdEngineMoveFinder.positionstack[0]) == true)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Check expected but there was no check.");
                AllTestsPassed = false;
            }
        }
        public void TestStalemate(string ppath, string ppositionfilename)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            if (a.posvalue == 0)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Stalemate expected but there was no stalemate.");
                AllTestsPassed = false;
            }
        }
        public void TestMate(string ppath, string ppositionfilename)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            if (a.posvalue == 100 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == -1)
            {
                //nothing
            }
            else if (a.posvalue == -100 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == 1)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Mate expected but there was no mate.");
                AllTestsPassed = false;
            }
        }
        public void TestNoMate(string ppath, string ppositionfilename)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(1);

            if (a.posvalue == 100 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == -1)
            {
                MessageBox.Show(ppositionfilename + " No mate expected but there was mate.");
                AllTestsPassed = false;
            }
            else if (a.posvalue == -100 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == 1)
            {
                MessageBox.Show(ppositionfilename + " No mate expected but there was mate.");
                AllTestsPassed = false;
            }
            else
            {
                //nothing
            }
        }
        public void TestNoMate_n(string ppath, string ppositionfilename, int depth)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(depth);

            if (a.posvalue >= 100 - (depth * 0.1) || a.posvalue <= (depth * 0.1) - 100)
            {
                MessageBox.Show(ppositionfilename + " No mate expected but there was mate.");
                AllTestsPassed = false;
            }
        }
        public void TestMate_n(string ppath, string ppositionfilename, int mate_in_n, int pi1, int pj1, int pz1, int pi2, int pj2, int pz2)
        {
            int depth;
            if (mate_in_n > 0 & mate_in_n < 5)
            {
                depth = mate_in_n * 2;
            }
            else
            {
                depth = 4;
            }

            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);

            DateTime startdatetime = DateTime.Now;
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(depth);
            DateTime enddatetime = DateTime.Now;

            int secondsneeded = (int)(enddatetime - startdatetime).TotalSeconds;
            if (depth < 5 & secondsneeded > 15)
            {
                MessageBox.Show(ppositionfilename + " Performance of calculation under acceptable levels.");
                AllTestsPassed = false;
            }

            if (pi1 != -1)
            {
                if (MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[0] == pi1 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[1] == pj1 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[2] == pz1 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[3] == pi2 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[4] == pj2 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[5] == pz2)
                {
                    //nothing
                }
                else
                {
                    MessageBox.Show(ppositionfilename + " Mate expected, but the identified move is not correct.");
                    AllTestsPassed = false;
                }
            }
            if (a.posvalue >= 100 - (depth * 0.1) & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == 1)
            {
                //nothing
            }
            else if (a.posvalue <= (depth * 0.1) - 100 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == -1)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Mate expected but there was no mate.");
                AllTestsPassed = false;
            }
        }
        public void TestStalemate_n(string ppath, string ppositionfilename, int stalemate_in_n)
        {
            int depth;
            if (stalemate_in_n > 0 & stalemate_in_n < 5)
            {
                depth = (stalemate_in_n * 2) + 1;
            }
            else
            {
                depth = 5;
            }

            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);

            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(depth);

            if (a.posvalue == 0)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Stalemate expected, but there was no stalemate.");
                AllTestsPassed = false;
            }
        }
        public void TestDraw_n(string ppath, string ppositionfilename, int depth)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);

            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(depth);

            if (a.posvalue == 0)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Forced draw expected, but there was no forced draw.");
                AllTestsPassed = false;
            }
        }
        public void TestMate_high_depth(string ppath, string ppositionfilename, int pdepth, int pi1, int pj1, int pz1, int pi2, int pj2, int pz2)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);

            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(pdepth);

            if (pi1 != -1)
            {
                if (MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[0] == pi1 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[1] == pj1 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[2] == pz1 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[3] == pi2 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[4] == pj2 &
                    MyWeirdEngineMoveFinder.positionstack[0].movelist[a.moveidx].coordinates[5] == pz2)
                {
                    //nothing
                }
                else
                {
                    MessageBox.Show(ppositionfilename + " Mate expected, but the identified move is not correct.");
                    AllTestsPassed = false;
                }
            }
            if (a.posvalue > 98 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == 1)
            {
                //nothing
            }
            else if (a.posvalue < -98 & MyWeirdEngineMoveFinder.positionstack[0].colourtomove == -1)
            {
                //nothing
            }
            else
            {
                MessageBox.Show(ppositionfilename + " Mate expected but there was no mate.");
                AllTestsPassed = false;
            }
        }
        public void BaselinePerformance(string ppath, string ppositionfilename, int depth, int baseline_seconds)
        {
            MyWeirdEngineJson.LoadPositionJson(ppath, ppositionfilename);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", ppositionfilename);

            DateTime startdatetime = DateTime.Now;
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(depth);
            DateTime enddatetime = DateTime.Now;
            int secondsneeded = (int)(enddatetime - startdatetime).TotalSeconds;

            //MessageBox.Show(ppositionfilename + " observed secondsneeded " + secondsneeded.ToString()
            //                                  + " baseline_seconds " + baseline_seconds.ToString());

            if (secondsneeded > baseline_seconds)
            {
                MessageBox.Show(ppositionfilename + " Performance of calculation under acceptable levels. "
                                                  + startdatetime.ToString() + "|" + enddatetime.ToString());
                AllTestsPassed = false;
            }
        }

        public void RunAllUnittests(string ppath)
        {
            AllTestsPassed = true;
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            MyWeirdEngineMoveFinder.myenginesettings.display_when_depth_gt = 7;
            MessageBox.Show("Start with running all unittests");

            TestPawn(ppath, "02A_pawn_white", 3, 4, 0, 3, 5, 0);
            TestPawn(ppath, "02A_pawn_black", 4, 3, 0, 4, 2, 0);
            TestPawn(ppath, "02B_pawn_white", 2, 1, 0, 2, 3, 0);
            TestPawn(ppath, "02B_pawn_black", 1, 6, 0, 1, 4, 0);
            TestPawn(ppath, "02C_pawn_white", 5, 3, 0, 4, 4, 0);
            TestPawn(ppath, "02C_pawn_white", 5, 3, 0, 6, 4, 0);
            TestPawn(ppath, "02C_pawn_black", 2, 3, 0, 3, 2, 0);
            TestPawn(ppath, "02C_pawn_black", 2, 3, 0, 1, 2, 0);
            TestPawnPromote(ppath, "02E_pawn_white", 1, 6, 0, 1, 7, 0);
            TestPawnPromote(ppath, "02E_pawn_white", 1, 6, 0, 0, 7, 0);
            TestPawnPromote(ppath, "02E_pawn_black", 6, 1, 0, 6, 0, 0);
            TestPawnPromote(ppath, "02E_pawn_black", 6, 1, 0, 7, 0, 0);

            TestMove(ppath, "03A_divergent_white", "Hunter", 4, 5, 0, 4, 6, 0, true);
            TestMove(ppath, "03A_divergent_white", "Hunter", 4, 5, 0, 2, 6, 0, true);
            TestMove(ppath, "03A_divergent_black", "Hunter", 2, 3, 0, 2, 2, 0, true);
            TestMove(ppath, "03A_divergent_black", "Hunter", 2, 3, 0, 4, 4, 0, true);
            TestMove(ppath, "03A_divergent_white", "Hunter", 4, 5, 0, 4, 4, 0, false);
            TestMove(ppath, "03A_divergent_white", "Hunter", 4, 5, 0, 2, 5, 0, false);
            TestMove(ppath, "03A_divergent_black", "Hunter", 2, 3, 0, 2, 4, 0, false);
            TestMove(ppath, "03A_divergent_black", "Hunter", 2, 3, 0, 4, 3, 0, false);

            TestMove(ppath, "03A_divergent_white_swap_xz", "Hunter", 0, 5, 4, 0, 6, 4, true);
            TestMove(ppath, "03A_divergent_white_swap_xz", "Hunter", 0, 5, 4, 0, 6, 2, true);
            TestMove(ppath, "03A_divergent_black_swap_xz", "Hunter", 0, 3, 2, 0, 2, 2, true);
            TestMove(ppath, "03A_divergent_black_swap_xz", "Hunter", 0, 3, 2, 0, 4, 4, true);
            TestMove(ppath, "03A_divergent_white_swap_xz", "Hunter", 0, 5, 4, 0, 4, 4, false);
            TestMove(ppath, "03A_divergent_white_swap_xz", "Hunter", 0, 5, 4, 0, 5, 2, false);
            TestMove(ppath, "03A_divergent_black_swap_xz", "Hunter", 0, 3, 2, 0, 4, 2, false);
            TestMove(ppath, "03A_divergent_black_swap_xz", "Hunter", 0, 3, 2, 0, 3, 4, false);

            TestMove(ppath, "03A_divergent_white_swap_yz", "Hunter", 4, 0, 5, 4, 0, 6, true);
            TestMove(ppath, "03A_divergent_white_swap_yz", "Hunter", 4, 0, 5, 2, 0, 6, true);
            TestMove(ppath, "03A_divergent_black_swap_yz", "Hunter", 2, 0, 3, 2, 0, 2, true);
            TestMove(ppath, "03A_divergent_black_swap_yz", "Hunter", 2, 0, 3, 4, 0, 4, true);
            TestMove(ppath, "03A_divergent_white_swap_yz", "Hunter", 4, 0, 5, 4, 0, 4, false);
            TestMove(ppath, "03A_divergent_white_swap_yz", "Hunter", 4, 0, 5, 2, 0, 5, false);
            TestMove(ppath, "03A_divergent_black_swap_yz", "Hunter", 2, 0, 3, 2, 0, 4, false);
            TestMove(ppath, "03A_divergent_black_swap_yz", "Hunter", 2, 0, 3, 4, 0, 3, false);

            TestCheck(ppath, "04A_check_white");
            TestCheck(ppath, "04A_check_black");

            TestStalemate(ppath, "05A_stalemate_white");
            TestStalemate(ppath, "05A_stalemate_black");
            TestStalemate(ppath, "05A_stalemate_white_swap_xz");
            TestStalemate(ppath, "05A_stalemate_black_swap_xz");
            TestStalemate(ppath, "05A_stalemate_white_swap_yz");
            TestStalemate(ppath, "05A_stalemate_black_swap_yz");

            TestMate(ppath, "06A_mate_0_white");
            TestMate(ppath, "06A_mate_0_black");

            TestMate_n(ppath, "06B_mate_1_white", 1, 5, 1, 0, 2, 4, 0);
            TestMate_n(ppath, "06B_mate_1_black", 1, 5, 6, 0, 2, 3, 0);
            TestMate_n(ppath, "06B_mate_1_white_swap_xz", 1, 0, 1, 5, 0, 4, 2);
            TestMate_n(ppath, "06B_mate_1_black_swap_xz", 1, 0, 6, 5, 0, 3, 2);
            TestMate_n(ppath, "06B_mate_1_white_swap_yz", 1, 5, 0, 1, 2, 0, 4);
            TestMate_n(ppath, "06B_mate_1_black_swap_yz", 1, 5, 0, 6, 2, 0, 3);

            TestMate_n(ppath, "06C_mate_2_white_01", 2, 0, 1, 0, 6, 7, 0);
            TestMate_n(ppath, "06C_mate_2_white_02", 2, 7, 1, 0, 1, 7, 0);
            TestMate_n(ppath, "06C_mate_2_black_01", 2, 7, 6, 0, 1, 0, 0);
            TestMate_n(ppath, "06C_mate_2_black_02", 2, 0, 6, 0, 6, 0, 0);

            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = false;
            TestMate_n(ppath, "06D_huntermate_3_white", 3, 2, 4, 0, 1, 4, 0);
            TestMate_n(ppath, "06D_huntermate_3_black", 3, 2, 3, 0, 1, 3, 0);
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            TestMate_n(ppath, "06D_huntermate_3_white", 3, 2, 4, 0, 1, 4, 0);
            TestMate_n(ppath, "06D_huntermate_3_black", 3, 2, 3, 0, 1, 3, 0);

            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = false;
            TestMate_n(ppath, "06D_huntermate_3_white_swap_xz", 3, 0, 4, 2, 0, 4, 1);
            TestMate_n(ppath, "06D_huntermate_3_black_swap_xz", 3, 0, 3, 2, 0, 3, 1);
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            TestMate_n(ppath, "06D_huntermate_3_white_swap_xz", 3, 0, 4, 2, 0, 4, 1);
            TestMate_n(ppath, "06D_huntermate_3_black_swap_xz", 3, 0, 3, 2, 0, 3, 1);

            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = false;
            TestMate_n(ppath, "06D_huntermate_3_white_swap_yz", 3, 2, 0, 4, 1, 0, 4);
            TestMate_n(ppath, "06D_huntermate_3_black_swap_yz", 3, 2, 0, 3, 1, 0, 3);
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            TestMate_n(ppath, "06D_huntermate_3_white_swap_yz", 3, 2, 0, 4, 1, 0, 4);
            TestMate_n(ppath, "06D_huntermate_3_black_swap_yz", 3, 2, 0, 3, 1, 0, 3);

            TestStalemate_n(ppath, "08A_stalemate_2_white", 2);
            TestStalemate_n(ppath, "08A_stalemate_2_black", 2);

            TestStalemate_n(ppath, "08B_insufficient_material_1", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_1_swap_xz", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_1_swap_yz", 14);

            TestStalemate_n(ppath, "08B_insufficient_material_2", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_2_swap_xz", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_2_swap_yz", 14);

            TestStalemate_n(ppath, "08B_insufficient_material_3", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_4", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_5", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_6", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_7", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_8", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_9a", 14);
            TestStalemate_n(ppath, "08B_insufficient_material_9b", 14);

            TestMate_n(ppath, "08C_sufficient_material_mate_1_white_01", 1, 3, 4, 0, 2, 6, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_1_white_02", 1, 3, 4, 0, 1, 5, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_1_white_03", 1, 3, 4, 0, 1, 5, 0);

            TestMate_n(ppath, "08C_sufficient_material_mate_2_white_01", 2, 4, 3, 0, 3, 4, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_2_white_01_swap_xz", 2, 0, 3, 4, 0, 4, 3);
            TestMate_n(ppath, "08C_sufficient_material_mate_2_white_01_swap_yz", 2, 4, 0, 3, 3, 0, 4);

            TestMate_n(ppath, "08C_sufficient_material_mate_1_black_01", 1, 3, 3, 0, 2, 1, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_1_black_02", 1, 3, 3, 0, 1, 2, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_1_black_03", 1, 3, 3, 0, 1, 2, 0);

            TestMate_n(ppath, "08C_sufficient_material_mate_2_black_01", 2, 4, 4, 0, 3, 3, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_2_black_01_swap_xz", 2, 0, 4, 4, 0, 3, 3);
            TestMate_n(ppath, "08C_sufficient_material_mate_2_black_01_swap_yz", 2, 4, 0, 4, 3, 0, 3);

            TestMate_n(ppath, "08C_sufficient_material_mate_2_white_02", 2, 5, 5, 0, 6, 5, 0);
            TestMate_n(ppath, "08C_sufficient_material_mate_2_black_02", 2, 5, 2, 0, 6, 2, 0);

            TestDraw_n(ppath, "08D_forced_draw_white_01", 7);
            TestDraw_n(ppath, "08D_forced_draw_black_01", 7);
            TestDraw_n(ppath, "08D_forced_draw_white_02", 6);
            TestDraw_n(ppath, "08D_forced_draw_black_02", 6);

            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = false;
            BaselinePerformance(ppath, "07A_mate_4_white_BN", 8, 3);
            BaselinePerformance(ppath, "07A_mate_4_black_BN", 8, 3);
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            BaselinePerformance(ppath, "07A_mate_4_white_BN", 8, 5);
            BaselinePerformance(ppath, "07A_mate_4_black_BN", 8, 5);

            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = false;
            BaselinePerformance(ppath, "07A_mate_4_white_BN_swap_xz", 8, 3);
            BaselinePerformance(ppath, "07A_mate_4_black_BN_swap_xz", 8, 3);
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            BaselinePerformance(ppath, "07A_mate_4_white_BN_swap_xz", 8, 5);
            BaselinePerformance(ppath, "07A_mate_4_black_BN_swap_xz", 8, 5);

            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = false;
            BaselinePerformance(ppath, "07A_mate_4_white_BN_swap_yz", 8, 3);
            BaselinePerformance(ppath, "07A_mate_4_black_BN_swap_yz", 8, 3);
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            BaselinePerformance(ppath, "07A_mate_4_white_BN_swap_yz", 8, 5);
            BaselinePerformance(ppath, "07A_mate_4_black_BN_swap_yz", 8, 5);



            TestMate_n(ppath, "15A_mate_in_1_nightrider_white", 1, 0, 2, 0, 1, 4, 0);
            TestMate_n(ppath, "15A_mate_in_1_nightrider_black", 1, 9, 5, 0, 8, 3, 0);

            MyWeirdEngineJson.LoadPieceTypesFromJson("fide");
            MyWeirdEngineJson.writelog("Unittests - Now going to 14A");
            TestMate_high_depth(ppath, "14A_mate_in_1_depth_14_white", 14, 4, 2, 0, 0, 2, 0);
            TestMate_high_depth(ppath, "14A_mate_in_1_depth_14_black", 14, 4, 5, 0, 0, 5, 0);
            TestMate_high_depth(ppath, "14B_mate_in_2_depth_5_bug_white", 5, 7, 3, 0, 6, 2, 0);
            TestMate_high_depth(ppath, "14B_mate_in_2_depth_5_bug_black", 5, 7, 4, 0, 6, 5, 0);

            MyWeirdEngineJson.LoadPieceTypesFromJson("unittestgame");
            TestMove(ppath, "16A_limited_range_white", "Queen3", 4, 4, 4, 7, 7, 4, true);
            TestMove(ppath, "16A_limited_range_white", "Queen3", 4, 4, 4, 8, 8, 4, false);
            TestMove(ppath, "16A_limited_range_white", "Queen3", 4, 4, 4, 1, 1, 4, true);
            TestMove(ppath, "16A_limited_range_white", "Queen3", 4, 4, 4, 0, 4, 4, false);
            TestMove(ppath, "16A_limited_range_white", "Queen3", 4, 4, 4, 4, 4, 2, true);
            TestMove(ppath, "16A_limited_range_white", "Queen3", 4, 4, 4, 4, 4, 6, false);

            TestMove(ppath, "16A_limited_range_black", "Queen3", 4, 4, 4, 7, 7, 4, true);
            TestMove(ppath, "16A_limited_range_black", "Queen3", 4, 4, 4, 8, 8, 4, false);
            TestMove(ppath, "16A_limited_range_black", "Queen3", 4, 4, 4, 1, 1, 4, true);
            TestMove(ppath, "16A_limited_range_black", "Queen3", 4, 4, 4, 0, 4, 4, false);
            TestMove(ppath, "16A_limited_range_black", "Queen3", 4, 4, 4, 4, 4, 2, true);
            TestMove(ppath, "16A_limited_range_black", "Queen3", 4, 4, 4, 4, 4, 6, false);


            if (AllTestsPassed == true)
            {
                MessageBox.Show("All unittests passed");
            }
            else
            {
                MessageBox.Show("Some unittests failed");
            }
        }
        public void RunNewUnittests(string ppath)
        {
            AllTestsPassed = true;
            MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = true;
            MyWeirdEngineMoveFinder.myenginesettings.display_when_depth_gt = 7;
            MessageBox.Show("Start with running new unittests");

            if (AllTestsPassed == true)
            {
                MessageBox.Show("All unittests passed");
            }
            else
            {
                MessageBox.Show("Some unittests failed");
            }
        }

    }
}
