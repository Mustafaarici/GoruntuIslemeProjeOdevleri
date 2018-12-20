using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision;
using AForge.Vision.Motion;
using System.Threading;
namespace GörüntüİşlemeLedUygulaması
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            


        }
        int red, blue, green;
        int objectX, objectY;
        Bitmap renk;
        bool kontrol;
     
        private VideoCaptureDevice pccam; // Kulanacağımız aygıt.
        private FilterInfoCollection pccamera; // Pc' de bulunan cameraları tutan bir dizi.
        BlobCounter blobCounter = new BlobCounter(); // Nesne tespiti değişkeni.
        private void Form1_Load(object sender, EventArgs e)
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < System.IO.Ports.SerialPort.GetPortNames().Length; i++)
            {
                comboBox1.Items.Add(System.IO.Ports.SerialPort.GetPortNames()[i]);  //Portlar combobox'a yazdırılır.
            }
            pccamera = new FilterInfoCollection(FilterCategory.VideoInputDevice); //Sisteme bagli olan Cam listesini aliyoruz
            foreach (FilterInfo VideoCaptureDevice in pccamera)
            {
                kameralistele.Items.Add(VideoCaptureDevice.Name); //PC'deki Kameralar hepsi ComboBox'da listelenir.
                kameralistele.SelectedIndex = 0;
            }
            timer1.Start(); // Ayarlanan rengi görüntülemek için kullanılmıştır.
        }
        private void pccam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            image = new Mirror(false, true).Apply(image);  
            pictureBox1.Image = image;

                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(red,green,blue)); // Algılanacak Renk ve merkez noktası bulunur.
                filter.Radius = 80;
                filter.ApplyInPlace(image1);//Filitre Çalıştırılır.     
                image1 = new Mirror(false, true).Apply(image1);
                cevreal(image1);// Algilanan rengi Çevrçevelemek veya hedeflemek için gerekli Method.
        }
        private void cevreal(Bitmap image)// Algilanan rengi Çevrçevelemek veya hedeflemek için gerekli Method.
        {
            
            blobCounter.MinWidth = 2;
            blobCounter.MinHeight = 2;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;
            Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grayImage = grayFilter.Apply(image);
            blobCounter.ProcessImage(grayImage);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    Graphics g = pictureBox1.CreateGraphics();
                    using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                    {

                        g.DrawRectangle(pen, objectRect);
                    }
                     objectX = objectRect.X + (objectRect.Width / 2); //Dikdörtgenin Koordinatlari alınır.
                     objectY = objectRect.Y + (objectRect.Height / 2);//Dikdörtgenin Koordinatlari alınır.
                    g.DrawString(objectX.ToString() + "X" + objectY.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(250, 1));
                    g.Dispose();
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            red = trackBar1.Value; // Kırmızı renk değeri.
            green = trackBar2.Value; // Yeşil renk değeri.
            blue = trackBar3.Value; // Mavi renk değeri.
            label4.Text = red.ToString(); // Renklerin sayısal değerleri.
            label5.Text = green.ToString();
            label6.Text = blue.ToString();
            Color renk = Color.FromArgb(red,green,blue); // Belirlenen değerler ile yeni renk oluşturuldu.
            pictureBox2.BackColor = renk; // arkaplan rengi oluşturulan renk olarak belirlendi.
        }

        private void timer2_Tick(object sender, EventArgs e)
        { 
            if(blobCounter.ObjectsCount > 0)   // Eğer bir nesne algılandı ise.
            {
                kontrol = true;
            }
            else
            {
                kontrol = false;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
          
            if(objectX == 0 && objectY== 0 && kontrol == true)
            {
                serialPort1.Write("b");
            }
            if (objectX > 0 && objectY > 0 && objectX <= 200 && objectY <= 154 && kontrol == true)
            {
                serialPort1.Write("a");
                panel1.BackColor = Color.Green;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 200 && objectX <= 400 && objectY <= 154 && kontrol == true)
            {
                serialPort1.Write("c");
                panel2.BackColor = Color.Green;
                panel1.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 400 && objectX <= 600 && objectY <= 154 && kontrol == true)
            {
                serialPort1.Write("d");
                panel3.BackColor = Color.Green;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 0 && objectX <= 200 && objectY >= 154 && objectY <= 308 && kontrol == true)
            {
                serialPort1.Write("e");
                panel4.BackColor = Color.Yellow;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 200 && objectX <= 400 && objectY >= 154 && objectY <= 308 && kontrol == true)
            {
                serialPort1.Write("f");
                panel5.BackColor = Color.Yellow;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 400 && objectX <= 600 && objectY >= 154 && objectY <= 308 && kontrol == true)
            {
                serialPort1.Write("g");
                panel6.BackColor = Color.Yellow;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 0 && objectX <= 200 && objectY >= 308 && objectY <= 462 && kontrol == true)
            {
                serialPort1.Write("h");
                panel7.BackColor = Color.Blue;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 200 && objectX <= 400 && objectY >= 308 && objectY <= 462 && kontrol == true)
            {
                serialPort1.Write("i");
                panel8.BackColor = Color.Blue;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel9.BackColor = Color.Gray;
            }
            if (objectX >= 400 && objectX <= 600 && objectY >= 308 && objectY <= 462 && kontrol == true)
            {
                serialPort1.Write("l");
                panel9.BackColor = Color.Blue;
                panel1.BackColor = Color.Gray;
                panel2.BackColor = Color.Gray;
                panel3.BackColor = Color.Gray;
                panel4.BackColor = Color.Gray;
                panel5.BackColor = Color.Gray;
                panel6.BackColor = Color.Gray;
                panel7.BackColor = Color.Gray;
                panel8.BackColor = Color.Gray;
            }
            else
            {
                serialPort1.Write("b");
            }
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            try
            {            
                pccam = new VideoCaptureDevice(pccamera[kameralistele.SelectedIndex].MonikerString);
                pccam.NewFrame += new NewFrameEventHandler(pccam_NewFrame);

                pccam.DesiredFrameRate = 30;          // Ekran Görüntü kalitesi için.
                pccam.DesiredFrameSize = new Size(483, 379);  // Ekran Görüntü büyüklüğü için.
                pccam.Start();
                timer2.Start();                
            }
            catch (Exception)//Kamera bulunmaması durumnda.
            {
                MessageBox.Show("HİÇ KAMERA BULUNAMADI", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
                MessageBox.Show("Bağlantı Kuruldu.");
            }
            catch
            {
                MessageBox.Show("Bağlantı Kurulamadı.");
            }
            
        }
    }
}
