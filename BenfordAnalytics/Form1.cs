using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace BenfordAnalytics
{
    public partial class Form1 : Form
    {
        Regex aaa = new Regex("^(0*)");

        private String[] inputstring;
        private double[] inputnumbers;
        private int datasize = 0;
        private double[] percoftwodigits = new double[90];
        private double[] percoffirst = new double[9];
        bool nowprint = false;
        bool inputviafile = true;
        public char[] _splitter;
        double[] nigrinidat1 = { 0.004,0.008,0.012};
        double[] nigrinidat12 = { 0.0006, 0.012, 0.018 };
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void importdata_Click(object sender, EventArgs e)
        {
            _splitter  = textBox1.Text.ToCharArray();
            /*      String tempstring = null;
                  if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                  {
                      System.IO.StreamReader sr = new
               System.IO.StreamReader(openFileDialog1.FileName);
                      tempstring = sr.  ReadToEnd();
                      sr.Close();
                  }*/
            // [V]
            /*     inputnumbers = System.IO.File.ReadAllLines (@"C:\Users\Stefan\Documents\SEMINAR\working\online benford results\inputdata.txt")  //(@"C:\Users\Stefan\Documents\Processing\PROJECTS\Stat\multi2test_2.txt")
          .Select(s => {
              try
                  {s = s.Substring(s.IndexOf(']')+1);
                  BigInteger returnnumber = -1;
                  if (!BigInteger.TryParse(aaa.Replace(s, "") //aaa.Replace("0",""). //Replace(s, "") //s.Replace("^(0*)", "")
                                      .Replace(".", "")
                                      .Replace(",", "")
                                      .Replace("-", ""), out returnnumber)){ return 0;}double returnnumber_ = Convert.ToDouble(returnnumber.ToString());return (double.IsNaN(returnnumber_)) ? 0 : returnnumber_;}catch (Exception en){return 0;}}).ToArray<double>();
      */

            // datasize = inputnumbers.Length;
            /*      char[] splitter = { ' ' };
                  inputstring = (String.Join(" ", System.IO.File.ReadAllLines(@"C:\Users\Stefan\Documents\SEMINAR\working\online benford results\inputdata.txt"))).Split(splitter);
                  inputstring.ToString(); String[] newinputstring = new String[inputstring.Length-1];
                  for(int i = 0; i < inputstring.Length-1; i++)
                  {
                      if(i== inputstring.Length-2)
                      {
                          i.ToString();
                      }
                      newinputstring[i] = inputstring[i];
                  }
                  inputstring = null;
                  inputstring = newinputstring;

           //[V]*/
            /// datasize = inputstring.Length;

            //   inputstring = inputnumbers.Select(s => (new BigInteger(s)).ToString()).ToArray<String>();
            if (inputviafile)
            {
                String tempstring = null;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamReader sr = new
             System.IO.StreamReader(openFileDialog1.FileName);
                    tempstring = sr.ReadToEnd();
                    sr.Close();
                }
                if (tempstring == "" || tempstring == null) { goto BREAKUP; }
                inputstring = tempstring.Split(_splitter);
                String[] newinputstring = new String[inputstring.Length - 1];
                for (int i = 0; i < inputstring.Length - 1; i++)
                {
                    newinputstring[i] = inputstring[i];
                }
                inputstring = null;
                inputstring = newinputstring;

            }
            else
            {
                if(richTextBox1.Text == "") { goto BREAKUP; }
                try
                {
                inputstring = richTextBox1.Text.Split(_splitter);

                }catch(Exception en)
                {
                    goto BREAKUP;
                }
            }
            foreach (var item in inputstring)
            {
                if (item.Length == 1) { richTextBox1.Text = "Alle Zahlen müssen mindestens zwei Stellen haben"; }
            }
            datasize = inputstring.Length;
            inputnumbers = inputstring.Select(s => { return Convert.ToDouble(s); }).ToArray<double>();
            inputnumbers = sort(inputnumbers);
            richTextBox3.Text = "";
            for (int i = 0; i < inputnumbers.Length; i++)
            {
                richTextBox3.Text += i+" " + inputnumbers[i].ToString()+ "\n";
            }



        BREAKUP:
            System.Environment.TickCount.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inputstring!=null)
            {
                richTextBox5.Text = "";
                richTextBox2.Text = "";
                richTextBox4.Text = "";
         
                analysefirst();
                analysefirsttwo();
                testOnExpo();
                richTextBox4.Text += inputstring.Select(s => { return Convert.ToInt32(s); }).ToArray<int>().Sum().ToString();
                panel3.Refresh();
                panel4.Refresh();
            }
        }
        private void analysefirst()
        {
            int[] occurences = new int[9];
            double[] _perc = new double[9];
            int[] firstnumber = inputstring.Select(s =>
           {
               if ((s.ToString())[0] == '0') { return Convert.ToInt32(s.Substring(1, 1)); }
               else
               {
                   return Convert.ToInt32(s.Substring(0,1));
               }
           }).ToArray<int>();
            for(int i = 0; i < occurences.Length; i++)
            {
                occurences[i] = firstnumber.Count(s => (s == i+1));
            }
            _perc = occurences.Select(s =>
            {
                return (100.0 / Convert.ToDouble(occurences.Sum())) * Convert.ToDouble(s);
            }).ToArray<double>();
            percoffirst = _perc;
            foreach (var item in _perc)
            {
                richTextBox5.Text += item.ToString() + "\n";
            }
            double chisquare = 0;
            double tabs = 0;
            double tvar = 0;
            for(int i = 0; i < 9; i++) {
                
                chisquare += Math.Pow((percoffirst[i]/100.0) - (Math.Log10(i + 2)-Math.Log10(i+1)) , 2)/ (Math.Log10(i + 2) - Math.Log10(i + 1)) ;
                tabs += (Math.Abs((percoffirst[i] / 100.0) - (Math.Log10(i + 2) -Math.Log10(i+1))))/9;
                tvar += (Math.Pow((percoffirst[i]/100.0) - (Math.Log10(i + 2) - Math.Log10(i + 1)), 2))/9;

            }
            chisquare *= datasize;
           // tabs /= 9;
           // tvar /= 9;
            label4.Text = tabs.ToString();
            label5.Text = tvar.ToString();
            label6.Text = chisquare.ToString();
            label7.Text = datasize.ToString();
            if (tabs > nigrinidat1[0]) { label8.Text = "Akzeptabel"; if (tabs > nigrinidat1[1]) { label8.Text = "Marginal akz."; if (tabs > nigrinidat1[2]) { label8.Text = "Nicht akz. !"; } } } else { label8.Text = "Gute Konformität!"; }
            richTextBox4.Text += tabs.ToString()+" ";
            richTextBox4.Text += tvar.ToString()+" " ;
            richTextBox4.Text += chisquare.ToString()+" " ;

            
        }

        private void analysefirsttwo() {
            int[] occurrences = new int[90];
            double[] _perc = new double[90];
            int[] firsttwo = inputstring.Select(s =>
            {
                s.ToString();
                if ((s.ToString())[0] == '0') { return Convert.ToInt32(s.Substring(1, 2)); }
                else
                {
                    return Convert.ToInt32(s.Substring(0, 2));
                }
            }).ToArray<int>();

            for (int i = 0; i < occurrences.Length; i++)
            {
                occurrences[i] = firsttwo.Count(s => (s == (i + 10)));
            }
            _perc = occurrences.Select(s =>
            {
                return (100.0 / Convert.ToDouble(occurrences.Sum())) * Convert.ToDouble(s);
            }).ToArray<double>();
            percoftwodigits = _perc;
            nowprint = true;
            panel1.Refresh();
            Update();
        }
        private void testOnExpo()
        {
            double[] quotients = new double[inputnumbers.Length-1];
            for(int i = 0; i < quotients.Length; i++){
                quotients[i] = inputnumbers[i] / inputnumbers[i + 1];
            }

            double mean = quotients.Average();
            double var =0;
            foreach (var item in quotients)
            {
              var +=   Math.Pow(item - mean, 2);
            }         var /= quotients.Length;
            richTextBox4.Text += var.ToString() + " ";
        }
        
        
        /*
         *  5
         *  7
         *  9
         *   10
         *  15
         *  19
         */


        private void panel1_Paint(object sender, PaintEventArgs e)
        { 
            if (nowprint)
            {
                PointF[] emirpoints = new PointF[90];
                
                float heigthvar;
                for(int i = 0; i < emirpoints.Length; i++)
                {

                    emirpoints[i] = new PointF(float.Parse(((panel1.Size.Width-5) - (890-i*10)).ToString()), float.Parse( (( Convert.ToDouble(panel1.Size.Height)/8)*percoftwodigits[i]).ToString()));
                    richTextBox2.Text += percoftwodigits[i] + "\n";
                }
                PointF[] expoints = new PointF[90];
                double chisum = 0;
                double tabssum = 0;
                double tvarsum = 0;
                for(int i = 0; i < expoints.Length; i++)
                {
                    expoints[i] = new PointF(float.Parse(((panel1.Size.Width - 5) - (890 - i * 10)).ToString()), float.Parse( (( Convert.ToDouble(panel1.Size.Height)/8)*  ((Math.Log10(i+11) - Math.Log10(i+10))*100 ) )  .ToString()));
                   // richTextBox3.Text += ((Math.Log10(i + 11) - Math.Log10(i + 10)) * 100) + "\n";
                    chisum  += Math.Pow((percoftwodigits[i] / 100.0) - ((Math.Log10(i + 11) - Math.Log10(i + 10)) ),2) / (Math.Log10(i + 11) - Math.Log10(i + 10)) ;
                    tabssum += Math.Abs((percoftwodigits[i] / 100.0) - (Math.Log10(i + 11) - Math.Log10(i + 10)));
                    tvarsum += Math.Pow((percoftwodigits[i] / 100.0) - (Math.Log10(i + 11) - Math.Log10(i + 10)),2);
                }
                chisum *= datasize;
                tabssum /= 90;
                tvarsum /= 90;
                label3.Text = chisum.ToString();
                label1.Text = tabssum.ToString();
                label2.Text = tvarsum.ToString();
                if(tabssum > nigrinidat12[0]) { label22.Text = "Akzeptabel"; if (tabssum > nigrinidat12[1]) { label22.Text = "Marginal akzp.";if (tabssum > nigrinidat12[2]) { label22.Text = "Nicht akzeptabel!"; }  } } else { label22.Text = "Perfekte Konformität!"; }
                richTextBox4.Text += tabssum.ToString()+" " ;
                richTextBox4.Text += tvarsum.ToString()+" " ;
                richTextBox4.Text += chisum.ToString()+" " ;
                e.Graphics.DrawLines(new Pen(Color.Blue, 1),emirpoints);
                e.Graphics.DrawLines(new Pen(Color.Red, 1), expoints);
                
            }
        }

        private void chart1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void chart1_PostPaint(object sender, System.Windows.Forms.DataVisualization.Charting.ChartPaintEventArgs e)
        {
           
        }
        double[] sort(double[] var1)  //TODO: use a heap sort
        {
            foreach (var item in var1)
            {
                for (int i = 0; i < var1.Length - 1; i++)
                {
                    if (var1[i] > var1[i + 1])
                    {
                        double vard = var1[i];
                        var1[i] = var1[i + 1];
                        var1[i + 1] = vard;
                    }
                }
            }
            return var1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            inputviafile = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            inputviafile = false;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Blue);
            Pen pen2 = new Pen(Color.Red);
            if (nowprint)
            {
                for(int i = 0; i < percoffirst.Length; i++)
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(10 + i * 30, panel3.Height - (4*Convert.ToInt32(percoffirst[i])), 10, 4* Convert.ToInt32(percoffirst[i])));
                    e.Graphics.DrawRectangle(pen2, new Rectangle(20 + i * 30, panel3.Height - (4 * Convert.ToInt32(   (Math.Log10(i + 2) - Math.Log10(i + 1))*100  )), 10, (4 * Convert.ToInt32((Math.Log10(i + 2) - Math.Log10(i + 1)) * 100))));
                    //(Math.Log10(i + 2) - Math.Log10(i + 1))
                }

            }
        }
        double[] sortback(double[] var1)  //TODO: use a heap sort
        {
            foreach (var item in var1)
            {
                for (int i = 0; i < var1.Length - 1; i++)
                {
                    if (var1[i] < var1[i + 1])
                    {
                        double vard = var1[i];
                        var1[i] = var1[i + 1];
                        var1[i + 1] = vard;
                    }
                }
            }
            return var1;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            if (nowprint)
            {
                double[] logModOne = new double[inputnumbers.Length];
                for (int i = 0; i < logModOne.Length; i++)
                {
                    logModOne[i] = (Math.Log10((inputnumbers[i])) % 1) * 416;
                }
                logModOne = sortback(logModOne);
                Pen pen = new Pen(Color.Green);
                Random rnd = new Random();
                int var3 = 1;
                try
                {

                    ArrayList currentvertnumbers = new ArrayList();
                    int currentvert = -1;
                    for (int i = 0; i < logModOne.Length; i++) //163; 327
                    {
                        if (currentvert != (int)(Math.Round(3 + ((panel4.Size.Height - 3) / logModOne[0]) * logModOne[i])))
                        {
                            currentvertnumbers.Clear();
                            currentvert = (int)(Math.Round(3 + ((panel4.Size.Height - 3) / logModOne[0]) * logModOne[i]));
                        }
                        int newrandom = -1;
                        do
                        {
                            newrandom = rnd.Next(1, panel4.Size.Width - 1);
                        } while (currentvertnumbers.Contains(newrandom));
                        currentvertnumbers.Add(rnd.Next(1, panel4.Size.Width - 1));

                        e.Graphics.DrawEllipse(pen, newrandom, (int)(Math.Round(3 + ((panel4.Size.Height - 3) / logModOne[0]) * logModOne[i])), 1, 1); //(int)((1/logModOne[logModOne.Length-1])*logModOne[i])
                    }
                }
                catch (Exception en)
                {

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel4.Width > 10)
            {
                panel4.Width -= 10;
                panel4.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panel4.Width < 297)
            {
                panel4.Width += 10;
                panel4.Refresh();
            }
        }
    }
}
