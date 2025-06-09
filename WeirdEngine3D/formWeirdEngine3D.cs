using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheWeirdEngine;

namespace WeirdEngine3D
{
    public partial class formWeirdEngine3D : Form
    {
        public WeirdEngineMoveFinder MyWeirdEngineMoveFinder;
        public WeirdEngineJson MyWeirdEngineJson;
        public formWeirdEngine3D()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Show Resources location Under construction");
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            MyWeirdEngineJson.LoadEngineSettingsFromJson("enginesettings");
            this.RefreshInformation();
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Switch PieceTypes Under construction");
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            this.DisableGUI();
            MyWeirdEngineJson.Load_UI_Input_Json(this.textBox1.Text);
            string s = MyWeirdEngineJson.MyUI_Input.positionfilename.Replace(".json", "");
            //MessageBox.Show(MyWeirdEngineJson.MyUI_Input.positionfilename);
            //MessageBox.Show(MyWeirdEngineJson.MyUI_Input.depth.ToString());
            MyWeirdEngineJson.LoadPositionJson(MyWeirdEngineJson.jsonsourcepath + "positions", s);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", s);
            calculationresponse a = MyWeirdEngineMoveFinder.Calculation_tree(MyWeirdEngineJson.MyUI_Input.depth);
            this.RefreshInformation();
            MessageBox.Show(MyWeirdEngineMoveFinder.FinalResponseLogString(a));
            this.EnableGUI();
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Show legal moves Under construction");
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            this.DisableGUI();
            MyWeirdEngineJson.Load_UI_Input_Json(this.textBox1.Text);
            string s = MyWeirdEngineJson.MyUI_Input.positionfilename.Replace(".json", "");
            MessageBox.Show(MyWeirdEngineJson.MyUI_Input.positionfilename + "/" +
                MyWeirdEngineJson.MyUI_Input.depth.ToString());
            MyWeirdEngineJson.LoadPositionJson(MyWeirdEngineJson.jsonsourcepath + "positions", s);
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", s);
            this.RefreshInformation();
            this.EnableGUI();
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            mfunittests Mymfunittests;
            string infilename = "unittestgame";
            MyWeirdEngineJson.LoadPieceTypesFromJson(infilename);
            MyWeirdEngineJson.SavePieceTypesAsJson(infilename);
            this.DisableGUI();
            string unittestpath = this.MyWeirdEngineJson.jsonsourcepath + "unittests";
            Mymfunittests = new mfunittests(this.MyWeirdEngineMoveFinder, this.MyWeirdEngineJson);
            Mymfunittests.RunAllUnittests(unittestpath);
            this.RefreshInformation();
            this.EnableGUI();
        }

        private void DisableGUI()
        {
            this.menuItem2.Enabled = false;
            this.menuItem3.Enabled = false;
            this.menuItem4.Enabled = false;
            this.menuItem5.Enabled = false;
            this.textBox1.Enabled = false;
            this.btnAbort.Enabled = true;
        }
        private void EnableGUI()
        {
            this.menuItem2.Enabled = true;
            this.menuItem3.Enabled = true;
            this.menuItem4.Enabled = true;
            this.menuItem5.Enabled = true;
            this.textBox1.Enabled = true;
            this.btnAbort.Enabled = false;
        }
        private void formWeirdEngine3D_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.BringToFront();

            MyWeirdEngineMoveFinder = new WeirdEngineMoveFinder();
            MyWeirdEngineJson = new WeirdEngineJson(this.MyWeirdEngineMoveFinder);
            MyWeirdEngineMoveFinder.MyWeirdEngineJson = this.MyWeirdEngineJson;//Just a reference
            MyWeirdEngineJson.jsonsourcepath = "C:\\Users\\Evert Jan\\pythonprojects\\chesspython3d\\";
            MyWeirdEngineJson.jsonworkpath = "C:\\Users\\Evert Jan\\pythonprojects\\chesspython3d\\";
            string infilename = "game3d";
            MyWeirdEngineJson.LoadPieceTypesFromJson(infilename);
            MyWeirdEngineJson.SavePieceTypesAsJson(infilename);
            MyWeirdEngineJson.LoadPositionJson(MyWeirdEngineJson.jsonsourcepath + "positions", "my3Dposition");
            MyWeirdEngineJson.SavePositionAsJson(MyWeirdEngineJson.jsonworkpath + "positions_verify\\", "my3Dposition");
            MyWeirdEngineJson.LoadEngineSettingsFromJson("enginesettings");
            this.RefreshInformation();
        }
        private void RefreshInformation()
        {
            this.lblInformation.Text = MyWeirdEngineJson.DisplayInformation();
        }

        private void menuItem12_Click(object sender, EventArgs e)
        {
            mfunittests Mymfunittests;
            string infilename = "unittestgame";
            MyWeirdEngineJson.LoadPieceTypesFromJson(infilename);
            MyWeirdEngineJson.SavePieceTypesAsJson(infilename);
            this.DisableGUI();
            string unittestpath = this.MyWeirdEngineJson.jsonsourcepath + "unittests";
            Mymfunittests = new mfunittests(this.MyWeirdEngineMoveFinder, this.MyWeirdEngineJson);
            Mymfunittests.RunNewUnittests(unittestpath);
            this.RefreshInformation();
            this.EnableGUI();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            MyWeirdEngineMoveFinder.externalabort = true;
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            mfunittests Mymfunittests;
            string infilename = "unittestgame";
            MyWeirdEngineJson.LoadPieceTypesFromJson(infilename);
            MyWeirdEngineJson.SavePieceTypesAsJson(infilename);
            this.DisableGUI();
            string unittestpath = this.MyWeirdEngineJson.jsonsourcepath + "unittests";
            Mymfunittests = new mfunittests(this.MyWeirdEngineMoveFinder, this.MyWeirdEngineJson);
            Mymfunittests.RunPerformancetests(unittestpath);
            this.RefreshInformation();
            this.EnableGUI();
        }
    }
}
