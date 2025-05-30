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
    public struct jsonpiecenamelist
    {
        public string[] piecetypes;
    }
    public struct jsonmovecoord
    {
        public int x_from;
        public int y_from;
        public int z_from;
        public int x_to;
        public int y_to;
        public int z_to;
    }
    public struct jsonlayer
    {
        public string[] squares;
    }
    public struct jsonchessposition
    {
        public int boardwidth;
        public int boardheight;
        public int depth_3d;
        public int colourtomove;
        public jsonlayer[] layers;
    }
    public struct jsonTransTableItem
    {
        public jsonchessposition t_position;
        public int used_depth;
        public double used_alpha;
        public double used_beta;
        public double calculated_value;
        public int number_of_no_selfcheck_moves;
        public string bestmove_str;
        public chessmove bestmove;
        public int pos_in_tt;
    }
    public struct jsonTransTable
    {
        public int TransTable_no_items_available;
        public jsonTransTableItem[] TranspositionTable;
    }
    public class WeirdEngineJson
    {
        public WeirdEngineMoveFinder MyWeirdEngineMoveFinder;
        public string jsonsourcepath;
        public string jsonworkpath;
        public string logfilename;
        public WeirdEngineJson(WeirdEngineMoveFinder pWeirdEngineMoveFinder)
        {
            this.MyWeirdEngineMoveFinder = pWeirdEngineMoveFinder;
        }
        public void SetLogfilename()
        {
            string myts;
            DateTime localDate;
            localDate = DateTime.Now;
            myts = localDate.ToString("yyyy-MM-dd_HH_mm");
            this.logfilename = "WeirdEngineMoveFinder_log_" + myts + ".log";
        }
        public void writelog(string themessage)
        {
            string myts;
            DateTime localDate;
            localDate = DateTime.Now;
            myts = localDate.ToString("yyyy-MM-ddTHH:mm:ss");

            using (StreamWriter writer = new StreamWriter(this.jsonworkpath + "log\\" + this.logfilename, append: true))
            {
                writer.WriteLine(myts + " " + themessage);
                writer.Close();
            }
        }
        public void LoadEngineSettingsFromJson(string pFileName)
        {
            string json;
            using (StreamReader r = new StreamReader(this.jsonsourcepath + "enginesettings\\" + pFileName + ".json"))
            {
                json = r.ReadToEnd();
            }
            enginesettings a = JsonConvert.DeserializeObject<enginesettings>(json);

            this.MyWeirdEngineMoveFinder.myenginesettings.presort_when_depth_gt = a.presort_when_depth_gt;
            this.MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate = a.setting_SearchForFastestMate;
            this.MyWeirdEngineMoveFinder.myenginesettings.presort_using_depth = a.presort_using_depth;
            this.MyWeirdEngineMoveFinder.myenginesettings.display_when_depth_gt = a.display_when_depth_gt;
            this.MyWeirdEngineMoveFinder.myenginesettings.consult_tt_when_depth_gt = a.consult_tt_when_depth_gt;
            this.MyWeirdEngineMoveFinder.myenginesettings.store_in_tt_when_depth_gt = a.store_in_tt_when_depth_gt;
        }


        public void LoadPieceFromJson(string pFileName, int seq)
        {
            string json;
            using (StreamReader r = new StreamReader(this.jsonsourcepath + "piecedefinitions\\" + pFileName + ".json"))
            {
                json = r.ReadToEnd();
            }
            chesspiecetype a = JsonConvert.DeserializeObject<chesspiecetype>(json);

            this.MyWeirdEngineMoveFinder.piecetypes[seq].symbol = a.symbol;
            this.MyWeirdEngineMoveFinder.piecetypes[seq].name = a.name;
            this.MyWeirdEngineMoveFinder.piecetypes[seq].IsDivergent = a.IsDivergent;
            this.MyWeirdEngineMoveFinder.piecetypes[seq].CheckDuplicateMoves = a.CheckDuplicateMoves;
            this.MyWeirdEngineMoveFinder.piecetypes[seq].EstimatedValue = a.EstimatedValue;
            if (a.stepleapmovevectors != null)
            {
                this.MyWeirdEngineMoveFinder.piecetypes[seq].stepleapmovevectors = new vector[a.stepleapmovevectors.Length];
                for (int i = 0; i < a.stepleapmovevectors.Length; i++)
                {
                    this.MyWeirdEngineMoveFinder.piecetypes[seq].stepleapmovevectors[i] = a.stepleapmovevectors[i];
                }
            }
            if (a.slidemovevectors != null)
            {
                this.MyWeirdEngineMoveFinder.piecetypes[seq].slidemovevectors = new vector[a.slidemovevectors.Length];
                for (int i = 0; i < a.slidemovevectors.Length; i++)
                {
                    this.MyWeirdEngineMoveFinder.piecetypes[seq].slidemovevectors[i] = a.slidemovevectors[i];
                }
            }
            if (a.stepleapcapturevectors != null)
            {
                this.MyWeirdEngineMoveFinder.piecetypes[seq].stepleapcapturevectors = new vector[a.stepleapcapturevectors.Length];
                for (int i = 0; i < a.stepleapcapturevectors.Length; i++)
                {
                    this.MyWeirdEngineMoveFinder.piecetypes[seq].stepleapcapturevectors[i] = a.stepleapcapturevectors[i];
                }
            }
            if (a.slidecapturevectors != null)
            {
                this.MyWeirdEngineMoveFinder.piecetypes[seq].slidecapturevectors = new vector[a.slidecapturevectors.Length];
                for (int i = 0; i < a.slidecapturevectors.Length; i++)
                {
                    this.MyWeirdEngineMoveFinder.piecetypes[seq].slidecapturevectors[i] = a.slidecapturevectors[i];
                }
            }
        }
        public void SavePieceAsJson(chesspiecetype pchesspiecetype, string pFileName)
        {
            string jsonString = JsonConvert.SerializeObject(pchesspiecetype, Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(this.jsonworkpath + "piecedefinitions_verify\\" + pFileName + ".json"))
            {
                writer.WriteLine(jsonString);
                writer.Close();
            }
        }

        public void LoadPieceTypesFromJson(string pFileName)
        {
            string json;
            using (StreamReader r = new StreamReader(this.jsonsourcepath + "games\\" + pFileName + ".json"))
            {
                json = r.ReadToEnd();
            }
            jsonpiecenamelist piecenames = JsonConvert.DeserializeObject<jsonpiecenamelist>(json);
            int n = piecenames.piecetypes.Length;
            this.MyWeirdEngineMoveFinder.piecetypes = null;
            this.MyWeirdEngineMoveFinder.piecetypes = new chesspiecetype[n];
            for (int i = 0; i < n; i++)
            {
                this.LoadPieceFromJson(piecenames.piecetypes[i], i);
            }
        }
        public void SavePieceTypesAsJson(string pFileName)
        {
            jsonpiecenamelist mynamelist = new jsonpiecenamelist();
            int n = this.MyWeirdEngineMoveFinder.piecetypes.Length;
            mynamelist.piecetypes = new string[n];
            for (int i = 0; i < n; i++)
            {
                mynamelist.piecetypes[i] = this.MyWeirdEngineMoveFinder.piecetypes[i].name;
                this.SavePieceAsJson(this.MyWeirdEngineMoveFinder.piecetypes[i], mynamelist.piecetypes[i]);
            }
            string jsonString = JsonConvert.SerializeObject(mynamelist, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(this.jsonworkpath + "games_verify\\" + pFileName + ".json"))
            {
                writer.WriteLine(jsonString);
                writer.Close();
            }
        }
        public string DisplayInformation()
        {
            string s;

            s = "";

            s = s + "presort_when_depth_gt " + MyWeirdEngineMoveFinder.myenginesettings.presort_when_depth_gt.ToString() + "\n";
            s = s + "SearchForFastestMate " + MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate.ToString() + "\n";
            s = s + "presort_using_depth " + MyWeirdEngineMoveFinder.myenginesettings.presort_using_depth.ToString() + "\n";
            s = s + "display_when_depth_gt " + MyWeirdEngineMoveFinder.myenginesettings.display_when_depth_gt.ToString() + "\n";
            s = s + "consult_tt_when_depth_gt "
                           + MyWeirdEngineMoveFinder.myenginesettings.consult_tt_when_depth_gt.ToString() + "\n";
            s = s + "store_in_tt_when_depth_gt "
                           + MyWeirdEngineMoveFinder.myenginesettings.store_in_tt_when_depth_gt.ToString() + "\n";
            s = s + "jsonsourcepath " + MyWeirdEngineMoveFinder.MyWeirdEngineJson.jsonsourcepath + "\n";
            s = s + "jsonworkpath " + MyWeirdEngineMoveFinder.MyWeirdEngineJson.jsonworkpath + "\n";

            return s;
        }
        public string PieceType2Str(int ptypenr)
        {
            int i;
            if (ptypenr > 0)
            {
                i = ptypenr - 1;
                return this.MyWeirdEngineMoveFinder.piecetypes[i].symbol;
            }
            if (ptypenr < 0)
            {
                i = (ptypenr * -1) - 1;
                return "-" + this.MyWeirdEngineMoveFinder.piecetypes[i].symbol;
            }
            return ".";
        }
        public int Str2PieceType(string psymbol)
        {
            int n = this.MyWeirdEngineMoveFinder.piecetypes.Length;
            for (int i = 0; i < n; i++)
            {
                if (this.MyWeirdEngineMoveFinder.piecetypes[i].symbol == psymbol)
                {
                    return i + 1;
                }
                if ("-" + this.MyWeirdEngineMoveFinder.piecetypes[i].symbol == psymbol)
                {
                    return (i + 1) * -1;
                }
            }
            return 0;
        }
        public string pti2Name(int pti)
        {
            if (pti > -1)
            {
                return this.MyWeirdEngineMoveFinder.piecetypes[pti].name;
            }
            return "";
        }
        public int Name2pti(string piecename)
        {
            int n = this.MyWeirdEngineMoveFinder.piecetypes.Length;
            for (int i = 0; i < n; i++)
            {
                if (this.MyWeirdEngineMoveFinder.piecetypes[i].name == piecename)
                {
                    return i;
                }
            }
            return -1;
        }
        public int MaxPieceSymbolLength()
        {
            int n = this.MyWeirdEngineMoveFinder.piecetypes.Length;
            string s;
            int mylen = 0;
            for (int i = 0; i < n; i++)
            {

                s = this.MyWeirdEngineMoveFinder.piecetypes[i].symbol;
                if (s.Length > mylen)
                {
                    mylen = s.Length;
                }
            }
            return mylen;
        }
        public void jsonchessposition_to_positionstack(jsonchessposition loadedpos, int posidx)
        {
            this.MyWeirdEngineMoveFinder.positionstack[posidx].colourtomove = loadedpos.colourtomove;
            
            for (int z = 0; z < loadedpos.depth_3d; z++)
            {
                for (int j = 0; j < loadedpos.boardheight; j++)
                {
                    int rj = (loadedpos.boardheight - 1) - j;
                    string[] mysymbol = loadedpos.layers[z].squares[rj].Split('|');
                    for (int i = 0; i < loadedpos.boardwidth; i++)
                    {
                        string s = mysymbol[i].TrimStart(' ');
                        this.MyWeirdEngineMoveFinder.positionstack[posidx].squares[i, j, z] = this.Str2PieceType(s);
                    }
                }
            }
        }

        public void LoadPositionJson(string ppath, string pFileName)
        {
            string json;

            try
            {
                using (StreamReader r = new StreamReader(ppath + "\\" + pFileName + ".json"))
                {
                    json = r.ReadToEnd();
                }
            }
            catch
            {
                MessageBox.Show("ppath " + ppath + " pFileName " + pFileName + " problem with loading");
                return;
            }

            jsonchessposition loadedpos;

            dynamic dummy = JsonConvert.DeserializeObject(json);

            loadedpos = JsonConvert.DeserializeObject<jsonchessposition>(json);
            this.MyWeirdEngineMoveFinder.init_positionstack(loadedpos.boardwidth, loadedpos.boardheight, loadedpos.depth_3d);
            jsonchessposition_to_positionstack(loadedpos, 0);
        }


        public jsonchessposition positionstack_to_jsonchessposition(chessposition pposition)
        {
            jsonchessposition mypos = new jsonchessposition();
            mypos.boardwidth = pposition.boardwidth;
            mypos.boardheight = pposition.boardheight;
            mypos.depth_3d = pposition.depth_3d;
            mypos.layers = new jsonlayer[mypos.depth_3d];

            for (int z = 0; z < mypos.depth_3d; z++)
            {
                mypos.layers[z].squares = new string[mypos.boardheight];
            }

            mypos.colourtomove = pposition.colourtomove;

            int targetwidth = this.MaxPieceSymbolLength() + 1;

            for (int z = 0; z < mypos.depth_3d; z++)
            {
                for (int j = 0; j < mypos.boardheight; j++)
                {
                    int rj = (mypos.boardheight - 1) - j;
                    string myvisualrank = "";
                    for (int i = 0; i < mypos.boardwidth; i++)
                    {
                        string mysymbol = this.PieceType2Str(pposition.squares[i, rj, z]);
                        while (mysymbol.Length < targetwidth)
                        {
                            mysymbol = " " + mysymbol;
                        }
                        myvisualrank += mysymbol;
                        if (i < mypos.boardwidth - 1)
                        {
                            myvisualrank += "|";
                        }
                    }
                    mypos.layers[z].squares[j] = myvisualrank;
                }
            }
            return mypos;
        }
        public void SavePositionAsJson(string ppath, string pFileName)
        {
            jsonchessposition mypos = positionstack_to_jsonchessposition(MyWeirdEngineMoveFinder.positionstack[0]);
            string jsonString;

            jsonString = JsonConvert.SerializeObject(mypos, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(ppath + pFileName + ".json"))
            {
                writer.WriteLine(jsonString);
                writer.Close();
            }
        }
        public string Coord2Squarename(int pi, int pj, int pz)
        {
            string myalphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (pi >= 26)
            {
                return "INVALID file number";
            }
            string s = myalphabet.ToLower()[pi].ToString();
            s += (pj + 1).ToString();
            s = (pz + 1).ToString() + ":" + s;
            return s;
        }
        public string ShortNotation(chessmove pmove, bool displayvalue)
        {
            string s = this.PieceType2Str(pmove.MovingPiece).Replace("-", "");
            s += this.Coord2Squarename(pmove.coordinates[0], pmove.coordinates[1], pmove.coordinates[2]);
            if (pmove.IsCapture == true)
            {
                s += "x";
            }
            else
            {
                s += "-";
            }
            s += this.Coord2Squarename(pmove.coordinates[3], pmove.coordinates[4], pmove.coordinates[5]);
            if (pmove.PromoteToPiece != 0)
            {
                s += this.PieceType2Str(pmove.PromoteToPiece).Replace("-", "");
            }

            if (displayvalue == true) { s += "(" + pmove.calculatedvalue.ToString() + ")"; }

            return s;
        }
        public string DisplayMovelist(chessposition pposition, bool displayvalue)
        {
            string s = "";
            for (int movei = 0; movei < pposition.movelist_totalfound; movei++)
            {
                string mvstr = ShortNotation(pposition.movelist[pposition.moveprioindex[movei]], displayvalue);
                if (s == "")
                {
                    s += mvstr;
                }
                else
                {
                    s += "|" + mvstr;
                }
            }
            return s;
        }

        public void LogAllSettings()
        {
            writelog(string.Format("presort_when_depth_gt : {0}",
                MyWeirdEngineMoveFinder.myenginesettings.presort_when_depth_gt));
            writelog(string.Format("setting_SearchForFastestMate : {0}",
                MyWeirdEngineMoveFinder.myenginesettings.setting_SearchForFastestMate));
            writelog(string.Format("presort_using_depth : {0}",
                MyWeirdEngineMoveFinder.myenginesettings.presort_using_depth));
            writelog(string.Format("display_when_depth_gt : {0}",
                MyWeirdEngineMoveFinder.myenginesettings.display_when_depth_gt));
            writelog(string.Format("consult_tt_when_depth_gt : {0}",
                MyWeirdEngineMoveFinder.myenginesettings.consult_tt_when_depth_gt));
            writelog(string.Format("store_in_tt_when_depth_gt : {0}",
                MyWeirdEngineMoveFinder.myenginesettings.store_in_tt_when_depth_gt));
        }
        public void DumpTranspositionTable()
        {
            string jsonString;
            jsonTransTable myposall = new jsonTransTable();
            int av = MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable_no_items_available;
            myposall.TransTable_no_items_available = av;
            myposall.TranspositionTable = new jsonTransTableItem[av];
            for (int p = 0; p < MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable_no_items_available; p++)
            {
                myposall.TranspositionTable[p] = new jsonTransTableItem();
                myposall.TranspositionTable[p].t_position = positionstack_to_jsonchessposition(
                    MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].t_position);
                myposall.TranspositionTable[p].used_depth =
                    MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].used_depth;
                myposall.TranspositionTable[p].used_alpha =
                    MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].used_alpha;
                myposall.TranspositionTable[p].used_beta =
                    MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].used_beta;
                myposall.TranspositionTable[p].calculated_value =
                    MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].calculated_value;
                myposall.TranspositionTable[p].number_of_no_selfcheck_moves =
                    MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].number_of_no_selfcheck_moves;
                myposall.TranspositionTable[p].bestmove_str =
                    ShortNotation(MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].bestmove, true);
                MyWeirdEngineMoveFinder.Init_chessmove(ref myposall.TranspositionTable[p].bestmove);
                MyWeirdEngineMoveFinder.MyWeirdEngineMoveGenerator.SynchronizeChessmove(
                                MyWeirdEngineMoveFinder.MyWeirdEnginePositionCompare.TransTable[p].bestmove,
                                ref myposall.TranspositionTable[p].bestmove);
                myposall.TranspositionTable[p].pos_in_tt = p;
            }
            jsonString = JsonConvert.SerializeObject(myposall, Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(this.jsonworkpath + "log\\"
                                                          + this.logfilename + ".transpositiontable.json"))
            {
                writer.WriteLine(jsonString);
                writer.Close();
            }
        }


    }
}
